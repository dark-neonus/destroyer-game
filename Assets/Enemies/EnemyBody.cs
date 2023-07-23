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

    private UnityEngine.Vector2 _direction;
    private float _distanceToPlayer;

    void Start()
    {
        _rb = GetComponentInParent<Rigidbody2D>();
        _enemyManager = GetComponentInParent<EnemyManager>();
    }

    void Update() {
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
            _enemyManager.isSeePlayer = true;
            if (_distanceToPlayer < _enemyManager.minDistance)
            {
                _direction = (transform.position - _enemyManager.player.position).normalized;
            } 
            else if (_distanceToPlayer > _enemyManager.maxDistance)
            {
                _direction = (_enemyManager.player.position - transform.position).normalized;
            }
        }
        else {
            _enemyManager.isSeePlayer = false;
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
}