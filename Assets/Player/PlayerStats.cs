using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats playerStats;

    public GameObject playerReference;
    private GameObject player;

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
        healthBarSlider.value = CalculateHealthPercentage();
        UpdateHealthText();
        player = FindObjectOfType<PlayerMovement>().gameObject;
        if (player != null) {
            Destroy(player);
        }
        Respawn();
        
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
            Respawn();
        }
    }
    public void Respawn() {
        health = maxHealth;
        healthBarSlider.value = CalculateHealthPercentage();
        UpdateHealthText();
        player = Instantiate(playerReference);
        CameraMechanics cameraFollowPlayer = Camera.main.GetComponent<CameraMechanics>();
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
}
