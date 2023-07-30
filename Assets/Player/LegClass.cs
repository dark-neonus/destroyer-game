using UnityEngine;

public class LegClass : MonoBehaviour 
{
    [HideInInspector] public Vector2 movingOffsetDirection;
    [HideInInspector] public bool isGrounded;

    public LegClass oppositeLeg;

    public Transform target;
    public float maxDistance;
    public float minBaseDistance;
    public float moveSpeed;

    public Transform limb1;
    public Transform limb2;

    private Vector2 _staticLegPosition;
    private Vector2 _posTmp;

    public Transform legEnd;
    protected float _maxLimbLen;


    public void InitLeg() {
        _staticLegPosition = legEnd.position;
        isGrounded = true;
        _posTmp = transform.position;
    }

    protected void _Step() {
        _staticLegPosition = Vector2.ClampMagnitude(_staticLegPosition - (Vector2)limb1.position, _maxLimbLen) + (Vector2)limb1.position;
        legEnd.position = _staticLegPosition;
        if ((oppositeLeg == null || oppositeLeg.isGrounded) && isGrounded && (Vector2.Distance(legEnd.position, target.position) > maxDistance || Vector2.Distance(legEnd.position, limb1.transform.position) > _maxLimbLen || Vector2.Distance(legEnd.position, limb1.transform.position) < minBaseDistance)) {
            isGrounded = false;
        }
        
        if (!isGrounded) {
            _staticLegPosition += (Vector2)transform.position - _posTmp;
            _staticLegPosition = Vector2.MoveTowards(_staticLegPosition, target.position, moveSpeed);
            if (_staticLegPosition == (Vector2)target.position) {
                isGrounded = true;
            }
        }
        _posTmp = transform.position;
    }

    protected virtual void _InverseKinematics() {}
}
