using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBody : MonoBehaviour
{
    [HideInInspector]
    private EnemyManager _enemyManager;

    public float moveSpeed;
    public float rotationSpeed;

    private Rigidbody2D _rb;

    private Vector2 _direction;
    private float _distanceToPlayer;

    void Start()
    {
        _rb = GetComponentInParent<Rigidbody2D>();
        _enemyManager = GetComponentInParent<EnemyManager>();
    }

    void Update() {
        CheckPlayerView();
        _Rotate();
    }
    

    void FixedUpdate()
    {
        _Move();
        _ShiftLegs();
    }

    private void _Move() {
        _direction = UnityEngine.Vector2.zero;
        _distanceToPlayer = Vector3.Distance(transform.position, _enemyManager.player.position);

        if (_distanceToPlayer <= _enemyManager.viewDistance)
        {
            _enemyManager.isPlayerInViewDistance = true;

            if (_enemyManager.isSeePlayer) {
                if (_distanceToPlayer < _enemyManager.minDistance)
                {
                    _direction = (transform.position - _enemyManager.player.position).normalized;
                } 
                else if (_distanceToPlayer > _enemyManager.maxDistance)
                {
                    _direction = (_enemyManager.player.position - transform.position).normalized;
                }
            }
        }
        else {
            _enemyManager.isPlayerInViewDistance = false;
        }
        _rb.velocity = _direction * moveSpeed; 
        
    }

    private void _Rotate() {
        if (_direction != UnityEngine.Vector2.zero) {
            float targetAngle_ = UnityEngine.Vector2.SignedAngle(UnityEngine.Vector2.right, _direction);

            float angle_ = Mathf.MoveTowardsAngle(transform.eulerAngles.z, targetAngle_, rotationSpeed * Time.deltaTime );
            transform.rotation = UnityEngine.Quaternion.Euler(0f, 0f, angle_);
        }
    }

    private void _ShiftLegs() {
        LegScript[] legScripts_ = GetComponentsInChildren<LegScript>();
        foreach (LegScript legScript_It in legScripts_) {
            legScript_It.movingOffsetDirection = _direction;
        }
        
    }

    private void CheckPlayerView() {
        if (_enemyManager.isPlayerInViewDistance) {
            _enemyManager.isSeePlayer = false;
            Vector2 direction_ = _enemyManager.player.position - transform.position;

            RaycastHit2D[] hits_ = Physics2D.RaycastAll(transform.position, direction_, Mathf.Infinity, _enemyManager.projectileDestroyerLayer);

            System.Array.Sort(hits_, (x, y) => x.distance.CompareTo(y.distance));

            foreach (var hit_It in hits_)
            {
                if (hit_It.collider.gameObject.layer == LayerMask.NameToLayer("Projectile Destroyer")) {
                    Debug.DrawRay(transform.position, hit_It.point - (Vector2)transform.position, Color.red);
                    Debug.Log("Mountain");
                    break;
                }
                else if (hit_It.collider.tag == "Player Trigger") {
                    Debug.DrawRay(transform.position, direction_, Color.red);
                    _enemyManager.isSeePlayer = true;
                    Debug.Log("Player");
                    break;
                }
            }
        }
    }
}
