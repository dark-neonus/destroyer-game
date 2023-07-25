using System.Collections.Generic;
using UnityEngine;

public class EnemyCannonBase : MonoBehaviour
{
    [HideInInspector]
    private EnemyManager _enemyManager;

    public float rotationSpeed;
    public List<GameObject> cannonPlatforms;
    
    void Start()
    {
        _enemyManager = GetComponentInParent<EnemyManager>();
    }

    void Update()
    {
        _RotateToPlayer();
        Vector2 direction_ = _enemyManager.player.position - transform.position;
    }

    private void _RotateToPlayer() {
        if (_enemyManager.isPlayerInViewDistance) {
            Vector2 direction_ = _enemyManager.player.position - transform.position;

            float angle_ = Mathf.MoveTowardsAngle(transform.eulerAngles.z, Mathf.Atan2(direction_.y, direction_.x) * Mathf.Rad2Deg, rotationSpeed * Time.deltaTime );

            transform.rotation = Quaternion.Euler(0f, 0f, angle_);
        }
    }
}
