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
    public float viewDistance;
    public float minDistance;
    public float maxDistance;

    [HideInInspector]
    public bool isPlayerInViewDistance;
    [HideInInspector]
    public bool isSeePlayer;
    [HideInInspector]
    public LayerMask projectileDestroyerLayer;
    
    void Start() {
        health = maxHealth;
        healthBarSlider.value = _CalculateHealthPercentage();
        isPlayerInViewDistance = false;
        projectileDestroyerLayer = LayerMask.GetMask("Ground", "Projectile Destroyer");

    }
    public void DealDamage(float damage_) {
        health -= damage_;
        _CheckDeath();
        healthBarSlider.value = _CalculateHealthPercentage();
    }

    public void HealCharacter(float heal_) {
        health += heal_;
        _CheckOverhealth();
        healthBarSlider.value = _CalculateHealthPercentage();
    }

    private void _CheckDeath() {
        if (health <= 0) {
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
