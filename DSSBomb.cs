using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DSSBomb : MonoBehaviour
{
    public float ArmingTime = 3f;
    public float DurationTime = 5f;
    public float Damage = 1f;
    public int RotationSpeed = 10;
    public float MaxDistance = 10f;
    public GameObject DestroyEffect;
    public AudioClip EnergyBeamSFX;

    public ParticleSystem RayStart;
    public LineRenderer Ray;
    public ParticleSystem RayImpact;
    public LayerMask ImpactMask;

    private bool IsShooting = false;

    void Start()
    {
        StartCoroutine(ActivateBomb());
        GetComponent<Rigidbody2D>().angularVelocity = Random.Range(-360f, 360f);
    }

    void Update()
    {
        if (IsShooting) {
            RaycastHit2D hit =  Physics2D.Raycast(RayStart.transform.position, RayStart.transform.forward, MaxDistance, ImpactMask);
            if (hit) {
                Ray.SetPosition(1, RayStart.transform.position);
                Ray.SetPosition(0, hit.point);
                RayImpact.transform.position = hit.point;
                if (hit.transform.CompareTag("Player")) {
                    hit.transform.GetComponent<CharacterController2D>().ApplyDamage(Damage, hit.point);
                }
            }
            else {
                Ray.SetPosition(1, RayStart.transform.position);
                Vector3 endPosition = RayStart.transform.position + (RayStart.transform.forward.normalized * MaxDistance);
                Ray.SetPosition(0, RayStart.transform.position + (RayStart.transform.forward.normalized * MaxDistance));
                RayImpact.transform.position = endPosition;
            }
        }
    }

    private IEnumerator ActivateBomb() {
        yield return new WaitForSeconds(ArmingTime);
        DisablePhysics();
        EnableRays();
        StartCoroutine(DisarmBomb());
    }

    public void DisablePhysics() {
        gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        gameObject.GetComponent<Rigidbody2D>().angularVelocity = 0f;
        gameObject.GetComponent<ContinuousRotation>().rotationsPerMinute = RotationSpeed;
    }

    public void EnableRays() {
        RayStart.gameObject.SetActive(true);
        Ray.gameObject.SetActive(true);
        RayImpact.gameObject.SetActive(true);
        IsShooting = true;

        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.clip = EnergyBeamSFX;
        audioSource.pitch = 1f;
        audioSource.Play();
    }

    private IEnumerator DisarmBomb() {
        yield return new WaitForSeconds(DurationTime);
        if (DestroyEffect != null)
            Destroy(Instantiate(DestroyEffect, transform.position, Quaternion.identity), 3f);
        Destroy(gameObject);
    }
}
