using System;
using UnityEngine;
using System.Collections;
using System.Linq; // used for Sum of array

public class AssignSplatMap : MonoBehaviour
{
    void Start()
    {
        // Get the attached terrain component
        var terrain = GetComponent<Terrain>();

        // Get a reference to the terrain data
        var terrainData = terrain.terrainData;
        // Splatmap data is stored internally as a 3d array of floats, so declare a new empty array ready for your custom splatmap data:
        var splatmapData =
            new float[terrainData.alphamapWidth, terrainData.alphamapHeight, terrainData.alphamapLayers];

        for (var y = 0; y < terrainData.alphamapHeight; y++)
        for (var x = 0; x < terrainData.alphamapWidth; x++)
        {
            // Normalise x/y coordinates to range 0-1 
            var y_01 = y / (float) terrainData.alphamapHeight;
            var x_01 = x / (float) terrainData.alphamapWidth;

            // Sample the height at this location (note GetHeight expects int coordinates corresponding to locations in the heightmap array)
            float height = terrainData.GetHeights(Mathf.RoundToInt(y_01 * terrainData.heightmapResolution),
                Mathf.RoundToInt(f: x_01 * terrainData.heightmapResolution), 1, 1)[0, 0];

            // Calculate the normal of the terrain (note this is in normalised coordinates relative to the overall terrain dimensions)
            Vector3 normal = terrainData.GetInterpolatedNormal(y_01, x_01);

            // Calculate the steepness of the terrain
            float steepness = terrainData.GetSteepness(y_01, x_01);

            // Setup an array to record the mix of texture weights at this point
            float[] splatWeights = new float[terrainData.alphamapLayers];

            // CHANGE THE RULES BELOW TO SET THE WEIGHTS OF EACH TEXTURE ON WHATEVER RULES YOU WANT

            // Texture[0] has constant influence
            splatWeights[(int) LayerId.DustId] = 0.3f;

            // Texture[1] is stronger at lower altitudes
            splatWeights[(int) LayerId.GroundId] = 1f - height;
            splatWeights[(int) LayerId.SnowId] = Mathf.Clamp01(height * height);

            // Texture[2] stronger on flatter terrain
            // Note "steepness" is unbounded, so we "normalise" it by dividing by the extent of heightmap height and scale factor
            // Subtract result from 1.0 to give greater weighting to flat surfaces
            splatWeights[(int) LayerId.GrassId] =
                1.0f - Mathf.Clamp01(steepness * steepness * 10f / terrainData.heightmapResolution);

            splatWeights[(int) LayerId.RockId] = 1.0f - splatWeights[(int) LayerId.GrassId];

            // // Texture[3] increases with height but only on surfaces facing positive Z axis 
            // splatWeights[SnowId] = Mathf.Clamp01(height * normal.z);


            // Sum of all textures weights must add to 1, so calculate normalization factor from sum of weights
            float z = splatWeights.Sum();

            // Loop through each terrain texture
            for (int i = 0; i < terrainData.alphamapLayers; i++)
            {
                // Normalize so that sum of all texture weights = 1
                splatWeights[i] /= z;

                // Assign this point to the splatmap array
                splatmapData[x, y, i] = splatWeights[i];
            }
        }

        // Finally assign the new splatmap to the terrainData:
        terrainData.SetAlphamaps(0, 0, splatmapData);
        terrain.terrainData = terrainData;
    }
}