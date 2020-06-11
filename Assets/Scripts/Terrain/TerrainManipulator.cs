using UnityEngine;

public static class TerrainManipulator
{
    public static void Stich(Terrain left, Terrain right, int terrainResolution, int stichDistance)
    {
        var a = left.terrainData.GetHeights(0, 0, terrainResolution + 1, terrainResolution + 1);
        var b = right.terrainData.GetHeights(0, 0, terrainResolution + 1, terrainResolution + 1);
        Stich(a, b, terrainResolution, stichDistance);
        left.terrainData.SetHeights(0, 0, a);
        right.terrainData.SetHeights(0, 0, b);
    }

    public static (int, int) GetTopBottomBorderForRoad(int terrainResolution, int oddPathWidth)
    {
        var top = terrainResolution / 2 - oddPathWidth / 2 + 1;
        var bottom = top + oddPathWidth - 1;
        return (top, bottom);
    }
    
    public static (int, int) GetSmoothedTopBottomBorderForRoad(int terrainResolution, int oddPathWidth)
    {
        var top = terrainResolution / 2 - oddPathWidth / 2 + 1;
        var bottom = top + oddPathWidth - 1;
        var smoothSide = (oddPathWidth - 1) / 3;
        return (top + smoothSide, bottom - smoothSide);
    }

    // TODO: добавить произвольную линию
    public static void MakePath(Terrain terrain, int terrainResolution, int oddPathWidth, float roadLevel)
    {
        var heights = terrain.terrainData.GetHeights(0, 0, terrainResolution + 1, terrainResolution + 1);
        var (top, bottom) = GetTopBottomBorderForRoad(terrainResolution, oddPathWidth);

        for (var i = top; i <= bottom; i++)
        for (var j = 0; j <= terrainResolution; j++)
            heights[i, j] = roadLevel;

        var smoothSide = (oddPathWidth - 1) / 3;
        for (var j = 0; j <= terrainResolution; j++)
        {
            foreach (var fixedPoint in new[] {top, bottom})
            {
                for (var i = 1; i <= smoothSide; i++)
                {
                    var average = heights[fixedPoint + i, j] / 2 + heights[fixedPoint - i, j] / 2;
                    var weight = Mathf.SmoothStep(1f, 0f, (float) i / smoothSide);
                    heights[fixedPoint + i, j] += (average - heights[fixedPoint + i, j]) * weight;
                    heights[fixedPoint - i, j] += (average - heights[fixedPoint - i, j]) * weight;
                }

                heights[fixedPoint, j] = heights[fixedPoint - 1, j] / 2 + heights[fixedPoint + 1, j] / 2;

            }
        }

        terrain.terrainData.SetHeights(0, 0, heights);
    }

    public static void Stich(float[,] a, float[,] b, int resolution, int count)
    {
        for (var i = 0; i < resolution; i++)
        {
            for (var j = 1; j <= count; j++)
            {
                var average = a[i, resolution - j] / 2 + b[i, j] / 2;
                var weight = Mathf.SmoothStep(1f, 0f, (float) j / count);
                a[i, resolution - j] += (average - a[i, resolution - j]) * weight;
                b[i, j] += (average - b[i, j]) * weight;
            }

            b[i, 0] = a[i, resolution] = (b[i, 1] + a[i, resolution - 1]) / 2;
        }
    }
}