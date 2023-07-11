using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public float lifeTime;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy Collider") {
            EnemyRecieveDamage enemy = collision.gameObject.GetComponentInParent<EnemyRecieveDamage>();
            if (enemy != null) {
                enemy.DealDamage(damage);
                Destroy(gameObject);
            }
            
        }
        else if (collision.tag == "Enemy Projectile") {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
