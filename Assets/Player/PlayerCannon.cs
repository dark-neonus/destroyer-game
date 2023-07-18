using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCannon : MonoBehaviour
{
    public Transform projectileSpawner;
    public GameObject projectile;
    public float projectileForce;
    public float cooldown;
    private float currentCooldown;


    void Start()
    {
        currentCooldown = 0;
    }

    void Update()
    {
        RotateToMouse();
        currentCooldown = Mathf.Max(0, currentCooldown - Time.deltaTime);
    }

    void RotateToMouse() {
        Vector2 mousePosition = Input.mousePosition;

        Vector2 objectPosition = Camera.main.WorldToScreenPoint(transform.position);

        Vector2 direction = mousePosition - objectPosition;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    public void Shoot() {
        if (currentCooldown == 0) {
            GameObject bullet = Instantiate(projectile, projectileSpawner.transform.position, transform.rotation);

            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 myPos = transform.position;
            Vector2 direction = (mousePos - myPos).normalized;
            bullet.GetComponent<Rigidbody2D>().velocity = direction * projectileForce;

            currentCooldown = cooldown;
        }
    }

}
