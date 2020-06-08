using System;
using System.Collections;
using UnityEngine;


public class MapCreator : MonoBehaviour
{
    public Terrain terrainPrefab;
    public int TerrainResolution;
    public int Width;
    public int Height;
    public string GeoDataPath;

    private TerrainLoader _terrainLoader;
    private InitTerrainLayers _initTerrainLayers;
    private AssignSplatMap _assignSplatMap;
    private AddFloraToTerrain _addFloraToTerrain;

    public void Start()
    {
        _assignSplatMap = terrainPrefab.GetComponent<AssignSplatMap>();
        _initTerrainLayers = terrainPrefab.GetComponent<InitTerrainLayers>();
        _terrainLoader = terrainPrefab.GetComponent<TerrainLoader>();
        _addFloraToTerrain = terrainPrefab.GetComponent<AddFloraToTerrain>();

        _terrainLoader.xOffset = 0;
        _terrainLoader.yOffset = 0;

        GeoDataLoader.Load(GeoDataPath);
        CreateTiles();

        _terrainLoader.xOffset = 0;
        _terrainLoader.yOffset = 0;
    }

    void CreateTiles()
    {
        var terrains = new Terrain[Height, Width];

        for (var i = 0; i < Height; i++)
        {
            for (var j = 0; j < Width; j++)
            {
                var terrainData = new TerrainData {heightmapResolution = TerrainResolution};
                terrainData.SetHeights(0, 0,
                    terrainPrefab.terrainData.GetHeights(0, 0, TerrainResolution, TerrainResolution));
                terrainData.size = terrainPrefab.terrainData.size;
                terrainData.alphamapResolution = TerrainResolution;

                var terrain = Terrain.CreateTerrainGameObject(terrainData);
                terrain.transform.SetParent(transform, false);
                terrain.transform.Translate(new Vector3(j * TerrainResolution, 0, i * TerrainResolution), Space.Self);

                Debug.Log(terrain.transform.position);
                
                CopyComponent(_terrainLoader, terrain);
                CopyComponent(_initTerrainLayers, terrain);
                CopyComponent(_assignSplatMap, terrain);
                CopyComponent(_addFloraToTerrain, terrain);

                _terrainLoader.yOffset += TerrainResolution;

                terrains[i, j] = terrain.GetComponent<Terrain>();
            }

            _terrainLoader.yOffset = 0;
            _terrainLoader.xOffset += TerrainResolution;
        }

        for (var i = 0; i < Height; i++)
        {
            for (var j = 0; j < Width; j++)
            {
                var left = j == 0 ? null : terrains[i, j - 1];
                var right = j == Width - 1 ? null : terrains[i, j + 1];
                var top = i == Height - 1 ? null : terrains[i + 1, j];
                var bottom = i == 0 ? null : terrains[i - 1, j];
                
                terrains[i, j].SetNeighbors(left, top, right, bottom);
            }
        }
    }

    T CopyComponent<T>(T original, GameObject destination) where T : Component
    {
        System.Type type = original.GetType();
        Component copy = destination.AddComponent(type);
        System.Reflection.FieldInfo[] fields = type.GetFields();
        foreach (System.Reflection.FieldInfo field in fields)
        {
            field.SetValue(copy, field.GetValue(original));
        }

        return copy as T;
    }
}