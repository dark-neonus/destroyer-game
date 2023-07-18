using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletTest : MonoBehaviour
{
    public float damage;
    public float lifeTime;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") {
            PlayerStats.playerStats.DealDamage(damage);
            Destroy(gameObject);
        }
    }
}
