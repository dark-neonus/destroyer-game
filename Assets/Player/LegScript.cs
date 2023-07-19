using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegScript : MonoBehaviour
{
    public Transform target;
    private Vector3 originalTargetPos;

    public float targetRandomOffset;
    public float movingOffset;
    [HideInInspector]
    public Vector3 movingOffsetDirection;

    public Transform foot;

    public GameObject limb;
    public float limbLengthFix;
    private SpriteRenderer limbSprite;

    public LegScript oppositeLimpScript;

    public float moveSpeed;
    public float maxDistance;
    public float maxError;
    public float targetOverthrow;
    public float maxLimbSize;

    private Vector3 fixPosition;
    private Vector3 legPosition;

    [HideInInspector]
    public bool Grounded;


    void Start() {
        fixPosition = target.position;
        limbSprite = limb.GetComponent<SpriteRenderer>();
        Grounded = true;
        originalTargetPos = target.localPosition;
    }

    void Update()
    {
        ShiftTarget();
        MoveLeg();
        Strech(limb, transform.position, legPosition, true);
        MoveFoot();
    }

    private void MoveLeg() {
        Grounded = true;
        if ((Vector3.Distance(target.position, fixPosition) > maxDistance) && (oppositeLimpScript == null || oppositeLimpScript.Grounded)) {
            fixPosition += (target.position - fixPosition) * targetOverthrow + new Vector3(Random.Range(-targetRandomOffset, targetRandomOffset), Random.Range(-targetRandomOffset, targetRandomOffset), 0);
            fixPosition = transform.position + Vector3.ClampMagnitude(fixPosition - transform.position, maxLimbSize);
        }
        else if (legPosition != fixPosition) {
            if (Vector3.Distance(legPosition, fixPosition) > maxError) {
            legPosition = Vector3.MoveTowards(legPosition, fixPosition, moveSpeed * Time.deltaTime * Mathf.Max(1.0f, 10 * Vector3.Distance(legPosition, fixPosition)));
            Grounded = false;
            }
            else {
                legPosition = fixPosition;
            }
        }
        if ((fixPosition - transform.position).magnitude > maxLimbSize) {
            fixPosition = target.position;
        }
    }

    

    public void Strech(GameObject _sprite,Vector3 _initialPosition, Vector3 _finalPosition, bool _mirrorZ) {
		Vector3 centerPos = (_initialPosition + _finalPosition) / 2f;
		_sprite.transform.position = centerPos;

		Vector3 direction = _finalPosition - _initialPosition;
		direction = Vector3.Normalize(direction);
		_sprite.transform.right = direction;

        Vector3 size = new Vector3(Vector3.Distance(_initialPosition, _finalPosition) * limbLengthFix, 1, 1);
        limb.transform.localScale = size;
	}

    private void MoveFoot() {
        foot.localRotation = limb.transform.localRotation;
        foot.position = legPosition;
    }

    private void ShiftTarget() {
        Vector3 tmp = originalTargetPos + Quaternion.Inverse(target.rotation) * movingOffsetDirection * movingOffset;
        target.localPosition = Vector3.MoveTowards(target.localPosition, tmp, 0.005f);
        // target.position += targetMovingOffset / reduceMovingOffset;
    }
}
