using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public Transform canon;
    public Transform projectileSpawner;
    public GameObject projectile;
    public float projectileForce;
    public float cooldown;
    private float currentCooldown;
    
    void Start() {
        currentCooldown = 0;
    }

    void Update() {
        CheckShooting();
    }



    private void CheckShooting() {
        if (Input.GetMouseButton(0) && currentCooldown <= 0) {
            GameObject bullet = Instantiate(projectile, projectileSpawner.transform.position, canon.transform.rotation);

            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 myPos = transform.position;
            Vector2 direction = (mousePos - myPos).normalized;
            bullet.GetComponent<Rigidbody2D>().velocity = direction * projectileForce;

            currentCooldown = cooldown;
        }
        else {
            currentCooldown -= Time.deltaTime;
        }
        
    }
}
