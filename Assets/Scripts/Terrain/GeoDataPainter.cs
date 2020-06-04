using UnityEngine;

public class GeoDataPainter
{
    public float[,] alphaMap;
    
    public GeoDataPainter(TerrainHeightData heightData, Gradient gradient)
    {
        alphaMap = new float[heightData.number_of_columns, heightData.numder_of_rows];
        
        for(int i=0; i<heightData.number_of_columns; i++)
        for (int j = 0; j < heightData.numder_of_rows; j++)
        {
        }
    }
}
