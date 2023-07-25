using UnityEngine;

public class PlayerBody : MonoBehaviour
{
    public float speed;
    public float rotationSpeed;
    private Vector2 _direction;

    private PlayerManager _playerManager;

    void Start()
    {
        _playerManager = GetComponentInParent<PlayerManager>();
    }

    void Update() {
        _TakeInput();
        _Rotate();
    }

    void FixedUpdate()
    {
        _Move();
        _ShiftLegs();
    }

    private void _Move() {
        // transform.Translate(direction * speed * Time.deltaTime);    
        _playerManager._rbVelocity = _direction * speed;   
    }

    private void _Rotate()
    {
        if (_direction != UnityEngine.Vector2.zero) {
            // Calculate the angle between the current direction and the right vector (1, 0)
            float targetAngle_ = UnityEngine.Vector2.SignedAngle(UnityEngine.Vector2.right, _direction);

            // Smoothly rotate the rectangle towards the target angle
            float angle_ = Mathf.MoveTowardsAngle(transform.eulerAngles.z, targetAngle_, rotationSpeed * Time.deltaTime );
            transform.rotation = UnityEngine.Quaternion.Euler(0f, 0f, angle_);
        }
    }

    private void _TakeInput()
    {
        _direction = UnityEngine.Vector2.zero;

        if (Input.GetKey(KeyCode.S))
        {
            _direction += UnityEngine.Vector2.down;
        }
        if (Input.GetKey(KeyCode.D))
        {
            _direction += UnityEngine.Vector2.right;
        }
        if (Input.GetKey(KeyCode.W))
        {
            _direction += UnityEngine.Vector2.up;
        }
        if (Input.GetKey(KeyCode.A))
        {
            _direction += UnityEngine.Vector2.left;
        }

        _direction.Normalize();
    }

    private void _ShiftLegs() {
        LegScript[] legScripts_ = GetComponentsInChildren<LegScript>();
        foreach (LegScript legScript_It in legScripts_) {
            legScript_It.movingOffsetDirection = _direction;
        }
        
    }
}