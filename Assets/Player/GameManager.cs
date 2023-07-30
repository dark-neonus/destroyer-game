using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;

    public List<GameObject> playerReferences;
    public int playerLevel;

    private int _tmpLevel;
    private GameObject _player;
    private Transform _playerTransform;

    public List<GameObject> playerCannonPrefabs;
    public List<int> selectedCannonIndices;

    public float health;
    public float maxHealth;

    public Slider healthBarSlider;
    public TextMeshProUGUI healthText;

    [HideInInspector]
    public List<GameObject> enemies = new List<GameObject>();

    void Awake()
    {
        if (gameManager != null) {
            Destroy(gameManager);
        }
        else {
            gameManager = this;
        }
        DontDestroyOnLoad(this);
    }

    void Start() {
        health = maxHealth;
        // healthBarSlider.value = CalculateHealthPercentage();
        // UpdateHealthText();
        _tmpLevel = playerLevel;
        _playerTransform = new GameObject("Player Spawn Transform").transform;
        _playerTransform.SetPositionAndRotation(UnityEngine.Vector3.zero, UnityEngine.Quaternion.identity);
        _playerTransform.localScale = UnityEngine.Vector3.one;
        SpawnPlayer();
        
    }

    void Update() {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            playerLevel = 0;
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            playerLevel = 1;
        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            playerLevel = 2;
        }
        if (playerLevel != _tmpLevel) {
            SpawnPlayer();
        }
        _tmpLevel = playerLevel;
    }

    public void DealDamage(float damage_) {
        health -= damage_;
        _CheckDeath();
        // healthBarSlider.value = _CalculateHealthPercentage();
        // _UpdateHealthText();
    }

    public void HealCharacter(float heal_) {
        health += heal_;
        _CheckOverhealth();
        healthBarSlider.value = _CalculateHealthPercentage();
        _UpdateHealthText();
    }

    private void _CheckDeath() {
        if (health <= 0) {
            Destroy(_player);
        }
    }

    private void _CheckOverhealth() {
        if (health > maxHealth) {
            health = maxHealth;
        }
    }

    private float _CalculateHealthPercentage() {
        return (health / maxHealth);
    }

    private void _UpdateHealthText() {
        healthText.text = Mathf.Ceil(health).ToString() + "/" + Mathf.Ceil(maxHealth).ToString(); 
    }



    public void PlayerInit() {
        health = maxHealth;
        // healthBarSlider.value = CalculateHealthPercentage();
        // UpdateHealthText();
        _player = Instantiate(playerReferences[playerLevel], _playerTransform.position, _playerTransform.rotation);
        _player.transform.localScale = _playerTransform.localScale;

        _SetCannons();
        _UpdatePlayerReferences();
    }

    private void _UpdatePlayerReferences() {
        CameraMechanics cameraFollowPlayer_ = GameObject.FindWithTag("MainCamera").GetComponent<CameraMechanics>();
        if (cameraFollowPlayer_ != null) {
            cameraFollowPlayer_.player = _player.transform;
        }
        EnemyManager[] enemyManagerScripts_ = FindObjectsOfType<EnemyManager>();
        foreach (EnemyManager enemyManagerScript_It in enemyManagerScripts_) {
            enemyManagerScript_It.player = _player.transform;
        }
        
    }


    public void SpawnPlayer() {
        _player = GameObject.FindWithTag("Player");
        if (_player != null) {
            _playerTransform.SetPositionAndRotation(_player.transform.position, _player.transform.rotation);
            _playerTransform.localScale = _player.transform.localScale;

            Destroy(_player);
            foreach (Transform child_It in _player.transform) {
	            GameObject.Destroy(child_It.gameObject);
            }
        }
        PlayerInit();
    }

    private void _SetCannons() {
        List<GameObject> cannonPlatforms_ = _player.GetComponentInChildren<CannonBase>().gameObject.GetComponent<CannonBase>().cannonPlatforms;

        for (int i = 0; i < Mathf.Min(cannonPlatforms_.Count, selectedCannonIndices.Count); i++) {
            if (playerCannonPrefabs[selectedCannonIndices[i]] != null) {
                Instantiate(playerCannonPrefabs[selectedCannonIndices[i]], cannonPlatforms_[i].transform);
            }
        } 
    }

    public GameObject GetNearestEnemy(Vector3 origin_) {
        if (enemies.Count != 0) {
            enemies.Sort((a, b) => Vector2.Distance(origin_, a.transform.position).CompareTo(Vector2.Distance(origin_, b.transform.position)));
            return enemies[0];
        }
        else {
            return null;
        }
    }

    public List<GameObject> GetNearestEnemyInDistance(Vector3 origin_, float allowDistance_) {
        List<GameObject> result_ = new List<GameObject>();
        if (enemies.Count != 0) {
            result_ = enemies.FindAll(obj => Vector3.Distance(obj.transform.position, origin_) < allowDistance_);
            result_.Sort((a, b) => Vector2.Distance(origin_, a.transform.position).CompareTo(Vector2.Distance(origin_, b.transform.position)));
        }
        return result_;
    }
}
