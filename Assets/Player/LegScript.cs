using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegScript : MonoBehaviour
{
    public Transform target;
    private Vector3 _originalTargetPos;

    public float targetRandomOffset;
    public float movingOffset;
    [HideInInspector]
    public Vector3 movingOffsetDirection;

    public Transform foot;

    public GameObject limb;
    public float limbLengthFix;
    private SpriteRenderer _limbSprite;

    public LegScript oppositeLimpScript;

    public float moveSpeed;
    public float maxDistance;
    public float maxError;
    public float targetOverthrow;
    public float maxLimbSize;

    private Vector3 _fixPosition;
    private Vector3 _legPosition;

    [HideInInspector]
    public bool isGrounded;


    void Start() {
        _limbSprite = limb.GetComponent<SpriteRenderer>();
        isGrounded = true;
        InitLegsPosition();
    }

    void Update()
    {
        _ShiftTarget();
        _MoveLeg();
        _Strech(limb, transform.position, _legPosition, true);
        _MoveFoot();
    }

    private void _MoveLeg() {
        isGrounded = true;
        if ((Vector3.Distance(target.position, _fixPosition) > maxDistance) && (oppositeLimpScript == null || oppositeLimpScript.isGrounded)) {
            _fixPosition += (target.position - _fixPosition) * targetOverthrow + new Vector3(Random.Range(-targetRandomOffset, targetRandomOffset), Random.Range(-targetRandomOffset, targetRandomOffset), 0);
            _fixPosition = transform.position + Vector3.ClampMagnitude(_fixPosition - transform.position, maxDistance);
        }
        else if (_legPosition != _fixPosition) {
            if (Vector3.Distance(_legPosition, _fixPosition) > maxError) {
            _legPosition = Vector3.MoveTowards(_legPosition, _fixPosition, moveSpeed * Time.deltaTime * Mathf.Max(1.0f, 10 * Vector3.Distance(_legPosition, _fixPosition)));
            isGrounded = false;
            }
            else {
                _legPosition = _fixPosition;
            }
        }
        if ((_fixPosition - transform.position).magnitude > maxLimbSize) {
            _fixPosition = target.position;
        }
        _legPosition = transform.position + Vector3.ClampMagnitude(_legPosition - transform.position, maxLimbSize);
    }

    

    private void _Strech(GameObject _sprite,Vector3 _initialPosition, Vector3 _finalPosition, bool _mirrorZ) {
        // Vector3 tmp_ = 
        _finalPosition = Vector3.ClampMagnitude(_finalPosition - transform.position, maxLimbSize) + transform.position;
		Vector3 centerPos_ = (_initialPosition + _finalPosition) / 2f;
		_sprite.transform.position = centerPos_;

		Vector3 direction_ = _finalPosition - _initialPosition;
		direction_ = Vector3.Normalize(direction_);
		_sprite.transform.right = direction_;

        Vector3 size_ = new Vector3(Mathf.Min(Vector3.Distance(_initialPosition, _finalPosition), maxLimbSize) * limbLengthFix, 1, 1);
        limb.transform.localScale = size_;
	}

    private void _MoveFoot() {
        foot.localRotation = limb.transform.localRotation;
        foot.position = _legPosition;
    }

    private void _ShiftTarget() {
        Vector3 tmp_ = _originalTargetPos + Quaternion.Inverse(target.rotation) * movingOffsetDirection * movingOffset;
        target.localPosition = Vector3.MoveTowards(target.localPosition, tmp_, 0.005f);
        // target.position += targetMovingOffset / reduceMovingOffset;
    }

    public void InitLegsPosition() {
        _fixPosition = target.position;
        _legPosition = _fixPosition;
        _originalTargetPos = target.localPosition;
    }
}
