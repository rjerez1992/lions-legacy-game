using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombarderScript : MonoBehaviour {
    public bool FacingRight = true;
    public float Speed = 1f;
    public SpawnProjectile ProjectileSpawner;
    public float BombStartOffset = 3f;
    public float BombDropRate = 1f;
    public float MaxBombSpawns = 10;

    private GameObject target;
    private int BombsSpawned = 0;
    private bool HasTargetOnRange = false;

    private int groundBlockers = 0;

    void Start() {
        target = FindObjectOfType<CharacterController2D>().gameObject;
        InvokeRepeating("SpawnBomb", BombStartOffset, BombDropRate);
        if (!FacingRight) {
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }
    }

    void Update() {
        if (HasTargetOnRange) {
            if (FacingRight)
                transform.Translate(Vector2.right * Speed * Time.deltaTime);
            else
                transform.Translate(Vector2.right * -Speed * Time.deltaTime);
        }
        if (DistanceToTarget() > 70) {
            Destroy(gameObject);
        }
    }

    public void OnTargetEnter() {
        HasTargetOnRange = true;  
    }

    public void OnTargetLeft() {
        HasTargetOnRange = false;
    }

    private float DistanceToTarget() {
        return Mathf.Abs(transform.position.x - target.transform.position.x);
    }

    public void SpawnBomb() {
        if (HasTargetOnRange) {
            if (BombsSpawned < MaxBombSpawns && groundBlockers <= 0) {
                ProjectileSpawner.SpawnProjectiles();
                BombsSpawned++;
            }
            else if (BombsSpawned >= MaxBombSpawns) {
                Destroy(gameObject);
            }
        }
    }

    public void OnGroundBlock() {
        groundBlockers++;
    }

    public void OnGroundUnblock() {
        groundBlockers--;
    }
}
