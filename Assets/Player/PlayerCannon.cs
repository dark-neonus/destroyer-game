using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canon : MonoBehaviour
{
    

    // Update is called once per frame
    void Update()
    {
        RotateToMouse();
    }

    void RotateToMouse() {
        Vector2 mousePosition = Input.mousePosition;

        // Get the position of the object in world space
        Vector2 objectPosition = Camera.main.WorldToScreenPoint(transform.position);

        // Calculate the direction from the object to the cursor position
        Vector2 direction = mousePosition - objectPosition;

        // Calculate the angle in degrees
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Rotate the object to face the cursor position only on the z-axis
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
