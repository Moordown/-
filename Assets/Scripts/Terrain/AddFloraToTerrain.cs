using System;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Terrain))]
public class AddFloraToTerrain : MonoBehaviour
{
    public GameObject[] Components;
    public float[] ComponentWeights;
    public float MinGrassValueForPopulation;

    public int RoadTop;
    public int RoadBottom;

    public void Start()
    {
        var terrain = GetComponent<Terrain>();
        var data = terrain.terrainData;

        var splatMap = data.GetAlphamaps(0, 0, data.alphamapWidth, data.alphamapHeight);
        for (var alphaX = 0; alphaX < data.alphamapWidth; alphaX++)
        {
            foreach (var (left, right) in new[] {(0, RoadTop), (RoadBottom, data.alphamapResolution)})
            {
                for (var alphaY = left; alphaY < right; alphaY++)
                {
                    if (splatMap[alphaX, alphaY, (int) LayerId.GrassId] < MinGrassValueForPopulation) continue;
                        // if (MaxIndex(alphaY, alphaX, data.alphamapLayers, splatMap) != (int) LayerId.GrassId)
                        // continue;
                    if (Mathf.Abs(data.GetHeight(alphaX, alphaY)) < 0.01) continue;
                    TryAddInstance(alphaX, data.GetHeight(alphaX, alphaY), alphaY);
                }
            }
        }
    }

    public int MaxIndex(int x, int y, int length, float[,,] arr)
    {
        float max = 0;
        int id = 0;
        for (int i = 0; i < length; i++)
        {
            if (arr[x, y, i] > max)
            {
                id = i;
                max = arr[x, y, i];
            }
        }

        return id;
    }

    void TryAddInstance(float x, float y, float z)
    {
        var s = 0f;
        var r = Random.value;

        for (var k = 0; k < Components.Length; k++)
        {
            s += ComponentWeights[k];
            if (!(s > r)) continue;
            var instance = Instantiate(Components[k], transform, false);
            instance.transform.Translate(new Vector3(x, y, z), Space.Self);
            instance.SetActive(true);
            break;
        }
    }
}