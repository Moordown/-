using UnityEngine;

public static class ArraySticher
{
    public static void Stich(Terrain left, Terrain right, int TerrainResolution)
    {
        var a = left.terrainData.GetHeights(0, 0, TerrainResolution + 1, TerrainResolution + 1);
        var b = right.terrainData.GetHeights(0, 0, TerrainResolution + 1, TerrainResolution + 1);
        Stich(a, b, TerrainResolution, 5);
        left.terrainData.SetHeights(0, 0, a);
        right.terrainData.SetHeights(0, 0, b);
    }
    
    public static void Stich(float[,] a, float[,] b, int resolution, int count)
    {
        for (int i = 0; i < resolution; i++)
        {
            for (int j = 0; j <= count; j++)
            {
                a[i, resolution - count + j] = Mathf.Lerp(a[i, resolution - count], b[i, count - 1], (float)j/count/2);
                b[i, j] = Mathf.Lerp(a[i, resolution - count], b[i, count-1], (float)(4 +j)/count/2);
            }
            a[i, resolution] = (a[i, resolution] + b[i, 0]) / 2;
            b[i, 0] = a[i, resolution];

            // DisplayCatmullRomSpline(
            //     new Vector3(-4, a[i, resolution - 5], 0),
            //     new Vector3(-2, a[i, resolution - 3], 0),
            //     new Vector3(2, b[i, 2], 0),
            //     new Vector3(4, b[i, 4], 0)
            // );
        }
    }

    //Display a spline between 2 points derived with the Catmull-Rom spline algorithm
    static void DisplayCatmullRomSpline(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        //The start position of the line
        Vector3 lastPos = p1;

        //The spline's resolution
        //Make sure it's is adding up to 1, so 0.3 will give a gap, but 0.2 will work
        float resolution = 0.2f;

        //How many times should we loop?
        int loops = Mathf.FloorToInt(1f / resolution);

        for (int i = 1; i <= loops; i++)
        {
            //Which t position are we at?
            float t = i * resolution;

            //Find the coordinate between the end points with a Catmull-Rom spline
            Vector3 newPos = GetCatmullRomPosition(t, p0, p1, p2, p3);


            //Save this pos so we can draw the next line segment
            lastPos = newPos;
        }
    }

    //Returns a position between 4 Vector3 with Catmull-Rom spline algorithm
    //http://www.iquilezles.org/www/articles/minispline/minispline.htm
    static Vector3 GetCatmullRomPosition(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        //The coefficients of the cubic polynomial (except the 0.5f * which I added later for performance)
        Vector3 a = 2f * p1;
        Vector3 b = p2 - p0;
        Vector3 c = 2f * p0 - 5f * p1 + 4f * p2 - p3;
        Vector3 d = -p0 + 3f * p1 - 3f * p2 + p3;

        //The cubic polynomial: a + b * t + c * t^2 + d * t^3
        Vector3 pos = 0.5f * (a + (b * t) + (c * t * t) + (d * t * t * t));

        return pos;
    }
}