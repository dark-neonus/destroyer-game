using UnityEngine;

public class EnemyBulletTest : MonoBehaviour
{
    public float damage;
    public float lifeTime;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update() {
        GameManager.gameManager.ProcessCoordinates(transform);
    }


    private void OnTriggerEnter2D(Collider2D collider_)
    {
        if (collider_.tag == "Player Trigger" || collider_.tag == "Player Collider") {
            GameManager.gameManager.DealDamage(damage);
            DestroyProjectile();
        }
        else if (collider_.gameObject.layer == LayerMask.NameToLayer("Projectile Destroyer")) {
            DestroyProjectile();
        }
    }

    private void OnTriggerStay2D(Collider2D collider_) {
        OnTriggerEnter2D(collider_);
    }

    private void OnTriggerExit2D(Collider2D collider_) {
        OnTriggerEnter2D(collider_);
    }

    public void DestroyProjectile() {
        Destroy(gameObject);
    }
}
