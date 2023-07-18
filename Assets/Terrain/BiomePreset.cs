using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "Biome Preset", menuName = "New Biome Preset")]
public class BiomePreset : ScriptableObject
{
    public Sprite[] tiles; 
    public string biomeName;

    public bool heightMatter;
    public float minHeight;
    public float maxHeight;

    public bool moistureMatter;
    public float minMoisture;
    public float maxMoisture;

    public bool heatMatter;
    public float minHeat;
    public float maxHeat;

    public int priority;

    public Sprite GetTleSprite() {
        return tiles[Random.Range(0, tiles.Length)];
    }

    public bool MatchCondition (float height, float moisture, float heat) {
        return (IsInRange(height, minHeight, maxHeight) || !heightMatter) && (IsInRange(moisture, minMoisture, maxMoisture) || !moistureMatter) && (IsInRange(heat, minHeat, maxHeat) || !heatMatter);
    }

    public bool IsInRange(float value, float start, float end) {
        return (value >= start) && (value <= end);
    }
}
