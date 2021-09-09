using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorableItem : MonoBehaviour
{
    public GlobalCharacterController GCC;
    public int Score;
    public GameObject ObtainedEffect;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (GCC == null)
            GCC = FindObjectOfType<GlobalCharacterController>();

        if (collision.CompareTag("Player")) {
            GCC.AddScore(Score);
            FindObjectOfType<CheckPointManager>().RegisterConsumable(gameObject.name);
            Destroy(Instantiate(ObtainedEffect, transform.position, Quaternion.identity), 3f);
            Destroy(gameObject);
        }
    }
}
