using System.Collections.Generic;
using UnityEngine;

public class CannonBase : MonoBehaviour
{
    public float rotationSpeed;
    public Transform body;
    public List<GameObject> cannonPlatforms;
    
    void Update()
    {
        RotateToMouse();
    }

    void RotateToMouse() {
        Vector2 mousePosition = Input.mousePosition;

        Vector2 objectPosition = Camera.main.WorldToScreenPoint(transform.position);

        Vector2 direction = mousePosition - objectPosition;

        float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.z, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg, rotationSpeed * Time.deltaTime );

        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
