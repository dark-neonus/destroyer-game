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
    public float heightHeightCoef;
    public float[,] heightMap;
    

    [Header("Moisture Map")]
    public Wave[] moistureWaves;
    public float moistureHeightCoef;
    private float[,] moistureMap;
    

    [Header("Heat Map")]
    public Wave[] heatWaves;
    public float heatHeightCoef;
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
        heightMap = NoiseGenerator.MountainGenerate(width, height, heightWaves, globalScale, offset);

        // moistureMap = NoiseGenerator.MountainGenerate(width, height, moistureWaves, offset);

        // heatMap = NoiseGenerator.MountainGenerate(width, height, heatWaves, offset);

        for (int x = 0; x < width; ++x) {
            for (int y = 0; y < height; ++y) {
                GameObject tile = Instantiate(tilePrefab, new Vector3(x, y, 0), Quaternion.identity, gameObject.transform);
                // tile.GetComponent<SpriteRenderer>().sprite = GetBiome(heightMap[x, y], moistureMap[x, y], heatMap[x, y]).GetTleSprite();
                tile.GetComponent<SpriteRenderer>().sprite = tileSprite;
                tile.GetComponent<SpriteRenderer>().color = new Color(heightMap[x, y], heightMap[x, y], heightMap[x, y]);
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