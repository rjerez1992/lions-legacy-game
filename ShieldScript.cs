using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShieldScript : MonoBehaviour {
    public float ShieldResistance = 3f;
    public GameObject ShieldDestructionEffect;
    public float EffectScaleMultiplier = 2f;

    public UnityEvent OnShieldDestroyed;

    void Start() {

    }

    void Update() {

    }

    public void ApplyDamage(float damage) {
        ShieldResistance -= damage;
        if (ShieldResistance <= 0) {
            OnShieldDestroyed.Invoke();
            GameObject go = Instantiate(ShieldDestructionEffect, transform.position, Quaternion.identity, null);
            go.transform.localScale = (Vector3.one * EffectScaleMultiplier);
            //gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
