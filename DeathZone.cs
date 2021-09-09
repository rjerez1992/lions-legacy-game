using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            if (FindObjectOfType<GlobalCharacterController>().HasShadowStep) {
                FindObjectOfType<GlobalCharacterController>().DoShadowStep();
                return;
            }
            collision.gameObject.GetComponent<CharacterController2D>().ForceApplyDamage(9999f);
            FindObjectOfType<CameraFollow>().enabled = false;
        }
        else if(!collision.gameObject.CompareTag("DontKill")) {
            Destroy(collision.gameObject, 1f);
        }
    }
}
