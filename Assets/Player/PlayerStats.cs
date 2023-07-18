using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats playerStats;

    public GameObject playerReference;
    private GameObject player;
    private GameObject playerCannonBase;

    public float health;
    public float maxHealth;

    public Slider healthBarSlider;
    public TextMeshProUGUI healthText;

    public List<GameObject> playerCannonBasePrefabs;
    public int playerLevel;

    public List<GameObject> playerCannonPrefabs;
    public List<int> selectedCannonIndices;


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
        player = GameObject.FindWithTag("Player");
        if (player != null) {
            Destroy(player);
            foreach (Transform child in transform) {
	            GameObject.Destroy(child.gameObject);
            }
        }
        UpdateCannonBaseInfo();
        PlayerInit();

        
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

    void Update()
    {
        CheckRespawn();
    }

    private void CheckRespawn() {
        if (Input.GetKey(KeyCode.Space) && player == null) {
            PlayerInit();
        }
    }
    public void PlayerInit() {
        health = maxHealth;
        // healthBarSlider.value = CalculateHealthPercentage();
        // UpdateHealthText();
        player = Instantiate(playerReference);

        InitCannonBase();

        UpdatePlayerReferences();
    }

    private void InitCannonBase() {
        playerCannonBase = Instantiate(playerCannonBasePrefabs[playerLevel], player.transform);

        player.GetComponent<PlayerShoot>().cannonBase = playerCannonBase.GetComponent<CannonBase>();

        List<GameObject> cannonPlatforms = playerCannonBase.GetComponent<CannonBase>().cannonPlatforms;

        for (int i = 0; i < Mathf.Min(cannonPlatforms.Count, selectedCannonIndices.Count); i++) {
            if (playerCannonPrefabs[selectedCannonIndices[i]] != null) {
                Instantiate(playerCannonPrefabs[selectedCannonIndices[i]], cannonPlatforms[i].transform);
            }
        } 
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

    public void UpdateCannonBaseInfo() {

    }
}
