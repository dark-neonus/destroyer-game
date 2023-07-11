using System.Xml.Schema;
using System;
using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float rotationSpeed;
    private UnityEngine.Vector2 direction;
    public Transform body;

    void Update() {
        TakeInput();
        Rotate();
        Move();
    }

    private void Move() {
        transform.Translate(direction * speed * Time.deltaTime);        
    }

    private void Rotate()
    {
        if (direction != UnityEngine.Vector2.zero) {
            // Calculate the angle between the current direction and the right vector (1, 0)
            float targetAngle = UnityEngine.Vector2.SignedAngle(UnityEngine.Vector2.right, direction);

            // Smoothly rotate the rectangle towards the target angle
            float angle = Mathf.MoveTowardsAngle(body.eulerAngles.z, targetAngle, rotationSpeed * Time.deltaTime );
            body.rotation = UnityEngine.Quaternion.Euler(0f, 0f, angle);
        }
    }

    private void TakeInput()
    {
        direction = UnityEngine.Vector2.zero;

        if (Input.GetKey(KeyCode.S))
        {
            direction += UnityEngine.Vector2.down;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction += UnityEngine.Vector2.right;
        }
        if (Input.GetKey(KeyCode.W))
        {
            direction += UnityEngine.Vector2.up;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction += UnityEngine.Vector2.left;
        }

        direction.Normalize();
    }


}