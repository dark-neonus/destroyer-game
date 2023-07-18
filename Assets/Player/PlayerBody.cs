using UnityEngine;

public class PlayerBody : MonoBehaviour
{
    public float speed;
    public float rotationSpeed;
    private UnityEngine.Vector2 direction;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
    }

    void Update() {
        TakeInput();
        Rotate();
    }

    void FixedUpdate()
    {
        Move();
        ShiftLegs();
    }

    private void Move() {
        // transform.Translate(direction * speed * Time.deltaTime);    
        rb.velocity = direction * speed;   
    }

    private void Rotate()
    {
        if (direction != UnityEngine.Vector2.zero) {
            // Calculate the angle between the current direction and the right vector (1, 0)
            float targetAngle = UnityEngine.Vector2.SignedAngle(UnityEngine.Vector2.right, direction);

            // Smoothly rotate the rectangle towards the target angle
            float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.z, targetAngle, rotationSpeed * Time.deltaTime );
            transform.rotation = UnityEngine.Quaternion.Euler(0f, 0f, angle);
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

    private void ShiftLegs() {
        LegScript[] legScripts = GetComponentsInChildren<LegScript>();
        foreach (LegScript legScript in legScripts) {
            legScript.targetMovingOffset = direction * speed;
        }
        
    }
}