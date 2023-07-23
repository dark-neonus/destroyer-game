using System.Collections.Generic;
using UnityEngine;

public class CannonBase : MonoBehaviour
{
    public float rotationSpeed;
    public List<GameObject> cannonPlatforms;
    
    void Update()
    {
        _RotateToMouse();
    }

    private void _RotateToMouse() {
        Vector2 mousePosition_ = Input.mousePosition;

        Vector2 objectPosition_ = Camera.main.WorldToScreenPoint(transform.position);

        Vector2 direction_ = mousePosition_ - objectPosition_;

        float angle_ = Mathf.MoveTowardsAngle(transform.eulerAngles.z, Mathf.Atan2(direction_.y, direction_.x) * Mathf.Rad2Deg, rotationSpeed * Time.deltaTime );

        transform.rotation = Quaternion.Euler(0f, 0f, angle_);
    }
}
