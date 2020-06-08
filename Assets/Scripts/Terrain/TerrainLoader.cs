using UnityEngine;

public class TerrainLoader : MonoBehaviour
{
    public string path;

    // for terrain loading
    public int xOffset;
    public int yOffset;

    public int resolution;
    private Terrain terrain;

    void Start()
    {
        terrain = gameObject.GetComponent<Terrain>();

        if (terrain == null)
        {
            Debug.LogWarning("Selected object " + gameObject.name + " is't Terrain. Please, select Terrain ");
            return;
        }

        terrain.terrainData.SetHeights(0, 0,
            GeoDataLoader.Slice(terrain.terrainData.heightmapResolution - 1, xOffset, yOffset));
    }
}