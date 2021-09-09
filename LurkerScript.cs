using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LurkerScript : MonoBehaviour
{
    public GameObject Spike;
    public LayerMask GroundLayer;
    public AudioSource SpawnSFX;
    public AudioClip SpawnClip;
    public float SpikesFrecuency = 3f;
    public float SpikesInterval = 0.15f;
    public int SpikesCount = 5;
    public float SpawnRange = 15f;
    public GameObject TeleportEffect;
    public float TeleportRange = 15f;

    private float SpikesDistance = 1.25f;
    private bool HasTargetInRange = false;

    void Start()
    {
        StartCoroutine(SpawnSpikes());
    }

    void Update()
    {

    }

    public void OnTargetEnter() {
        if (!HasTargetInRange) {
            HasTargetInRange = true;
            StartCoroutine(SpawnSpikes());
        }
    }

    public void OnTargetLeft() {
        HasTargetInRange = false;
    }

    public void Teleport() {
        Transform targetPosition = FindObjectOfType<CharacterController2D>().transform;
        Vector2 verticalOffset = new Vector2(0, TeleportRange);
        float[] angles = new float[]{ -70f, -65f, -60f, -55f, -50f, -45f, 45f, 50f, 55f, 60f, 65f, 70f };
        angles = angles.OrderBy(x => Random.value).ToArray();

        for (int i = 0; i < angles.Length; i++) {
            Vector2 castDirection = Quaternion.Euler(0, 0, angles[i]) * Vector2.down;
            RaycastHit2D hit =  Physics2D.Raycast((Vector2)targetPosition.position + verticalOffset, castDirection, 300f, GroundLayer);
            if (hit && hit.normal.normalized != Vector2.right && hit.normal.normalized != Vector2.left) {
                GameObject go = Instantiate(TeleportEffect, transform.position, Quaternion.identity);
                go.transform.DOMove(hit.point, 1.5f);
                Destroy(go, 2f);
                transform.position = hit.point;
                break;
            }
        }
    }

    public IEnumerator SpawnSpikes() {
        while (HasTargetInRange) {
            Transform targetPosition = FindObjectOfType<CharacterController2D>().transform;
            Vector2 direction = new Vector2(1, -0.35f);
            RaycastHit2D hit = Physics2D.Raycast(targetPosition.position, direction, SpawnRange, GroundLayer);
            RaycastHit2D hitGround = new RaycastHit2D();

            if (hit) {
                direction = new Vector2(-1, -1);
                hitGround = Physics2D.Raycast(hit.point + new Vector2(-0.1f, 0), direction, 0.1f, GroundLayer);
            }

            if (hitGround) {
                for (int i = 0; i < SpikesCount; i++) {
                    GameObject go = Instantiate(Spike, hit.point + new Vector2(-SpikesDistance * i, 0), Quaternion.identity);
                    go.GetComponent<AudioSource>().pitch -= 0.02f * i;
                    SpawnSFX.PlayOneShot(SpawnClip);
                    SpawnSFX.pitch += 0.05f;
                    yield return new WaitForSeconds(SpikesInterval);
                }
                SpawnSFX.pitch = 1f;
            }
            else {
                direction = new Vector2(-1, -0.35f);
                hit = Physics2D.Raycast(targetPosition.position, direction, SpawnRange, GroundLayer);

                if (hit) {
                    direction = new Vector2(1, -1);
                    hitGround = Physics2D.Raycast(hit.point + new Vector2(0.1f, 0), direction, 0.1f, GroundLayer);
                }

                if (hitGround) {
                    for (int i = 0; i < SpikesCount; i++) {
                        GameObject go = Instantiate(Spike, hit.point + new Vector2(SpikesDistance * i, 0), Quaternion.identity);
                        go.GetComponent<AudioSource>().pitch -= 0.02f * i;
                        SpawnSFX.PlayOneShot(SpawnClip);
                        SpawnSFX.pitch += 0.05f;
                        yield return new WaitForSeconds(SpikesInterval);
                    }
                    SpawnSFX.pitch = 1f;
                }
            }
            yield return new WaitForSeconds(SpikesFrecuency);
        }
        
    }
}
