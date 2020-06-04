using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class GeoDataLoader
{
    private TerrainHeightData terrain_data;

    public float[,] heightMap => terrain_data.height_map;

    public TerrainHeightData terrainHeightData => terrain_data;

    public bool load(string path, int resolution)
    {
        try
        {
            string[] header_data = File.ReadAllLines(path);

            Dictionary<string, string> header_params = new Dictionary<string, string>();

            for (int i = 0; i < header_data.Length; i++)
            {
                string[] param = header_data[i].Split('=');
                header_params.Add(EraseSpaces(param[0]), EraseSpaces(param[1]));
            }

            if (!ParseHeader(header_params, ref terrain_data))
                return false;

            terrain_data.normalize_data = new float[terrain_data.numder_of_rows, terrain_data.number_of_columns];

            string dir = Path.GetDirectoryName(path);
            string data_path = Path.Combine(dir, terrain_data.file_title + ".bin");

            BinaryReader reader = new BinaryReader(File.Open(data_path, FileMode.Open));

            for (int i = 0; i < terrain_data.numder_of_rows; i++)
            for (int j = 0; j < terrain_data.number_of_columns; j++)
            {
                float height = reader.ReadSingle();
                float level = (height - terrain_data.elev_m_minimum) / terrain_data.y_range;

                if (level > 1.0f)
                    level = 1.0f;

                if (level < 0.0f)
                    level = 0.0f;

                terrain_data.normalize_data[i, j] = level;
            }

            terrain_data.height_map = new float[resolution, resolution];

            float dx = terrain_data.x_range / resolution - 1;
            float dz = terrain_data.z_range / resolution - 1;

            for (int i = 0; i < resolution; i++)
            for (int j = 0; j < resolution; j++)
            {
                terrain_data.height_map[i, j] = getHeight(i * dx, j * dz);
            }

            return true;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            return false;
        }
    }

    private bool ParseHeader(Dictionary<string, string> header_data,
        ref TerrainHeightData data)
    {
        try
        {
            data.file_title = header_data["file_title"];
        }
        catch (Exception)
        {
            return false;
        }

        try
        {
            data.data_format = header_data["data_format"];
        }
        catch (Exception)
        {
        }

        try
        {
            data.map_projection = header_data["map_projection"];
        }
        catch (Exception)
        {
        }

        try
        {
            data.ellipsoid = header_data["ellipsoid"];
        }
        catch (Exception)
        {
        }

        try
        {
            data.left_map_x = float.Parse(header_data["left_map_x"], CultureInfo.InvariantCulture);
            data.lower_map_y = float.Parse(header_data["lower_map_y"], CultureInfo.InvariantCulture);
            data.right_map_x = float.Parse(header_data["right_map_x"], CultureInfo.InvariantCulture);
            data.upper_map_y = float.Parse(header_data["upper_map_y"], CultureInfo.InvariantCulture);
            data.numder_of_rows = int.Parse(header_data["number_of_rows"]);
            data.number_of_columns = int.Parse(header_data["number_of_columns"]);
            data.elev_m_unit = header_data["elev_m_unit"];
            data.elev_m_minimum = float.Parse(header_data["elev_m_minimum"], CultureInfo.InvariantCulture);
            data.elev_m_maximum = float.Parse(header_data["elev_m_maximum"], CultureInfo.InvariantCulture);
            data.elev_m_missing_flag = int.Parse(header_data["elev_m_missing_flag"]);
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }

    private string EraseSpaces(string str)
    {
        string tmp = "";

        for (int i = 0; i < str.Length; i++)
        {
            if (str[i] != ' ')
                tmp += str[i];
        }

        return tmp;
    }

    float getHeight(float x, float z)
    {
        float height = 0.0f;

        float dx = terrain_data.x_range / (terrain_data.numder_of_rows - 1);
        float dz = terrain_data.z_range / (terrain_data.number_of_columns - 1);

        int i = (int) (x / dx);
        int j = (int) (z / dz);

        if ((i >= terrain_data.numder_of_rows) || (j >= terrain_data.number_of_columns))
            return 0.0f;

        float dydx = (terrain_data.normalize_data[i + 1, j] - terrain_data.normalize_data[i, j]) / dx;
        float dydz = (terrain_data.normalize_data[i, j + 1] - terrain_data.normalize_data[i, j]) / dz;

        height = terrain_data.normalize_data[i, j] + dydx * (x - i * dx) + dydz * (z - j * dz);

        return height;
    }
}