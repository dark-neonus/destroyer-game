using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public float lifeTime;


    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D collider_)
    {
        if (collider_.tag == "Enemy Collider") {
            EnemyManager enemy = collider_.gameObject.GetComponentInParent<EnemyManager>();
            if (enemy != null) {
                enemy.DealDamage(damage);
                DestroyProjectile();
            }
            
        }
        else if (collider_.tag == "Enemy Projectile") {
            collider_.GetComponent<EnemyBulletTest>().DestroyProjectile();
            DestroyProjectile();
        }
        else if (collider_.gameObject.layer == LayerMask.NameToLayer("Projectile Destroyer")) {
            DestroyProjectile();
        }
    }

    private void OnTriggerStay2D(Collider2D collider_) {
        OnTriggerEnter2D(collider_);
    }

    public void DestroyProjectile() {
        Destroy(gameObject);
    }
}
