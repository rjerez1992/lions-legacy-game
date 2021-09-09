using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperController : MonoBehaviour {
    public GameObject Projectile;
    public ParticleSystem ShootEffect;
    public GameObject SighLine;
    public float AIMInterval = 2.5f;
    public float TimeBeforeShoot = 1.5f;
    public AudioClip AimSFX;
    public AudioClip ShootSFX;
    public Transform LaserPoint;

    private bool HasTarget = false;
    private GameObject Target;
    private AudioSource audioSource;
    private GameObject SighLineInstance;
    private bool IsAiming = false;
    private Vector2 direction;
    private Rigidbody2D rbody;


    void Start() {
        audioSource = GetComponent<AudioSource>();
        if (!audioSource) {
            Debug.LogError("No audio source attached to object");
        }
        rbody = GetComponent<Rigidbody2D>();
    }

    void Update() {

    }

    public void TargetInRange() {
        if (!HasTarget) {
            HasTarget = true;
            StartCoroutine(PrepareAIM());
            Target = FindObjectOfType<CharacterController2D>().gameObject;
            DisableMovement();
        }
    }

    public void TargetLeftRange() {
        HasTarget = false;
        Target = null;
        EnableMovement();
    }

    public void DisableMovement() {
        GetComponent<EnemyMovement>().enabled = false;
        rbody.velocity = Vector2.zero;
        rbody.isKinematic = true;
    }

    public void EnableMovement() {
        rbody.isKinematic = false;
        GetComponent<EnemyMovement>().enabled = true;
    }

    IEnumerator PrepareAIM() {
        yield return new WaitForSeconds(AIMInterval);
        if (HasTarget && !IsAiming) {
            IsAiming = true;
            audioSource.PlayOneShot(AimSFX);
            SighLineInstance = Instantiate(SighLine, Vector2.zero, Quaternion.identity, null);
            direction = (Target.transform.position - LaserPoint.position);//.normalized;
            SighLineInstance.GetComponent<LineRenderer>().SetPosition(0, LaserPoint.position);
            SighLineInstance.GetComponent<LineRenderer>().SetPosition(1, direction * 999);
            StartCoroutine(Shoot());
        }
    }

    IEnumerator Shoot() {
        yield return new WaitForSeconds(TimeBeforeShoot);
        Destroy(SighLineInstance);
        audioSource.PlayOneShot(ShootSFX);
        ShootEffect.Play();
        IsAiming = false;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Destroy(Instantiate(Projectile, LaserPoint.position, Quaternion.AngleAxis(angle - 90f, Vector3.forward), null), 5f);
        StartCoroutine(PrepareAIM());
    }

    private void OnDestroy() {
        if (SighLineInstance != null)
            Destroy(SighLineInstance);
    }
}
