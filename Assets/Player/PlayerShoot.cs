using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{

    public CannonBase cannonBase;


    void Update() {
        _CheckShooting();
    }



    private void _CheckShooting() {
        if (Input.GetMouseButton(0)) {
            GameObject cannon_;
            foreach (GameObject platform_It in cannonBase.cannonPlatforms) {
                cannon_ = platform_It.transform.GetChild(0).gameObject;
                if (cannon_ != null) {
                    cannon_.GetComponent<PlayerCannon>().Shoot();
                }
            }
        }
    }
}
