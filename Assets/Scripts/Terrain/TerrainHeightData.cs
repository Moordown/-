using System;

public struct TerrainHeightData
{
    public string file_title;
    public string data_format;
    public string map_projection;
    public string ellipsoid;
    public float left_map_x;
    public float lower_map_y;
    public float right_map_x;
    public float upper_map_y;
    public int numder_of_rows;
    public int number_of_columns;
    public string elev_m_unit;
    public float elev_m_minimum;
    public float elev_m_maximum;
    public int elev_m_missing_flag;

    public float[,] normalize_data;
    public float[,] height_map;

    private const float EarthRadius = 6378.0e3f;
    private const float m_per_deg = (float) Math.PI * EarthRadius / 180.0f;

    public float x_range => (right_map_x - left_map_x) * m_per_deg;
    public float y_range => elev_m_maximum - elev_m_minimum;
    public float z_range => (upper_map_y - lower_map_y) * m_per_deg;
}