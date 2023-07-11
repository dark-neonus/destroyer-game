using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "Biome Preset", menuName = "New Biome Preset")]
public class BiomePreset : ScriptableObject
{
    public Sprite[] tiles;
    public float minHeight;
    public float maxHeight;
    public float minMoisture;
    public float maxMoisture;
    public float minHeat;
    public float maxHeat;

    public int priority;

    public Sprite GetTleSprite() {
        return tiles[Random.Range(0, tiles.Length)];
    }

    public bool MatchCondition (float height, float moisture, float heat) {
        return IsInRange(height, minHeight, maxHeight) && IsInRange(moisture, minMoisture, maxMoisture) && IsInRange(heat, minHeat, maxHeat);
    }

    public bool IsInRange(float value, float start, float end) {
        return (value >= start) && (value <= end);
    }
}
