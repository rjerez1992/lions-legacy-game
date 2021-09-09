using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamageZone : MonoBehaviour
{
    //TODO: Add destructible damage zone
    public float damage = 1f;
    public float impactForce = 30f;
    public AudioSource damageAudio;
    public GameObject damageEffect;
    public bool DestroyAfterDamage = false;
    public bool DestroyOnTouchingGround = false;
    public GameObject destroyEffect;
    public bool HitEffectOnDamage = false;
    public bool AOEOnDestruction = false;
    public float AOERadius = 1f;
    public float AOEDamage = 1f;
    public bool DestroyOnTouchingAnything = false;

    public UnityEvent OnDealDamage;
    private Tweener ShakeEffect;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OnExternalDestruction() {
        if (AOEOnDestruction)
            DoAOEDamage();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        //TODO: Animate when hitting enemy
        if (collision.gameObject.CompareTag("Player")) {
            if (damageAudio != null)
                damageAudio.Play();
            if (damageEffect != null)
                Destroy(Instantiate(damageEffect, collision.contacts[0].point, Quaternion.identity), 2f);
            collision.gameObject.GetComponent<CharacterController2D>().ApplyDamage(damage, transform.position, impactForce);
            if (HitEffectOnDamage && !DestroyAfterDamage)
                DoHitEffect();
            if (DestroyAfterDamage) {
                if (AOEOnDestruction)
                    DoAOEDamage();
                Destroy(gameObject);
            }
            OnDealDamage.Invoke();
        }
        if ((DestroyOnTouchingGround && collision.gameObject.CompareTag("Ground")) || DestroyOnTouchingAnything) {
            if(destroyEffect != null)
                Destroy(Instantiate(destroyEffect, collision.contacts[0].point, Quaternion.identity), 2f);
            if (AOEOnDestruction)
                DoAOEDamage();
            Destroy(gameObject);
        }
    }

    private void DoHitEffect() {
        if (ShakeEffect != null)
            ShakeEffect.Restart();
        else
            ShakeEffect = transform.DOShakeScale(0.3f, 0.15f);
    }

    private void DoAOEDamage() {
        Collider2D[] collidersEnemies = Physics2D.OverlapCircleAll(transform.position, AOERadius);
        for (int i = 0; i < collidersEnemies.Length; i++) {
            if (collidersEnemies[i].gameObject.tag == "Player") {
                collidersEnemies[i].GetComponent<CharacterController2D>().ApplyDamage(AOEDamage, transform.position, 20f);
            }
        }
    }
}
