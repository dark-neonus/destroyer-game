using UnityEngine;

public class WalkerLeg : LegClass
{
    // private SpriteRenderer _limb1Sprite;
    private float _limq1SpriteSize;

    private void Start() {
        InitLeg();
        _maxLimbLen = legEnd.localPosition.magnitude;
        //_limb1Sprite = limb1.GetComponent<SpriteRenderer>();
        _limq1SpriteSize = limb1.GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void Update() {
        _Step();
        _InverseKinematics();
    }

    protected override void _InverseKinematics() {
        limb2.position = legEnd.position;

        Vector2 tmp_ = legEnd.position - limb1.position;
        limb1.rotation = Quaternion.AngleAxis(Mathf.Atan2(tmp_.y, tmp_.x) * Mathf.Rad2Deg, Vector3.forward);
        limb2.rotation = limb1.rotation;

        limb1.localScale = new Vector3(Vector3.Distance(limb1.position, legEnd.position) / _limq1SpriteSize, limb1.localScale.y, 1f);
    }
}
