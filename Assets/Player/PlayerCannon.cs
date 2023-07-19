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

    private Transform sprite;
    public float kickbackDuration;
    public float kickbackOffset;
    private float kickbackTargetPosition;
    private bool isKickbacking;


    void Start()
    {
        isKickbacking = false;
        currentCooldown = 0;
        sprite = GetComponentInChildren<SpriteRenderer>().gameObject.transform;
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

            if (!isKickbacking) {
                StartCoroutine(Kickback());
            }
        }
    }

    private IEnumerator Kickback() {
        isKickbacking = true;
        float timer = 0f;
        kickbackTargetPosition = sprite.localPosition.x - kickbackOffset;

        while (timer < kickbackDuration / 2) {
            timer += Time.deltaTime;
            sprite.localPosition = new Vector3(Mathf.Lerp(sprite.localPosition.x, kickbackTargetPosition, timer / kickbackDuration * 2), sprite.localPosition.y, sprite.localPosition.z);
            yield return null;
        }

        sprite.localPosition = new Vector3(kickbackTargetPosition, sprite.localPosition.y, sprite.localPosition.z);


        timer = 0f;
        kickbackTargetPosition = sprite.localPosition.x + kickbackOffset;

        while (timer < kickbackDuration / 2) {
            timer += Time.deltaTime;
            sprite.localPosition = new Vector3(Mathf.Lerp(sprite.localPosition.x, kickbackTargetPosition, timer / kickbackDuration * 2), sprite.localPosition.y, sprite.localPosition.z);
            yield return null;
        }
        sprite.localPosition = new Vector3(kickbackTargetPosition, sprite.localPosition.y, sprite.localPosition.z);
        isKickbacking = false;
    }

}
