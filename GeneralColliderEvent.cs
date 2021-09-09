using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GeneralColliderEvent : MonoBehaviour
{
    public string TargetTag = string.Empty;
    public UnityEvent OnTargetEnter;
    public UnityEvent OnTagetLeave;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (TargetTag == string.Empty || collision.gameObject.CompareTag(TargetTag)) {
            OnTargetEnter.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (TargetTag == string.Empty || collision.gameObject.CompareTag(TargetTag)) {
            OnTagetLeave.Invoke();
        }
    }
}
