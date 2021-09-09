using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RandomTimeEnabler : MonoBehaviour
{
    public float MinTime = 1f;
    public float MaxTime = 3f;

    public UnityEvent OnEnable;

    void Start()
    {
        Invoke("EnableNow", Random.Range(MinTime, MaxTime));
    }

    void Update()
    {
        
    }

    public void EnableNow() {
        OnEnable.Invoke();
        this.enabled = false;
    }
}
