using UnityEngine;

[RequireComponent(typeof(Terrain))]
public class TerrainLoader : MonoBehaviour
{
    // for terrain loading
    public int xOffset;
    public int yOffset;

    private Terrain terrain;

    void Start()
    {
        terrain = gameObject.GetComponent<Terrain>();
        if (GeoDataLoader.IsLoaded)
        {
            terrain.terrainData.SetHeights(0, 0,
                GeoDataLoader.Slice(terrain.terrainData.heightmapResolution - 1, xOffset, yOffset));
        }
    }
}