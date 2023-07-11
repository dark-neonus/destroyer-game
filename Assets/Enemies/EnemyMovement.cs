using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform player;
    public float viewDistance;
    public float minDistance;
    public float moveSpeed;

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > viewDistance)
        {
            // Player is outside the view distance, do nothing
            return;
        }
        else if (distanceToPlayer < minDistance)
        {
            // Player is too close, move away from the player
            Vector3 direction = (transform.position - player.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
        else
        {
            // Player is within the view distance, move towards the player
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }
}
