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
    private int _width;
    private int _height;
    public float globalScale;
    public Vector2 offset;

    [Header("Height Map")]
    public Wave[] heightWaves;
    private float[,] _heightMap;
    

    [Header("Moisture Map")]
    public Wave[] moistureWaves;
    private float[,] _moistureMap;
    

    [Header("Heat Map")]
    public Wave[] heatWaves;
    private float[,] _heatMap;


    public Sprite tileSprite;
    

    void Start ()
    {
        foreach (Wave wave_It in heightWaves) {
            wave_It.seed = Random.Range(0.0f, 1000.0f);
        }
        foreach (Wave wave_It in moistureWaves) {
            wave_It.seed = Random.Range(0.0f, 1000.0f);
        }
        foreach (Wave wave_It in heatWaves) {
            wave_It.seed = Random.Range(0.0f, 1000.0f);
        }
        GenerateMap();
    }

    public void GenerateMap() {
        _width = (int)(originalWidth * globalScale);
        _height = (int)(originalHeight * globalScale);
        _heightMap = NoiseGenerator.HeightGenerate(_width, _height, heightWaves, globalScale, offset);

        _moistureMap = NoiseGenerator.MoistureGenerate(_width, _height, moistureWaves, globalScale, offset);

        _heatMap = NoiseGenerator.HeatGenerate(_width, _height, heatWaves, globalScale, offset);

        BiomePreset tmpBiome_;

        for (int x_It = 0; x_It < _width; ++x_It) {
            for (int y_It = 0; y_It < _height; ++y_It) {
                GameObject tile_ = Instantiate(tilePrefab, new Vector3(x_It - _width / 2, y_It - _height / 2, 0), Quaternion.identity, gameObject.transform);
                tmpBiome_ = _GetBiome(_heightMap[x_It, y_It], _moistureMap[x_It, y_It], _heatMap[x_It, y_It]);
                if (tmpBiome_.biomeName == "Ocean") {
                    _moistureMap[x_It, y_It] = 1.0f;
                }
                tile_.GetComponent<SpriteRenderer>().sprite = tmpBiome_.GetTleSprite();
                tile_.GetComponent<TileInfo>().InitTile(TileInfo.biomeIDs[tmpBiome_.biomeName], _heightMap[x_It, y_It], _moistureMap[x_It, y_It], _heatMap[x_It, y_It]);
                // tile.transform.localScale = new Vector3(1.01f, 1.01f, 0.0f);
            }
        } 
    }

    void Update() {
        if (Input.GetKey(KeyCode.Space)) {
            foreach (Transform child_It in transform) {
	            GameObject.Destroy(child_It.gameObject);
            }
            GenerateMap();
        }
        if (Input.GetKey(KeyCode.Z)) {
            foreach (Transform child_It in transform) {
	            GameObject.Destroy(child_It.gameObject);
            }
            foreach (Wave wave_It in heightWaves) {
                wave_It.seed = Random.Range(0.0f, 1000.0f);
            }
            foreach (Wave wave_It in moistureWaves) {
                wave_It.seed = Random.Range(0.0f, 1000.0f);
            }
            foreach (Wave wave_It in heatWaves) {
                wave_It.seed = Random.Range(0.0f, 1000.0f);
            }
            GenerateMap();
        }
    }

    private BiomePreset _GetBiome (float height_, float moisture_, float heat_) {
        List<BiomeTempData> biomeTemp_ = new List<BiomeTempData>();

        foreach(BiomePreset biome_It in biomes) {
            if(biome_It.MatchCondition(height_, moisture_, heat_)) {
                biomeTemp_.Add(new BiomeTempData(biome_It));                
            }
        }

        BiomePreset biomeToReturn_ = null;

        biomeTemp_ = biomeTemp_.OrderByDescending(x => x.biome.priority).ToList();

        if (biomeTemp_.Count != 0) {
            biomeToReturn_ = biomeTemp_[0].biome;
        }
        else {
            biomeToReturn_ = biomes[0];
        }

        return biomeToReturn_;
    }
}

public class BiomeTempData {
    public BiomePreset biome;
    public BiomeTempData (BiomePreset preset_) {
        biome = preset_;
    }
        
    public float GetDiffValue (float height_, float moisture_, float heat_) {
        return (height_ - biome.minHeight) + (moisture_ - biome.minMoisture) + (heat_ - biome.minHeat);
    }
}
