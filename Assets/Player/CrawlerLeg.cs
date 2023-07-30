using UnityEngine;

public class CrawlerLeg : LegClass
{
    public bool isMirror;

    private float _limb1Len;
    private float _limb2Len;
    

    private void Start() {
        InitLeg();
        _CalculateLengths();
    }

    private void Update() {
        _Step();
        _InverseKinematics();
    }

    protected override void _InverseKinematics() {
        Vector2 tmp_ = legEnd.position - limb1.position;
        limb1.rotation = Quaternion.AngleAxis(Mathf.Atan2(tmp_.y, tmp_.x) * Mathf.Rad2Deg, Vector3.forward);
        float legEndDistance_ = Vector2.Distance(legEnd.position, limb1.transform.position);

        limb2.localRotation = Quaternion.identity;

        if (_limb1Len * legEndDistance_ > 0.01f && legEndDistance_ < _maxLimbLen) {
            float cosTeor_ = (Mathf.Pow(_limb1Len, 2) + Mathf.Pow(legEndDistance_, 2) - Mathf.Pow(_limb2Len, 2)) / (2 * _limb1Len * legEndDistance_);

            float angle_ = Mathf.Acos(cosTeor_) * Mathf.Rad2Deg;
            limb1.Rotate(Vector3.forward, (isMirror ? -1f : 1f) * angle_, Space.Self);

            tmp_ = legEnd.position - limb2.position;
            limb2.rotation = Quaternion.AngleAxis(Mathf.Atan2(tmp_.y, tmp_.x) * Mathf.Rad2Deg, Vector3.forward);
        }
    }

    

    private void _CalculateLengths() {
        _limb1Len = Vector2.Distance(limb1.transform.position, limb2.transform.position);
        _limb2Len = Vector2.Distance(limb2.transform.position, legEnd.transform.position);
        _maxLimbLen = _limb1Len + _limb2Len;
    }
}
