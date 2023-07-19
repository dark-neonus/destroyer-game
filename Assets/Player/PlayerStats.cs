using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats playerStats;

    public List<GameObject> playerReferences;
    public int playerLevel;

    private int tmpLevel;
    private GameObject player;

    public List<GameObject> playerCannonPrefabs;
    public List<int> selectedCannonIndices;

    public float health;
    public float maxHealth;

    public Slider healthBarSlider;
    public TextMeshProUGUI healthText;


    void Awake()
    {
        if (playerStats != null) {
            Destroy(playerStats);
        }
        else {
            playerStats = this;
        }
        DontDestroyOnLoad(this);
    }

    void Start() {
        health = maxHealth;
        // healthBarSlider.value = CalculateHealthPercentage();
        // UpdateHealthText();
        tmpLevel = playerLevel;
        SpawnPlayer();
        
    }

    void Update()
    {
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
        if (playerLevel != tmpLevel) {
            SpawnPlayer();
        }
        tmpLevel = playerLevel;
    }

    public void DealDamage(float damage) {
        health -= damage;
        CheckDeath();
        healthBarSlider.value = CalculateHealthPercentage();
        UpdateHealthText();
    }

    public void HealCharacter(float heal) {
        health += heal;
        CheckOverhealth();
        healthBarSlider.value = CalculateHealthPercentage();
        UpdateHealthText();
    }

    private void CheckDeath() {
        if (health <= 0) {
            Destroy(player);
        }
    }

    private void CheckOverhealth() {
        if (health > maxHealth) {
            health = maxHealth;
        }
    }

    private float CalculateHealthPercentage() {
        return (health / maxHealth);
    }

    private void UpdateHealthText() {
        healthText.text = Mathf.Ceil(health).ToString() + "/" + Mathf.Ceil(maxHealth).ToString(); 
    }



    public void PlayerInit() {
        health = maxHealth;
        // healthBarSlider.value = CalculateHealthPercentage();
        // UpdateHealthText();
        player = Instantiate(playerReferences[playerLevel]);
        SetCannons();
        UpdatePlayerReferences();
    }

    private void UpdatePlayerReferences() {
        CameraMechanics cameraFollowPlayer = GameObject.FindWithTag("MainCamera").GetComponent<CameraMechanics>();
        if (cameraFollowPlayer != null) {
            cameraFollowPlayer.player = player.transform;
        }
        EnemyCannon[] enemyCannons = FindObjectsOfType<EnemyCannon>();
        foreach (EnemyCannon cannon in enemyCannons) {
            cannon.player = player.transform;
        }
        EnemyTestShoot[] shotingScripts = FindObjectsOfType<EnemyTestShoot>();
        foreach (EnemyTestShoot shotingScript in shotingScripts) {
            shotingScript.player = player.transform;
        }
        EnemyMovement[] enemyMovementScripts = FindObjectsOfType<EnemyMovement>();
        foreach (EnemyMovement enemyMovementScript in enemyMovementScripts) {
            enemyMovementScript.player = player.transform;
        }
    }

    public void SpawnPlayer() {
        player = GameObject.FindWithTag("Player");
        if (player != null) {
            Destroy(player);
            foreach (Transform child in transform) {
	            GameObject.Destroy(child.gameObject);
            }
        }
        PlayerInit();
    }

    private void SetCannons() {
        List<GameObject> cannonPlatforms = player.GetComponentInChildren<CannonBase>().gameObject.GetComponent<CannonBase>().cannonPlatforms;

        for (int i = 0; i < Mathf.Min(cannonPlatforms.Count, selectedCannonIndices.Count); i++) {
            if (playerCannonPrefabs[selectedCannonIndices[i]] != null) {
                Instantiate(playerCannonPrefabs[selectedCannonIndices[i]], cannonPlatforms[i].transform);
            }
        } 
    }
}
