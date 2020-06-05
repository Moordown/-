using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


[RequireComponent(typeof(Terrain))]
public class TerrainLoader : MonoBehaviour
{
    public string path;


    public float GrassMin;
    public float GrassMax;
    public GameObject[] GrassComponents;
    public float[] GrassComponentWeights;

    // for terrain loading
    public int xOffset;
    public int zOffset;

    public int xSize;
    public int ySize;
    public int zSize;
    private GeoDataLoader loader;
    private Terrain terrain;

    void Start()
    {
        terrain = gameObject.GetComponent<Terrain>();

        if (terrain == null)
        {
            Debug.LogWarning("Selected object " + gameObject.name + " is't Terrain. Please, select Terrain ");
            return;
        }

        loader = new GeoDataLoader();

        if (!loader.load(path, terrain.terrainData.heightmapResolution, zOffset, xOffset))
            Debug.LogError("Geo data loading is failed");
        else
        {
            Debug.Log("Geo data loaded successfully");
            terrain.terrainData.size = new Vector3(xSize, ySize, zSize);
            terrain.terrainData.SetHeights(0, 0, loader.heightMap);
            PopulateLayers();
        }
    }

    void PopulateLayers()
    {
        var heightZRange = loader.heightMap.GetLength(0);
        var heightXRange = loader.heightMap.GetLength(1);

        var xm = (float) heightXRange / xSize;
        var zm = (float) heightZRange / zSize;
        
        for (var z = 0; z < loader.heightMap.GetLength(0); z++)
        for (var x = 0; x < loader.heightMap.GetLength(1); x++)
        {
            var y = loader.heightMap[z, x];
            if (GrassMin < y && y < GrassMax)
                TryAddInstance(x / xm, y * ySize, z / zm, GrassComponents, GrassComponentWeights);
        }
    }

    void TryAddInstance(float x, float y, float z, GameObject[] components, float[] weights)
    {
        var s = 0f;
        var r = Random.value;

        for (var k = 0; k < components.Length; k++)
        {
            s += weights[k];
            if (!(s > r)) continue;
            var instance = Instantiate(components[k], transform, true);
            instance.transform.Translate(new Vector3(x + zOffset, y, z), Space.Self);
            instance.SetActive(true);
            break;
        }
    }
}