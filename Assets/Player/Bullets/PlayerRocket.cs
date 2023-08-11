using System.Collections.Generic;
using UnityEngine;

public class PlayerRocket : MonoBehaviour
{
    public float damage;
    public float lifeTime;
    public float followSmooth;
    public float viewDistance;

    [HideInInspector]
    public Vector2 rbVelocity;
    private Rigidbody2D _rb; 

    private GameObject _target;
    private LayerMask _enemiesLayers;

    private int _counter;
    private int _counter1;
    private int _maxCounter = 6;
    private int _maxCounter1 = 80;

    void Start()
    {
        Destroy(gameObject, lifeTime);
        _rb = GetComponent<Rigidbody2D>();
        rbVelocity = _rb.velocity;
        _enemiesLayers = LayerMask.GetMask("Enemy Air","Enemy Ground", "Projectile Destroyer");
        _counter = _maxCounter;
        _counter1 = _maxCounter1;
    }

    private void Update() {
        GameManager.gameManager.ProcessCoordinates(transform);
        _Move();
        if (_counter1 <= 0) {
                _FindTarget();
                _counter1 = _maxCounter1;
            }
        _counter1 -= 1;
    }

    private void _Move() {
        if (_target == null || Vector2.Distance(_target.transform.position, transform.position) > viewDistance) {
            if (_counter <= 0) {
                _FindTarget();
                _counter = _maxCounter;
            }
            _counter -= 1;
        }
        else {
            Vector2 direction_ = _target.transform.position - transform.position;
            direction_ = Vector2.MoveTowards(transform.right, direction_, followSmooth).normalized;
            transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(direction_.y, direction_.x) * Mathf.Rad2Deg, Vector3.forward);
            rbVelocity = direction_ * rbVelocity.magnitude;
        }
        _rb.velocity = rbVelocity;
    }

    private void _FindTarget() {
        List<GameObject> nearestEnemies_ = GameManager.gameManager.GetNearestEnemyInDistance(transform.position, viewDistance);
        
        if (nearestEnemies_.Count != 0) {
            Vector3 direction_;
            RaycastHit2D hit_;
            foreach (GameObject enemy_It in nearestEnemies_) {
                direction_ = enemy_It.transform.position - transform.position;
                hit_ = Physics2D.Raycast(transform.position, direction_, viewDistance, _enemiesLayers);

                if (hit_.collider.gameObject.layer == LayerMask.NameToLayer("Enemy Ground") || hit_.collider.gameObject.layer == LayerMask.NameToLayer("Enemy Air")) {
                    _target = enemy_It;
                    // direction_ = Vector3.MoveTowards(transform.right, direction_, followSmooth).normalized;
                    // transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(direction_.y, direction_.x) * Mathf.Rad2Deg, Vector3.forward);
                    // _rb.velocity = direction_ * rbVelocity.magnitude;
                    break;
                }
            }
        }

        // if (nearestEnemy_ != null && Vector3.Distance(transform.position, nearestEnemy_.transform.position) <= viewDistance){
        //     _target = nearestEnemy_.transform.position;
        //     Vector3 direction_ = Vector3.MoveTowards(transform.right, _target - transform.position, followSmooth).normalized;
// 
        //     transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(direction_.y, direction_.x) * Mathf.Rad2Deg, Vector3.forward);
// 
        //     _rb.velocity = direction_ * rbVelocity.magnitude;
        // }
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
