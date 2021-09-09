using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyWarden : MonoBehaviour
{
    public GameObject Boomerang;
    public bool IsPlayerOnRange = false;
    public float BoomerangFrecuency = 3f;
    private bool SpawnedFirst = false;
    private EnemyMovement MovementScript;
    public bool FollowTarget = true;

    void Start()
    {
        InvokeRepeating("SpawnProjectiles", 0f, 3);
        MovementScript = GetComponent<EnemyMovement>();
    }

    void Update()
    {
        
    }

    public void PlayerInRange() {
        IsPlayerOnRange = true;
        if (!SpawnedFirst) {
            SpawnBoomerang();
        }
        SpawnedFirst = true;
        if (FollowTarget) {
            MovementScript.FollowTarget = true;
        }
    }

    public void PlayerLeftRange() {
        IsPlayerOnRange = false;
        MovementScript.FollowTarget = false;
    }

    public void SpawnBoomerang() {
        GameObject bo = Instantiate(Boomerang, transform.position, transform.rotation, null);
        BoomerangMovement bm = bo.GetComponent<BoomerangMovement>();
        bm.targetPosition = FindObjectOfType<CharacterController2D>().gameObject.transform.position;
        bm.entityCaster = gameObject;
    }

    public void SpawnProjectiles() {
        if (IsPlayerOnRange) {
            if (SpawnedFirst)
                SpawnedFirst = false;
            else
                SpawnBoomerang();
        }
    }
}
