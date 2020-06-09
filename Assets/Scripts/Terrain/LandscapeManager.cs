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

    private TerrainLoader _terrainLoader;
    private GameObject[,] _terrains;

    public void Start()
    {
        _terrainLoader = terrainPrefab.GetComponent<TerrainLoader>();

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

    private bool flag = false;
    
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

            if (Width > 1 && !flag)
            {
                var left = _terrains[i, Width - 1].GetComponent<Terrain>();
                var right = _terrains[i, 0].GetComponent<Terrain>();
                ArraySticher.Stich(left, right, TerrainResolution);

                for (int j = 1; j < Width; j++)
                {
                    left = _terrains[i, j - 1].GetComponent<Terrain>();
                    right = _terrains[i, j].GetComponent<Terrain>();
                    ArraySticher.Stich(left, right, TerrainResolution);
                }

                flag = true;
            }
            
            yield return new WaitForSeconds(0);
        }
    }



    void CreateTiles()
    {
        for (var i = 0; i < Height; i++)
        {
            for (var j = 0; j < Width; j++)
            {
                var terrain = CreateNewTerrainFrom(terrainPrefab);

                terrain.transform.SetParent(transform, false);
                terrain.transform.Translate(new Vector3(j * TerrainResolution, 0, i * TerrainResolution), Space.Self);

                _terrainLoader.yOffset += TerrainResolution;

                _terrains[i, j] = terrain;
            }

            _terrainLoader.yOffset = TerrainResolution * WidthOffset;
            _terrainLoader.xOffset += TerrainResolution;
        }
    }

    private GameObject CreateNewTerrainFrom(Terrain copyTerrain)
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
        Debug.Log(terrain.transform.position);

        CopyComponent(copyTerrain.GetComponent<TerrainLoader>(), terrain);
        CopyComponent(copyTerrain.GetComponent<InitTerrainLayers>(), terrain);
        CopyComponent(copyTerrain.GetComponent<AssignSplatMap>(), terrain);
        CopyComponent(copyTerrain.GetComponent<AddFloraToTerrain>(), terrain);
        return terrain;
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