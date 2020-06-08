using UnityEngine;

public class InitTerrainLayers : MonoBehaviour
{
    public TerrainLayer GrassLayer;
    public TerrainLayer GroundLayer;
    public TerrainLayer DustLayer;
    public TerrainLayer RockLayer;
    public TerrainLayer SnowLayer;

    void Start()
    {
        // Get the attached terrain component
        var terrain = GetComponent<Terrain>();

        // Get a reference to the terrain data
        var terrainData = terrain.terrainData;
        terrainData.terrainLayers = CreateTerrainLayers();
    }
    
    TerrainLayer[] CreateTerrainLayers()
    {
        var terrainLayers = new TerrainLayer[5];
        terrainLayers[(int) LayerId.GrassId] = GrassLayer;
        terrainLayers[(int) LayerId.GroundId] = GroundLayer;
        terrainLayers[(int) LayerId.DustId] = DustLayer;
        terrainLayers[(int) LayerId.SnowId] = SnowLayer;
        terrainLayers[(int) LayerId.RockId] = RockLayer;

        return terrainLayers;
    }
}
