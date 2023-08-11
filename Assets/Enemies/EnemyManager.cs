using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    public float maxHealth;
    [HideInInspector]
    public float health;

    public Slider healthBarSlider;

    [HideInInspector]
    public Transform player;
    public float originalViewDistance;
    [HideInInspector]
    public float viewDistance;
    public float minDistance;
    public float maxDistance;

    [HideInInspector]
    public bool isPlayerInViewDistance;
    [HideInInspector]
    public bool isAngry;
    [HideInInspector]
    public LayerMask projectileDestroyerLayer;
    
    void Start() {
        health = maxHealth;
        healthBarSlider.value = _CalculateHealthPercentage();
        isPlayerInViewDistance = false;
        projectileDestroyerLayer = LayerMask.GetMask("Player Air","Player Ground", "Projectile Destroyer");
        viewDistance = originalViewDistance;
        isAngry = false;
        GameManager.gameManager.enemies.Add(gameObject);
    }

    private void Update() {
        GameManager.gameManager.ProcessCoordinates(transform);
    }

    public void DealDamage(float damage_) {
        health -= damage_;
        _CheckDeath();
        healthBarSlider.value = _CalculateHealthPercentage();
        isAngry = true;
    }

    public void HealCharacter(float heal_) {
        health += heal_;
        _CheckOverhealth();
        healthBarSlider.value = _CalculateHealthPercentage();
    }

    private void _CheckDeath() {
        if (health <= 0) {
            GameManager.gameManager.enemies.Remove(gameObject);
            Destroy(gameObject);
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
}
