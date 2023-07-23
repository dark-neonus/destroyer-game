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

    public static float[,] HeightGenerate(int width_, int height_, Wave[] waves_, float globaScale_, Vector2 offset_) {
        float[,] noiseMap_ = new float[width_, height_];

        float centerX_ = width_ / 2;
        float centerY_ = height_ / 2;

        for (int x_It = 0; x_It < width_; ++x_It) {
            for (int y_It = 0; y_It < height_; ++y_It) {
                float samplePosX_ = 0.0f;
                float samplePosY_ = 0.0f;

                float normalization_ = 0.0f;

                foreach (Wave wave_It in waves_) {
                    samplePosX_ = (float)x_It * wave_It.scale / globaScale_ + offset_.x;
                    samplePosY_ = (float)y_It * wave_It.scale / globaScale_ + offset_.y;
                    noiseMap_[x_It, y_It] += wave_It.amplitude * Mathf.PerlinNoise(samplePosX_ * wave_It.frequency + wave_It.seed, samplePosY_ * wave_It.frequency + wave_It.seed);
                    normalization_ += wave_It.amplitude;
                }

                noiseMap_[x_It, y_It] /= normalization_;
                noiseMap_[x_It, y_It] = Mathf.Max(noiseMap_[x_It, y_It] - 0.01f, 0);
                // Distance from center [0, 1]
                float r_ = NormilizedDistance(centerX_, centerY_, x_It, y_It);
                float b_ = noiseMap_[x_It, y_It];
                float g_ = 0.5f * Mathf.Pow(4.0f * Mathf.Pow(b_, b_) * (r_ - 0.4f), 5.0f);
                
                noiseMap_[x_It, y_It] = Mathf.Pow(b_, b_ * g_) + b_ - 1.0f;


                noiseMap_[x_It, y_It] = Mathf.Max(0.0f, Mathf.Min(1.0f, noiseMap_[x_It, y_It]));

                //noiseMap[x, y] = Mathf.Pow(noiseMap[x, y], 2.0f * Mathf.Pow(mountainFunc(centerX, centerY, x, y), Mathf.Pow(1.0f - mountainFunc(centerX, centerY, x, y), 2)));
            }
        }

        

        return noiseMap_;
    }

    public static float[,] MoistureGenerate(int width_, int height_, Wave[] waves_, float globaScale_, Vector2 offset_) {
        float[,] noiseMap_ = new float[width_, height_];

        float centerX_ = width_ / 2;
        float centerY_ = height_ / 2;

        for (int x_It = 0; x_It < width_; ++x_It) {
            for (int y_It = 0; y_It < height_; ++y_It) {
                float samplePosX_ = 0.0f;
                float samplePosY_ = 0.0f;

                float normalization_ = 0.0f;

                foreach (Wave wave_It in waves_) {
                    samplePosX_ = (float)x_It * wave_It.scale / globaScale_ + offset_.x;
                    samplePosY_ = (float)y_It * wave_It.scale / globaScale_ + offset_.y;
                    noiseMap_[x_It, y_It] += wave_It.amplitude * Mathf.PerlinNoise(samplePosX_ * wave_It.frequency + wave_It.seed, samplePosY_ * wave_It.frequency + wave_It.seed);
                    normalization_ += wave_It.amplitude;
                }

                noiseMap_[x_It, y_It] /= normalization_;
                // Distance from center [0, 1]
                float r_ = NormilizedDistance(centerX_, centerY_, x_It, y_It);
                
                noiseMap_[x_It, y_It] = noiseMap_[x_It, y_It] + Mathf.Pow(r_ - 0.1f, 8.0f);

                noiseMap_[x_It, y_It] = Mathf.Max(0.0f, Mathf.Min(1.0f, noiseMap_[x_It, y_It]));

            }
        }

        

        return noiseMap_;
    }

    public static float[,] HeatGenerate(int width_, int height_, Wave[] waves_, float globaScale_, Vector2 offset_) {
        float[,] noiseMap_ = new float[width_, height_];

        float centerX_ = width_ / 2;
        float centerY_ = height_ / 2;

        for (int x_It = 0; x_It < width_; ++x_It) {
            for (int y_It = 0; y_It < height_; ++y_It) {
                float samplePosX_ = 0.0f;
                float samplePosY_ = 0.0f;

                float normalization_ = 0.0f;

                foreach (Wave wave_It in waves_) {
                    samplePosX_ = (float)x_It * wave_It.scale / globaScale_ + offset_.x;
                    samplePosY_ = (float)y_It * wave_It.scale / globaScale_ + offset_.y;
                    noiseMap_[x_It, y_It] += wave_It.amplitude * Mathf.PerlinNoise(samplePosX_ * wave_It.frequency + wave_It.seed, samplePosY_ * wave_It.frequency + wave_It.seed);
                    normalization_ += wave_It.amplitude;
                }

                noiseMap_[x_It, y_It] /= normalization_;
                // Distance from center [0, 1]
                float r_ = NormilizedDistance(centerX_, centerY_, x_It, y_It);
                
                noiseMap_[x_It, y_It] = noiseMap_[x_It, y_It] * (1 - 10.0f * Mathf.Exp(-9.0f * r_ -2.25f));

                noiseMap_[x_It, y_It] = Mathf.Max(0.0f, Mathf.Min(1.0f, noiseMap_[x_It, y_It]));

            }
        }

        

        return noiseMap_;
    }


    // Return Distance to center in range [0, 1]
    public static float NormilizedDistance(float centerX_, float centerY_, float x_, float y_) {
        // float distance = Mathf.Pow((Mathf.Pow(centerX - x, 2) + Mathf.Pow(centerY - y, 2)), 0.5f);
        float distance_ = (Mathf.Max(Mathf.Abs(x_ - centerX_), Mathf.Abs(y_ - centerY_)) + Mathf.Sqrt(Mathf.Pow(x_ - centerX_, 2) + Mathf.Pow(y_ - centerY_, 2))) / 2.0f; 
        return Mathf.Min(distance_ / centerX_, 1);
    }

    public static bool IsInRange(float value_, float start_, float end_) {
        return (value_ >= start_) && (value_ <= end_);
    }
}
