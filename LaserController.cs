using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    public float Damage = 1f;

    public ParticleSystem RayStart;
    public LineRenderer Ray;
    public ParticleSystem RayImpact;
    public LayerMask ImpactMask;
    public AudioClip EnergyBeamSFX;

    private float MaxDistance = 100f;

    void Start()
    {
        
    }

    void Awake() {
        RayStart.gameObject.SetActive(true);
        Ray.gameObject.SetActive(true);
        RayImpact.gameObject.SetActive(true);

        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.clip = EnergyBeamSFX;
        audioSource.pitch = 1f;
        audioSource.Play();
    }

    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(RayStart.transform.position, RayStart.transform.forward, MaxDistance, ImpactMask);
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
