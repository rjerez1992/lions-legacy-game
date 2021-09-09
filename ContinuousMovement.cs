using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuousMovement : MonoBehaviour
{
    public float Speed = 3f;
    public Vector2 Direction = Vector2.up;

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        transform.Translate(Direction * Speed * Time.deltaTime);
    }
}
