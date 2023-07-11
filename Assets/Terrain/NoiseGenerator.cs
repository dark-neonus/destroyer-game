using System;
using UnityEngine;

[System.Serializable]
public class Wave {
    public float seed;
    public float frequency;
    public float amplitude;
    public float scale;
}

public class NoiseGenerator : MonoBehaviour
{

    public static float[,] MountainGenerate(int width, int height, Wave[] waves, float globaScale, Vector2 offset) {
        float[,] noiseMap = new float[width, height];

        float centerX = width / 2;
        float centerY = height / 2;

        for (int x = 0; x < width; ++x) {
            for (int y = 0; y < height; ++y) {
                float samplePosX = 0.0f;
                float samplePosY = 0.0f;

                float normalization = 0.0f;

                foreach (Wave wave in waves) {
                    samplePosX = (float)x * wave.scale / globaScale + offset.x;
                    samplePosY = (float)y * wave.scale / globaScale + offset.y;
                    noiseMap[x, y] += wave.amplitude * Mathf.PerlinNoise(samplePosX * wave.frequency + wave.seed, samplePosY * wave.frequency + wave.seed);
                    normalization += wave.amplitude;
                }

                noiseMap[x, y] /= normalization;
                noiseMap[x, y] = Mathf.Max(noiseMap[x, y] - 0.01f, 0);
                // Distance from center [0, 1]
                float r = NormilizedDistance(centerX, centerY, x, y);
                float b = noiseMap[x, y];
                float g = 0.5f * Mathf.Pow(4.0f * Mathf.Pow(b, b) * (r - 0.4f), 5.0f);
                
                noiseMap[x, y] = Mathf.Pow(b, b * g) + b - 1.0f;

                noiseMap[x, y] = Mathf.Max(0.0f, Mathf.Min(1.0f, noiseMap[x, y]));

                //noiseMap[x, y] = Mathf.Pow(noiseMap[x, y], 2.0f * Mathf.Pow(mountainFunc(centerX, centerY, x, y), Mathf.Pow(1.0f - mountainFunc(centerX, centerY, x, y), 2)));
            }
        }

        

        return noiseMap;
    }

    // Return value that is higher when closer to border
    public static float HeightFunct(float centerX, float centerY, float x, float y) {
        // float distance = 1.0f;
        // float xF = 1.0f - NormilizedDistance(centerX, centerY, x, y);
        // distance = 0.2f * Mathf.Tan(2.332f * xF - 1.1f) + 0.4f;
        float distance = 1.0f;
        float xF = NormilizedDistance(centerX, centerY, x, y);
        distance = -Mathf.Pow(xF - 1.1f, 2) + 1;
        return 1.0f - Mathf.Max(distance, 0);
    }

    public static float HeightCoefFunct(float centerX, float centerY, float x, float y) {
        float distance = 1.0f;
        float xF = 1.0f - NormilizedDistance(centerX, centerY, x, y);
        distance = -Mathf.Pow(xF - 1.1f, 2) + 1;
        return 1.0f - distance;
    }

    public static float MoistureFunct(float centerX, float centerY, float x, float y) {
        float distance = 1.0f;
        float xF = NormilizedDistance(centerX, centerY, x, y);
        distance = 0.2f * Mathf.Tan(2.332f * xF - 1.1f) + 0.4f;
        return distance;
    }

    public static float HeatFunct(float centerX, float centerY, float x, float y) {
        float distance = 1.0f;
        float xF = NormilizedDistance(centerX, centerY, x, y);
        distance = 0.2f * Mathf.Tan(2.332f * xF - 1.1f) + 0.4f;
        return distance;
    }

    // Return Distance to center in range [0, 1]
    public static float NormilizedDistance(float centerX, float centerY, float x, float y) {
        // float distance = Mathf.Pow((Mathf.Pow(centerX - x, 2) + Mathf.Pow(centerY - y, 2)), 0.5f);
        float distance = (Mathf.Max(Mathf.Abs(x - centerX), Mathf.Abs(y - centerY)) + Mathf.Sqrt(Mathf.Pow(x - centerX, 2) + Mathf.Pow(y - centerY, 2))) / 2.0f; 
        return Mathf.Min(distance / centerX, 1);
    }

    public static bool IsInRange(float value, float start, float end) {
        return (value >= start) && (value <= end);
    }
}
