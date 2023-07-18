using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegScript : MonoBehaviour
{
    public Transform target;
    public float targetRandomOffset;
    public Vector3 targetMovingOffset;
    public float reduceMovingOffset;
    private Vector3 originalTargetPos;

    public Transform foot;

    public GameObject limb;
    private SpriteRenderer limbSprite;
    private Vector2 originalSpriteSize;

    public GameObject oppositeLimp;
    private LegScript oppositeLimpScript;

    public float moveSpeed;
    public float maxDistance;
    public float maxError;

    private Vector3 fixPosition;
    private Vector3 legPosition;

    public bool Grounded;


    void Start() {
        fixPosition = target.position;
        limbSprite = limb.GetComponent<SpriteRenderer>();
        originalSpriteSize = limbSprite.size;
        Grounded = true;
        oppositeLimpScript = oppositeLimp.GetComponent<LegScript>();
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
        if (Vector3.Distance(target.position, fixPosition) > maxDistance && (oppositeLimpScript == null || oppositeLimpScript.Grounded)) {
            fixPosition += (target.position - fixPosition) * 1.5f + new Vector3(Random.Range(-targetRandomOffset, targetRandomOffset), Random.Range(-targetRandomOffset, targetRandomOffset), 0);
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
    }

    

    public void Strech(GameObject _sprite,Vector3 _initialPosition, Vector3 _finalPosition, bool _mirrorZ) {
		Vector3 centerPos = (_initialPosition + _finalPosition) / 2f;
		_sprite.transform.position = centerPos;

		Vector3 direction = _finalPosition - _initialPosition;
		direction = Vector3.Normalize(direction);
		_sprite.transform.right = direction;

        Vector3 size = new Vector3(Vector3.Distance(_initialPosition, _finalPosition) * 2.5f, 1, 1);
        limb.transform.localScale = size;
	}

    private void MoveFoot() {
        foot.localRotation = transform.localRotation;
        foot.position = legPosition;
    }

    private void ShiftTarget() {
        Vector3 tmp = originalTargetPos + Quaternion.Inverse(target.rotation) * (targetMovingOffset / reduceMovingOffset);
        target.localPosition = Vector3.MoveTowards(target.localPosition, tmp, 0.005f);
        // target.position += targetMovingOffset / reduceMovingOffset;
    }
}
