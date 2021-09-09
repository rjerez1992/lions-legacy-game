using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PINBomb : MonoBehaviour
{
    public float ArmingTime = 3f;
    public GameObject ExplodeEffect;
    public GameObject Projectile;
    public float ProjectileAngleSpan = 15f;
    public float MaxTimeLife = 5f;

    void Start() {
        StartCoroutine(ActivateBomb());
        GetComponent<Rigidbody2D>().angularVelocity = Random.Range(-360f, 360f);
    }

    void Update() {

    }

    private IEnumerator ActivateBomb() {
        yield return new WaitForSeconds(ArmingTime);
        Destroy(Instantiate(ExplodeEffect, transform.position, Quaternion.identity), 3f);
        int projectileCount = Mathf.FloorToInt(360f / ProjectileAngleSpan);
        for (int i = 0; i < projectileCount; i++) {
            Destroy(Instantiate(Projectile, transform.position, Quaternion.Euler(0, 0, (i * ProjectileAngleSpan))), MaxTimeLife);
        }
        Destroy(gameObject);
    }
}
