using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private CannonBase _cannonBase;
    private Rigidbody2D _rb;
    [HideInInspector]
    public Vector2 _rbVelocity;
    [HideInInspector]
    public bool canStepOnOcean;

    void Start() {
        _cannonBase = GetComponentInChildren<CannonBase>();
        _rb = GetComponent<Rigidbody2D>();
        canStepOnOcean = false;
    }

    private void Update() {
        _CheckShooting();
        
    }

    private void LateUpdate() {
        _Move();
    }
    

    private void _Move() {
        _rb.velocity = _rbVelocity;
    }


    private void _CheckShooting() {
        if (Input.GetMouseButton(0)) {
            GameObject cannon_;
            foreach (GameObject platform_It in _cannonBase.cannonPlatforms) {
                cannon_ = platform_It.transform.GetChild(0).gameObject;
                if (cannon_ != null) {
                    cannon_.GetComponent<PlayerCannon>().Shoot();
                }
            }
        }
    }

/*
    private void OnTriggerEnter2D(Collider2D collider_)
    {
        if (collider_.CompareTag("Terrain"))
        {
            Debug.Log("Step on a tile");
            int biomeID = collider_.GetComponent<TileInfo>().biomeID;
            if (biomeID == TileInfo.biomeIDs["Ocean"] && !canStepOnOcean)
            {
                Debug.Log("Step on the ocean");
                // Calculate the collision normal and adjust the player's velocity.
                Vector2 collisionNormal = (transform.position - collider_.transform.position).normalized;
                _rbVelocity -= Vector2.Dot(_rbVelocity, collisionNormal) * collisionNormal * 1.1f;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collider_) {
        OnTriggerEnter2D(collider_);
    }
    */
    private void OnTriggerStay2D(Collider2D collider)
    {
        if (!canStepOnOcean && collider.CompareTag("Terrain"))
        {
            int biomeID = collider.gameObject.GetComponent<TileInfo>().biomeID;
            if (biomeID == TileInfo.biomeIDs["Ocean"])
            {
                // Calculate the collision normal.
                Vector2 collisionNormal = (transform.position - collider.transform.position).normalized;

                // Calculate the projection of velocity onto the collision normal.
                float dotProduct = Vector2.Dot(_rbVelocity, collisionNormal);
                Vector2 projection = dotProduct * collisionNormal;

                // Adjust the player's velocity by subtracting the projection.
                _rbVelocity -= projection;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collider_) {
        OnTriggerStay2D(collider_);
    }
}
