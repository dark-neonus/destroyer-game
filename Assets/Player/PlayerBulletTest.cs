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

    void OnTriggerEnter2D(Collider2D collision_)
    {
        if (collision_.tag == "Enemy Collider") {
            EnemyManager enemy = collision_.gameObject.GetComponentInParent<EnemyManager>();
            if (enemy != null) {
                enemy.DealDamage(damage);
                Destroy(gameObject);
            }
            
        }
        else if (collision_.tag == "Enemy Projectile") {
            Destroy(collision_.gameObject);
            Destroy(gameObject);
        }
    }
}
