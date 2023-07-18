using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public BiomePreset[] biomes;
    public GameObject tilePrefab;

    [Header("Dimensions")]
    public int originalWidth;
    public int originalHeight;
    private int width;
    private int height;
    public float globalScale;
    public Vector2 offset;

    [Header("Height Map")]
    public Wave[] heightWaves;
    public float[,] heightMap;
    

    [Header("Moisture Map")]
    public Wave[] moistureWaves;
    private float[,] moistureMap;
    

    [Header("Heat Map")]
    public Wave[] heatWaves;
    private float[,] heatMap;


    public Sprite tileSprite;
    

    void Start ()
    {
        foreach (Wave wave in heightWaves) {
            wave.seed = Random.Range(0.0f, 1000.0f);
        }
        foreach (Wave wave in moistureWaves) {
            wave.seed = Random.Range(0.0f, 1000.0f);
        }
        foreach (Wave wave in heatWaves) {
            wave.seed = Random.Range(0.0f, 1000.0f);
        }
        GenerateMap();
    }

    void GenerateMap() {
        width = (int)(originalWidth * globalScale);
        height = (int)(originalHeight * globalScale);
        heightMap = NoiseGenerator.HeightGenerate(width, height, heightWaves, globalScale, offset);

        moistureMap = NoiseGenerator.MoistureGenerate(width, height, moistureWaves, globalScale, offset);

        heatMap = NoiseGenerator.HeatGenerate(width, height, heatWaves, globalScale, offset);

        BiomePreset tmpBiome;

        for (int x = 0; x < width; ++x) {
            for (int y = 0; y < height; ++y) {
                GameObject tile = Instantiate(tilePrefab, new Vector3(x - width / 2, y - height / 2, 0), Quaternion.identity, gameObject.transform);
                tmpBiome = GetBiome(heightMap[x, y], moistureMap[x, y], heatMap[x, y]);
                if (tmpBiome.biomeName == "Ocean") {
                    moistureMap[x, y] = 1.0f;
                }
                tile.GetComponent<SpriteRenderer>().sprite = tmpBiome.GetTleSprite();
                tile.GetComponent<TileInfo>().InitTile(TileInfo.biomeIDs[tmpBiome.biomeName], heightMap[x, y], moistureMap[x, y], heatMap[x, y]);
                // tile.transform.localScale = new Vector3(1.01f, 1.01f, 0.0f);
            }
        } 
    }

    void Update() {
        if (Input.GetKey(KeyCode.Space)) {
            foreach (Transform child in transform) {
	            GameObject.Destroy(child.gameObject);
            }
            GenerateMap();
        }
        if (Input.GetKey(KeyCode.Z)) {
            foreach (Transform child in transform) {
	            GameObject.Destroy(child.gameObject);
            }
            foreach (Wave wave in heightWaves) {
                wave.seed = Random.Range(0.0f, 1000.0f);
            }
            foreach (Wave wave in moistureWaves) {
                wave.seed = Random.Range(0.0f, 1000.0f);
            }
            foreach (Wave wave in heatWaves) {
                wave.seed = Random.Range(0.0f, 1000.0f);
            }
            GenerateMap();
        }
    }

    BiomePreset GetBiome (float height, float moisture, float heat) {
        List<BiomeTempData> biomeTemp = new List<BiomeTempData>();

        foreach(BiomePreset biome in biomes) {
            if(biome.MatchCondition(height, moisture, heat)) {
                biomeTemp.Add(new BiomeTempData(biome));                
            }
        }

        BiomePreset biomeToReturn = null;

        biomeTemp = biomeTemp.OrderByDescending(x => x.biome.priority).ToList();

        if (biomeTemp.Count != 0) {
            biomeToReturn = biomeTemp[0].biome;
        }
        else {
            biomeToReturn = biomes[0];
        }

        return biomeToReturn;
    }
}

public class BiomeTempData {
    public BiomePreset biome;
    public BiomeTempData (BiomePreset preset) {
        biome = preset;
    }
        
    public float GetDiffValue (float height, float moisture, float heat) {
        return (height - biome.minHeight) + (moisture - biome.minMoisture) + (heat - biome.minHeat);
    }
}
