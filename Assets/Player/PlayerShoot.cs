using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{

    public CannonBase cannonBase;


    void Update() {
        CheckShooting();
    }



    private void CheckShooting() {
        if (Input.GetMouseButton(0)) {
            GameObject cannon;
            foreach (GameObject platform in cannonBase.cannonPlatforms) {
                cannon = platform.transform.GetChild(0).gameObject;
                if (cannon != null) {
                    cannon.GetComponent<PlayerCannon>().Shoot();
                }
            }
        }
    }
}
