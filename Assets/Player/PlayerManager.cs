using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private CannonBase[] _cannonBases;
    private Rigidbody2D _rb;
    [HideInInspector]
    public Vector2 _rbVelocity;
    [HideInInspector]
    public bool canStepOnOcean;

    

    void Start() {
        _cannonBases = GetComponentsInChildren<CannonBase>();
        _rb = GetComponent<Rigidbody2D>();
        canStepOnOcean = false;
    }

    private void Update() {
        _CheckShooting();
    }

    private void LateUpdate() {
        _Move();
    }

    private void FixedUpdate() {
        GameManager.gameManager.ActiveProcessCoordinates(transform, 1.2f);
    }
    

    private void _Move() {
        _rb.velocity = _rbVelocity;
    }


    private void _CheckShooting() {
        if (Input.GetMouseButton(0)) {
            GameObject cannon_;
            foreach (GameObject platform_It in GameManager.gameManager.cannonPlatforms) {
                cannon_ = platform_It.transform.GetChild(0).gameObject;
                if (cannon_ != null) {
                    cannon_.GetComponent<PlayerCannon>().Shoot();
                }
            }
        }
    }
}
