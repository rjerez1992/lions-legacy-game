using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZenController : MonoBehaviour
{
    public enum SpawnType { RANDOM, STAR, ROTATING }

    public GameObject Projectile;
    public SpawnType SpawnMode;
    public float SpawnFrecuency = 5f;
    public float SpawnDuration = 3f;
    public float SpawnRate = 0.5f;
    public float SpawnForce = 1f;
    public float ProjectileDuration = 2f;
    public float RotationRate = 5f;
    public Transform SpawnPosition;
    public bool FollowTarget = true;


    private bool IsSpawning = false;
    private float RotatingOffset = 0f;
    private EnemyMovement MovementScript;
    private bool HasTarget = false;    

    void Start()
    {
        MovementScript = GetComponent<EnemyMovement>();
    }

    
    void Update()
    {
        
    }

    public void OnTargetEnter() {
        if (!HasTarget && !IsSpawning) {
            HasTarget = true;
            StartCoroutine(SpawnSchedule());
        }
        if (FollowTarget) {
            MovementScript.FollowTarget = true;
        }
    }

    public void OnTargetLeft() {
        HasTarget = false;
        MovementScript.FollowTarget = false;
    }

    public IEnumerator SpawnSchedule() {
        while (HasTarget) {
            IsSpawning = true;
            MovementScript.DisableMovement();
            StartCoroutine(SpawnProjectiles());
            yield return new WaitForSeconds(SpawnDuration);
            MovementScript.EnableMovement();
            IsSpawning = false;
            yield return new WaitForSeconds(SpawnFrecuency);
        }
    }

    public IEnumerator SpawnProjectiles() {
        while (IsSpawning) {
            switch (SpawnMode) {
                case SpawnType.RANDOM:
                    SpawnRandom();
                    break;
                case SpawnType.STAR:
                    SpawnStar();
                    break;
                case SpawnType.ROTATING:
                    SpawnRotating();
                    break;
            }

            yield return new WaitForSeconds(SpawnRate);
        }        
    }

    private void SpawnRandom() {
        float randomAngle = Random.Range(-140, 140);
        Vector2 projectileDirection = Quaternion.Euler(0, 0, randomAngle) * Vector2.up;
        GameObject projectileInstance = Instantiate(Projectile, SpawnPosition.position, Quaternion.identity);
        projectileInstance.GetComponent<Rigidbody2D>().AddForce(projectileDirection * SpawnForce, ForceMode2D.Impulse);
        Destroy(projectileInstance, ProjectileDuration);
    }

    private void SpawnStar() {
        float starPoints = 6;
        float step = 360 / starPoints;
        for (int i = 0; i < starPoints; i++) {
            Vector2 projectileDirection = Quaternion.Euler(0, 0, step * i) * Vector2.up;
            GameObject projectileInstance = Instantiate(Projectile, SpawnPosition.position, Quaternion.identity);
            projectileInstance.GetComponent<Rigidbody2D>().AddForce(projectileDirection * SpawnForce, ForceMode2D.Impulse);
            Destroy(projectileInstance, ProjectileDuration);
        }
    }

    private void SpawnRotating() {
        float rotatingAxis = 4;
        float step = 360 / rotatingAxis;
        RotatingOffset += RotationRate;
        for (int i = 0; i < rotatingAxis; i++) {
            Vector2 projectileDirection = Quaternion.Euler(0, 0, (step * i) + RotatingOffset) * Vector2.up;
            GameObject projectileInstance = Instantiate(Projectile, SpawnPosition.position, Quaternion.identity);
            projectileInstance.GetComponent<Rigidbody2D>().AddForce(projectileDirection * SpawnForce, ForceMode2D.Impulse);
            Destroy(projectileInstance, ProjectileDuration);
        }
    }
}
