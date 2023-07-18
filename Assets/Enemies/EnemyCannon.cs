using System.Collections.Generic;
using UnityEngine;

public class EnemyCannon : MonoBehaviour
{
    public Transform player;
    public float viewDistance;
    public float rotationSmooth;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        RotateToPlayer();
    }

    private void RotateToPlayer()
    {
        if (player != null) {
            if (Vector2.Distance(transform.position, player.position) <= viewDistance)
            {
                Vector2 direction = player.position - transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSmooth * Time.deltaTime);
            }
        }
    }
}

