using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorSpike : MonoBehaviour
{
    public GameObject Spike;
    public float ArmingTime = 1f;
    public float SpikeSpeed = 7f;
    public ParticleSystem SpikeEffect;
    public AudioSource SpikeSFX;
    public GameObject DissapearEffect;

    private bool IsAttacking = false;
    private bool IsMovingUp = true;

    void Start()
    {
        StartCoroutine(DoAttack());
    }

    void Update()
    {
        if (IsAttacking) {
            if (IsMovingUp) {
                Spike.transform.Translate(Vector2.up * Time.deltaTime * SpikeSpeed);
                if (Spike.transform.localPosition.y >= 2.4)
                    IsMovingUp = false;
            }
            else {
                Spike.transform.Translate(Vector2.down * Time.deltaTime * SpikeSpeed);
                if (Spike.transform.localPosition.y <= -2.4) {
                    Destroy(Instantiate(DissapearEffect, transform.position, Quaternion.identity), 1f);
                    Destroy(gameObject);
                }
            }
        }
    }

    public IEnumerator DoAttack() {
        yield return new WaitForSeconds(ArmingTime);
        IsAttacking = true;
        SpikeEffect.Play();
        SpikeSFX.Play();
    }
}
