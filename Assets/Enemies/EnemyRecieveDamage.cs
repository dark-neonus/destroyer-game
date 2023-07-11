using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyRecieveDamage : MonoBehaviour
{
    public float maxHealth;
    public float health;

    public Slider healthBarSlider;
    
    void Start() {
        health = maxHealth;
        healthBarSlider.value = CalculateHealthPercentage();

    }
    public void DealDamage(float damage) {
        health -= damage;
        CheckDeath();
        healthBarSlider.value = CalculateHealthPercentage();
    }

    public void HealCharacter(float heal) {
        health += heal;
        CheckOverhealth();
        healthBarSlider.value = CalculateHealthPercentage();
    }

    private void CheckDeath() {
        if (health <= 0) {
            Destroy(gameObject);
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
}
