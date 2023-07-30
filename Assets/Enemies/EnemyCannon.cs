using System.Collections.Generic;
using UnityEngine;

public class EnemyCannon : MonoBehaviour
{
    [HideInInspector]
    private EnemyManager _enemyManager;
    public float rotationSmooth;
    private float _angleToPlayer;

    public Transform projectileSpawner;
    public GameObject projectile;
    public float projectileForce;
    public float cooldown;
    private float _currentCooldown;
    private bool _isCanShootPlayer;

    void Start() {
        _currentCooldown = 0;
        _enemyManager = GetComponentInParent<EnemyManager>();
    }

    void Update()
    {
        _InteractWithPlayer();
        _currentCooldown = Mathf.Max(0, _currentCooldown - Time.deltaTime);
    }

    private void FixedUpdate() {
        _CheckPlayerView();
    }

    private void _InteractWithPlayer()
    {
        if (_enemyManager.isPlayerInViewDistance) {
            _Rotate();
        }
        if (_isCanShootPlayer)
        {
            _Shoot();
        }
    }

    private void _Rotate() {
        Vector2 direction_ = _enemyManager.player.position - transform.position;
        _angleToPlayer = Mathf.Atan2(direction_.y, direction_.x) * Mathf.Rad2Deg;
        Quaternion targetRotation_ = Quaternion.Euler(0f, 0f, _angleToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation_, rotationSmooth * Time.deltaTime);
    }

    public void _Shoot() {
        if (_currentCooldown == 0) {
            GameObject bullet = Instantiate(projectile, projectileSpawner.transform.position, transform.rotation);

            Vector2 direction_ = (_enemyManager.player.transform.position - transform.position).normalized;
            bullet.GetComponent<Rigidbody2D>().velocity = direction_ * projectileForce;

            _currentCooldown = cooldown;

            // if (!_isKickbacking) {
            //     StartCoroutine(Kickback());
            // }
        }
    }

    private void _CheckPlayerView() {
        _isCanShootPlayer = false;
        if (_enemyManager.isPlayerInViewDistance) {
            Vector2 direction_ = _enemyManager.player.position - projectileSpawner.position;

            RaycastHit2D hit_ = Physics2D.Raycast(projectileSpawner.position, direction_, _enemyManager.viewDistance, _enemyManager.projectileDestroyerLayer);

            // System.Array.Sort(hits_, (x, y) => x.distance.CompareTo(y.distance));

            if (hit_.collider != null && (hit_.collider.tag == "Player Trigger" || hit_.collider.tag == "Player Collider")) {
                Debug.DrawRay(projectileSpawner.position, direction_, Color.red);
                _isCanShootPlayer = true;
            }
            else {
                Debug.DrawRay(projectileSpawner.position, hit_.point - (Vector2)projectileSpawner.position, Color.blue);
            }
            
        }
    }
}

