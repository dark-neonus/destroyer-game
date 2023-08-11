using UnityEngine;

public class BulletClass : MonoBehaviour
{
    public float damage;
    public float lifeTime;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}
