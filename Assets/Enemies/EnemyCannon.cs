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
        _CheckPlayerView();
        _InteractWithPlayer();
        _currentCooldown = Mathf.Max(0, _currentCooldown - Time.deltaTime);
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
        if (_enemyManager.isPlayerInViewDistance) {
            _isCanShootPlayer = false;
            Vector2 direction_ = _enemyManager.player.position - projectileSpawner.position;

            RaycastHit2D[] hits_ = Physics2D.RaycastAll(projectileSpawner.position, direction_, Mathf.Infinity, _enemyManager.projectileDestroyerLayer);

            System.Array.Sort(hits_, (x, y) => x.distance.CompareTo(y.distance));

            foreach (var hit_It in hits_)
            {
                if (hit_It.collider.gameObject.layer == LayerMask.NameToLayer("Projectile Destroyer")) {
                    Debug.DrawRay(projectileSpawner.position, hit_It.point - (Vector2)projectileSpawner.position, Color.blue);
                    Debug.Log("Mountain");
                    break;
                }
                else if (hit_It.collider.tag == "Player Trigger") {
                    Debug.DrawRay(projectileSpawner.position, direction_, Color.red);
                    _isCanShootPlayer = true;
                    Debug.Log("Player");
                    break;
                }
            }
        }
    }
}

