using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LandscapeManager : MonoBehaviour
{
    public Terrain terrainPrefab;
    public int TerrainResolution;
    public int TerrainHeight;

    public int Width;
    public int Height;

    public int WidthOffset;
    public int HeightOffset;

    public string GeoDataPath;

    public float MoveSpeed;
    public float RoadLevel;
    
    public int StichDistance;
    public int OddPathWidth;

    private TerrainLoader _terrainLoader;
    private AssignSplatMap _assignSplatMap;
    private AddFloraToTerrain _addFloraToTerrain;
    private InitTerrainLayers _initTerrainLayers;
    private OneByOneDieComponent _oneByOneDieComponent;
    private PushInTied _pushInTied;
    
    private GameObject[,] _terrains;

    public void Start()
    {
        _terrainLoader = terrainPrefab.GetComponent<TerrainLoader>();
        _assignSplatMap = terrainPrefab.GetComponent<AssignSplatMap>();
        _oneByOneDieComponent = terrainPrefab.GetComponent<OneByOneDieComponent>();
        _pushInTied = terrainPrefab.GetComponent<PushInTied>();
        
        _addFloraToTerrain = terrainPrefab.GetComponent<AddFloraToTerrain>();
        var (top, bottom) = TerrainManipulator.GetTopBottomBorderForRoad(TerrainResolution, OddPathWidth);
        _addFloraToTerrain.RoadTop = top;
        _addFloraToTerrain.RoadBottom = bottom;
        
        _initTerrainLayers = terrainPrefab.GetComponent<InitTerrainLayers>();

        _terrainLoader.xOffset = TerrainResolution * HeightOffset;
        _terrainLoader.yOffset = TerrainResolution * WidthOffset;

        GeoDataLoader.Load(GeoDataPath);

        _terrains = new GameObject[Height, Width];
        CreateTiles();

        _terrainLoader.xOffset = 0;
        _terrainLoader.yOffset = 0;
    }

    void Update()
    {
        // Update position
        foreach (var terrain in _terrains)
        {
            terrain.transform.Translate(MoveSpeed * Time.deltaTime * Vector3.left);
        }

        // Update ring
        if ((_terrains[0, 0].transform.position - transform.position).magnitude > TerrainResolution)
        {
            StartCoroutine(UpdateRing());
        }
    }

    IEnumerator UpdateRing()
    {
        for (var i = 0; i < Height; i++)
        {
            var rightTerrain = _terrains[i, 0];
            for (var j = 1; j < Width; j++)
                _terrains[i, j - 1] = _terrains[i, j];
            _terrains[i, Width - 1] = rightTerrain;
        }
        
        yield return new WaitForSeconds(0);

        for(int i=0; i<Height; i++)
        {
            _terrains[i, Width - 1].transform.SetParent(transform, false);
            _terrains[i, Width - 1].transform.Translate(
                new Vector3((Width) * TerrainResolution, 0, i * TerrainResolution), Space.Self);
            yield return new WaitForSeconds(0);
        }
    }


    void CreateTiles()
    {
        for (var i = 0; i < Height; i++)
        {
            for (var j = 0; j < Width; j++)
            {
                var terrain = CreateNewTerrainFrom();

                terrain.transform.SetParent(transform, false);
                terrain.transform.Translate(new Vector3(j * TerrainResolution, 0, i * TerrainResolution), Space.Self);

                _terrainLoader.yOffset += TerrainResolution;

                _terrains[i, j] = terrain;
            }

            _terrainLoader.yOffset = TerrainResolution * WidthOffset;
            _terrainLoader.xOffset += TerrainResolution;
        }

        StartCoroutine(PopulateTerrain());
    }
    
    private GameObject CreateNewTerrainFrom()
    {
        var terrainData = new TerrainData
        {
            heightmapResolution = TerrainResolution + 1,
            alphamapResolution = TerrainResolution,
            size = new Vector3(TerrainResolution, TerrainHeight, TerrainResolution)
        };
        terrainData.SetHeights(0, 0,
            terrainPrefab.terrainData.GetHeights(0, 0, TerrainResolution, TerrainResolution));
        var terrain = Terrain.CreateTerrainGameObject(terrainData);

        CopyComponent(_terrainLoader, terrain);
        CopyComponent(_oneByOneDieComponent, terrain);
        CopyComponent(_pushInTied, terrain);
        
        return terrain;
    }


    private IEnumerator PopulateTerrain()
    {
        // Видимо unity не успевает обработать terrain, поэтому мы берем задержку,
        // пока он его создаст, чтобы потом сшить остальные terrain
        yield return new WaitForSeconds(0);
        StichTerrains();
        foreach (var terrain in _terrains)
        {
            TerrainManipulator.MakePath(terrain.GetComponent<Terrain>(), TerrainResolution, OddPathWidth, RoadLevel);
            CopyComponent(_initTerrainLayers, terrain);
            CopyComponent(_assignSplatMap, terrain);
            CopyComponent(_addFloraToTerrain, terrain);
        }
    }
    
    
    private void StichTerrains()
    {
        if (Width <= 1) return;
        for (var i = 0; i < Height; i++)
        {
            var left = _terrains[i, Width - 1].GetComponent<Terrain>();
            var right = _terrains[i, 0].GetComponent<Terrain>();
            TerrainManipulator.Stich(left, right, TerrainResolution, StichDistance);

            for (var j = 1; j < Width; j++)
            {
                left = _terrains[i, j - 1].GetComponent<Terrain>();
                right = _terrains[i, j].GetComponent<Terrain>();
                TerrainManipulator.Stich(left, right, TerrainResolution, StichDistance);
            }
        }
    }
    
    private static void CopyComponent<T>(T original, GameObject destination) where T : Component
    {
        var type = original.GetType();
        var copy = destination.AddComponent(type);
        var fields = type.GetFields();
        foreach (var field in fields)
        {
            field.SetValue(copy, field.GetValue(original));
        }
    }
}