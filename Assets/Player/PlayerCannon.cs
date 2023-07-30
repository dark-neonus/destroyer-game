using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCannon : MonoBehaviour
{
    public List<Transform> projectileSpawners;
    public GameObject projectile;
    public float projectileForce;
    public float cooldown;
    private float _currentCooldown;

    private Transform _sprite;
    public float kickbackDuration;
    public float kickbackOffset;
    private float _kickbackTargetPosition;
    private bool _isKickbacking;


    void Start()
    {
        _isKickbacking = false;
        _currentCooldown = 0;
        _sprite = GetComponentInChildren<SpriteRenderer>().gameObject.transform;
    }

    void Update()
    {
        _RotateToMouse();
        _currentCooldown = Mathf.Max(0, _currentCooldown - Time.deltaTime);
    }

    private void _RotateToMouse() {
        Vector2 mousePosition_ = Input.mousePosition;

        Vector2 objectPosition_ = Camera.main.WorldToScreenPoint(transform.position);

        Vector2 direction_ = mousePosition_ - objectPosition_;

        float angle_ = Mathf.Atan2(direction_.y, direction_.x) * Mathf.Rad2Deg;
        
        transform.rotation = Quaternion.Euler(0f, 0f, angle_);
    }

    public void Shoot() {
        if (_currentCooldown == 0) {
            // Vector2 mousePos_ = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // Vector2 myPos_ = transform.position;
            Vector2 direction_;

            foreach (Transform projectileSpawnet_It in projectileSpawners) {
                direction_ = projectileSpawnet_It.right.normalized;
                GameObject bullet = Instantiate(projectile, projectileSpawnet_It.transform.position, projectileSpawnet_It.rotation);
                bullet.GetComponent<Rigidbody2D>().velocity = direction_ * projectileForce;
            }

            _currentCooldown = cooldown;

            if (!_isKickbacking) {
                StartCoroutine(_Kickback());
            }
        }
    }

    private IEnumerator _Kickback() {
        _isKickbacking = true;
        float timer_ = 0f;
        _kickbackTargetPosition = _sprite.localPosition.x - kickbackOffset;

        while (timer_ < kickbackDuration / 2) {
            timer_ += Time.deltaTime;
            _sprite.localPosition = new Vector3(Mathf.Lerp(_sprite.localPosition.x, _kickbackTargetPosition, timer_ / kickbackDuration * 2), _sprite.localPosition.y, _sprite.localPosition.z);
            yield return null;
        }

        _sprite.localPosition = new Vector3(_kickbackTargetPosition, _sprite.localPosition.y, _sprite.localPosition.z);


        timer_ = 0f;
        _kickbackTargetPosition = _sprite.localPosition.x + kickbackOffset;

        while (timer_ < kickbackDuration / 2) {
            timer_ += Time.deltaTime;
            _sprite.localPosition = new Vector3(Mathf.Lerp(_sprite.localPosition.x, _kickbackTargetPosition, timer_ / kickbackDuration * 2), _sprite.localPosition.y, _sprite.localPosition.z);
            yield return null;
        }
        _sprite.localPosition = new Vector3(_kickbackTargetPosition, _sprite.localPosition.y, _sprite.localPosition.z);
        _isKickbacking = false;
    }

}
