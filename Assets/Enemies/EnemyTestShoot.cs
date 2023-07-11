using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTestShoot : MonoBehaviour
{
    public GameObject cannon;
    public Transform projectileSpawner;
    public Transform player;
    public GameObject projectile;
    public float projectileForce;
    public float cooldown;
    private float currentCooldown;

    private float viewDistance;
    
    void Start() {
        currentCooldown = 0;
        viewDistance = cannon.GetComponent<EnemyCannon>().viewDistance;
        player = FindObjectOfType<PlayerMovement>().gameObject.transform;
    }

    void Update() {
        CheckShooting();
    }


    private void CheckShooting() {
        if (player != null) {
            if (Vector2.Distance(transform.position, player.position) <= viewDistance && currentCooldown <= 0) {
                GameObject bullet = Instantiate(projectile, projectileSpawner.transform.position, cannon.transform.rotation);

                Vector2 direction = (player.position - transform.position).normalized;
                bullet.GetComponent<Rigidbody2D>().velocity = direction * projectileForce;

                currentCooldown = cooldown * Random.Range(0.8f, 1.2f);
            }
            else {
                currentCooldown -= Time.deltaTime;
            }
        }
    }
}
