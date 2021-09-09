using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TronsbonkController : MonoBehaviour
{
    [Header("UI Settings")]
    public GameObject BossCanvas;
    public Slider Health;
    public Text Name;

    [Header("Weapons Settings")]
    public GameObject BasicProjectile;
    public GameObject BasicMissile;
    public LayerMask Stage2LaserMask;
    public GameObject Stage3Projectile;
    public GameObject Stage3BulletHell;

    [Header("Audio Settings")]
    public AudioSource AttackAnnounce;
    public AudioClip LaserAnnounce;
    public AudioClip MissileAnnounce;
    public AudioClip Stage2Announce;
    public AudioClip MoveAnnounce;
    public AudioClip[] Stage3Voices;
    public AudioClip Stage3VoicePresentation;
    public AudioClip ExplosionLoad;
    public AudioClip ExplosionExplode;

    [Header("Prefabs Settings")]
    public GameObject Laser1;
    public GameObject Laser2;
    public GameObject MissileSpawner1;
    public GameObject MissileSpawner2;
    public GameObject MissileEffect;
    public GameObject MissileEffect2;
    public GameObject LiftOffEffect;
    public GameObject ThrustEffect;
    public GameObject TronsbonkSprite1;
    public GameObject TronsbonkSprite2;
    public Transform[] TargetPositions;
    public GameObject AppearingPlaforms;
    public GameObject FireExplosion;
    public GameObject SoulExplosionBig;
    public GameObject BackTrigger;
    public GameObject[] LasersStage3;
    public GameObject MegaExplosion;
    public GameObject EnableOnDeath;
    public GameObject Stage3HellSpawn;

    private DestructibleObject destructibleScript;
    private float lifeRecord;
    private float skillFrecuency = 5;
    private float movementFrecuency = 5;
    private float elapsedCyclesSkill = 0;
    private float elapsedCyclesMovement = 0;
    private bool doingSpecialAttack = false;
    private bool prioritizeMissiles = false;
    private int currentStage = 1;
    private bool projectileSwitch = false;
    private Tweener moveTween;
    private bool doingMovement = false;
    

    void Start()
    {
        BossCanvas.SetActive(true);
        destructibleScript = GetComponent<DestructibleObject>();
        lifeRecord = destructibleScript.LifePercentage();
        Health.DOValue(lifeRecord, 1f);
        StartCoroutine(DoSkills());
    }

    void Update()
    {
        if (destructibleScript.LifePercentage() != lifeRecord) {
            lifeRecord = destructibleScript.LifePercentage();
            Health.DOComplete();
            Health.DOValue(lifeRecord, 0.4f);
        }
        if (currentStage == 1 && destructibleScript.LifePercentage() <= 0.6f) {
            StartCoroutine(EnableStageTwo());
        }
        else if (currentStage == 2 && destructibleScript.LifePercentage() <= 0.35f) {
            StartCoroutine(EnableStageThree());
        }
        else if (currentStage == 3 && destructibleScript.LifePercentage() <= 0.01f) {
            StartCoroutine(DefeatStage());
        }
    }

    private IEnumerator DoSkills() {
        while (1 == 1) {
            yield return new WaitForSeconds(1);
            if (!doingSpecialAttack) {
                float r = Random.value;
                switch (currentStage) {
                    case 1:
                        //STAGE 1
                        if (elapsedCyclesSkill >= skillFrecuency && Random.value > 0.6) {
                            elapsedCyclesSkill = 0;
                            doingSpecialAttack = true;
                            if (r > 0.5f || prioritizeMissiles) {
                                DoMissileAttack();
                                yield return new WaitForSeconds(2);
                                doingSpecialAttack = false;
                            }
                            else {
                                DoLaserAttack();
                            }
                        }
                        else {
                            elapsedCyclesSkill++;
                            SpawnNormalProjectile();
                        }
                        break;
                    case 2:
                        //STAGE 2
                        if (elapsedCyclesSkill >= skillFrecuency && Random.value > 0.3) {
                            elapsedCyclesSkill = 0;
                            doingSpecialAttack = true;
                            if (r > 0.4f ) {
                                DoMissileAttack();
                                yield return new WaitForSeconds(2);
                                doingSpecialAttack = false;
                            }
                            else {
                                StartCoroutine(DoRotationLaser());
                            }
                        }
                        else {
                            elapsedCyclesSkill++;
                            SpawnNormalProjectile2();
                        }
                        break;
                    case 3:
                        //STAGE 3
                        if (elapsedCyclesSkill >= skillFrecuency && Random.value > 0.15) {
                            elapsedCyclesSkill = 0;
                            doingSpecialAttack = true;
                            if (r > 0.5f) {
                                StartCoroutine(DoBulletHell());
                            }
                            else {
                                StartCoroutine(DoDeathLasers());
                            }
                        }
                        else {
                            elapsedCyclesSkill++;
                            DoStage3SimpleProjectile();
                        }
                        break;
                    default:
                        //NOTE: Changing stage in progress
                        break;
                }
            }
        }
    }

    public IEnumerator DoMovement() {
        while (1 == 1) {
            yield return new WaitForSeconds(1);
            if (!doingSpecialAttack && !doingMovement && currentStage != -1) {
                float r = Random.value;
                if (elapsedCyclesMovement >= movementFrecuency && Random.value > 0.4) {
                    elapsedCyclesMovement = 0;
                    doingMovement = true;
                    if(moveTween != null)
                        moveTween.Kill();
                    Transform targetPos;
                    int indexPos;
                    do {
                        indexPos = Random.Range(0, TargetPositions.Length);
                        targetPos = TargetPositions[indexPos];
                    } while (indexPos == 2 || targetPos.position == transform.position);
                    AttackAnnounce.PlayOneShot(MoveAnnounce);
                    moveTween = transform.DOMove(targetPos.position, 3f);
                    yield return new WaitForSeconds(2);
                    doingMovement = false;
                }
                else
                    elapsedCyclesMovement++;
            }
        }
    }

    private void SpawnNormalProjectile() {
        float r = Random.value;
        float positionChange = 0.4f;
        if (r > 0.8)
            positionChange = 4f;
        else if (r > 0.4)
            positionChange = 2.3f;
        Vector2 spawnPosition = transform.position;
        spawnPosition.y += 1 * positionChange;
        Destroy(Instantiate(BasicProjectile, spawnPosition, Quaternion.Euler(0, 0, 90f)), 3f);
    }

    private void DoLaserAttack() {
        //NOTE: Enables 2 lasers that do half a swipe in the front of the boss
        AttackAnnounce.PlayOneShot(LaserAnnounce);
        Laser1.transform.rotation = Quaternion.Euler(0f, 0f, -160f);
        Laser2.transform.rotation = Quaternion.Euler(0f, 0f, -180f);
        Laser1.SetActive(true);
        Laser2.SetActive(true);
        Invoke("DisableLasers", 5f);
    }

    public void DoMissileAttack() {
        //NOTE: Spawns 6 missiles that take random time to activate and go to target position (+ randomness)
        AttackAnnounce.PlayOneShot(MissileAnnounce);

        MissileEffect.SetActive(true);
        MissileEffect2.SetActive(true);
        Invoke("DisableMissileEffects", 2f);
        int defaultMissiles = 3;

        if (prioritizeMissiles)
            defaultMissiles = 5;

        for (int i = 0; i < defaultMissiles; i++) {
            float randomAngle = Random.Range(-10f, 60f);
            Vector2 projectileDirection = Quaternion.Euler(0, 0, randomAngle) * Vector2.up;
            GameObject go = Instantiate(BasicMissile, MissileSpawner1.transform.position, Quaternion.identity);
            go.GetComponent<Rigidbody2D>().AddForce(projectileDirection * Random.Range(5f, 8.5f), ForceMode2D.Impulse);
            Destroy(go, 5f);

            randomAngle = Random.Range(-10f, 60f);
            projectileDirection = Quaternion.Euler(0, 0, randomAngle) * Vector2.up;
            go = Instantiate(BasicMissile, MissileSpawner2.transform.position, Quaternion.identity);
            go.GetComponent<Rigidbody2D>().AddForce(projectileDirection * Random.Range(5f, 8.5f), ForceMode2D.Impulse);
            Destroy(go, 5f);
        }
    }

    private void SpawnNormalProjectile2() {
        //NOTE: Spawns normal projectiles horizontal, then diagonal
        float px = transform.position.x;
        float py = transform.position.y;
        float ds = 2f;

        if (projectileSwitch) {
            Destroy(Instantiate(BasicProjectile, new Vector2(px, py + ds), Quaternion.Euler(0, 0, 0f)), 3f);
            Destroy(Instantiate(BasicProjectile, new Vector2(px + ds, py), Quaternion.Euler(0, 0, -90f)), 3f);
            Destroy(Instantiate(BasicProjectile, new Vector2(px, py - ds), Quaternion.Euler(0, 0, -180f)), 3f);
            Destroy(Instantiate(BasicProjectile, new Vector2(px - ds, py), Quaternion.Euler(0, 0, -270f)), 3f);
        }
        else {
            Destroy(Instantiate(BasicProjectile, new Vector2(px - ds, py + ds), Quaternion.Euler(0, 0, 45f)), 3f);
            Destroy(Instantiate(BasicProjectile, new Vector2(px - ds, py - ds), Quaternion.Euler(0, 0, 135f)), 3f);
            Destroy(Instantiate(BasicProjectile, new Vector2(px + ds, py - ds), Quaternion.Euler(0, 0, 225f)), 3f);
            Destroy(Instantiate(BasicProjectile, new Vector2(px + ds, py + ds), Quaternion.Euler(0, 0, 315f)), 3f);
        }
        projectileSwitch = !projectileSwitch;
    }

    private IEnumerator DoRotationLaser() {
        //NOTE: Centers on the scenary and does N lasers rotating (Stage 2, replaces basic laser attack)
        AttackAnnounce.PlayOneShot(LaserAnnounce);
        transform.DOMove(TargetPositions[2].position, 2f);
        yield return new WaitForSeconds(2f);

        AttackAnnounce.PlayOneShot(LaserAnnounce);
        Laser1.GetComponent<LaserController>().ImpactMask = Stage2LaserMask;
        Laser2.GetComponent<LaserController>().ImpactMask = Stage2LaserMask;
        Laser1.GetComponent<ContinuousRotation>().rotationsPerMinute = -3.5f;
        Laser2.GetComponent<ContinuousRotation>().rotationsPerMinute = -3.5f;
        Laser1.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        Laser2.transform.rotation = Quaternion.Euler(0f, 0f, -180f);
        Laser1.SetActive(true);
        Laser2.SetActive(true);
        yield return new WaitForSeconds(5f);

        Laser1.SetActive(false);
        Laser2.SetActive(false);
        doingSpecialAttack = false;
    }

    private void DoStage3SimpleProjectile() {
        float px = transform.position.x;
        float py = transform.position.y;
        float ds = 2f;
        Destroy(Instantiate(Stage3Projectile, new Vector2(px, py + ds), Quaternion.Euler(0, 0, 0f)), 3f);
        Destroy(Instantiate(Stage3Projectile, new Vector2(px + ds, py), Quaternion.Euler(0, 0, -90f)), 3f);
        Destroy(Instantiate(Stage3Projectile, new Vector2(px, py - ds), Quaternion.Euler(0, 0, -180f)), 3f);
        Destroy(Instantiate(Stage3Projectile, new Vector2(px - ds, py), Quaternion.Euler(0, 0, -270f)), 3f);
        Destroy(Instantiate(Stage3Projectile, new Vector2(px - ds, py + ds), Quaternion.Euler(0, 0, 45f)), 3f);
        Destroy(Instantiate(Stage3Projectile, new Vector2(px - ds, py - ds), Quaternion.Euler(0, 0, 135f)), 3f);
        Destroy(Instantiate(Stage3Projectile, new Vector2(px + ds, py - ds), Quaternion.Euler(0, 0, 225f)), 3f);
        Destroy(Instantiate(Stage3Projectile, new Vector2(px + ds, py + ds), Quaternion.Euler(0, 0, 315f)), 3f);
    }

    private IEnumerator DoBulletHell() {
        //Move to center, then pick any of the four location and move to each of them (square)
        transform.DOMove(TargetPositions[2].position, 2f);
        yield return new WaitForSeconds(1f);
        AttackAnnounce.PlayOneShot(Stage3Voices[3]);
        yield return new WaitForSeconds(1f);

        float rotatingAxis = 4;
        float step = 360 / rotatingAxis;
        float RotatingOffset = 0f;
        float RotationRate = 20f;

        for (int x = 0; x < 25; x++) {
            RotatingOffset += RotationRate;
            for (int i = 0; i < rotatingAxis; i++) {
                Destroy(Instantiate(Stage3BulletHell, Stage3HellSpawn.transform.position, Quaternion.Euler(0, 0, (step * i) + RotatingOffset)), 3f);
            }
            yield return new WaitForSeconds(0.25f);
        }

        doingSpecialAttack = false;
    }

    private IEnumerator DoDeathLasers() {
        //NOTE: Spawns lasers from the core (N more than stage 2) (Stage 3)
       
        transform.DOMove(TargetPositions[2].position, 2f);
        yield return new WaitForSeconds(1f);
        AttackAnnounce.PlayOneShot(LaserAnnounce);
        yield return new WaitForSeconds(1f);

        LasersStage3[0].transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        LasersStage3[1].transform.rotation = Quaternion.Euler(0f, 0f, 120f);
        LasersStage3[2].transform.rotation = Quaternion.Euler(0f, 0f, 240f);
        LasersStage3[0].SetActive(true);
        LasersStage3[1].SetActive(true);
        LasersStage3[2].SetActive(true);
        yield return new WaitForSeconds(7f);

        LasersStage3[0].SetActive(false);
        LasersStage3[1].SetActive(false);
        LasersStage3[2].SetActive(false);
        doingSpecialAttack = false;
    }

    private void DisableLasers() {
        Laser1.SetActive(false);
        Laser2.SetActive(false);
        doingSpecialAttack = false;
    }

    private void DisableMissileEffects() {
        MissileEffect.SetActive(false);
        MissileEffect2.SetActive(false);
    }

    public void EnablePrioritizeMissile() {
        prioritizeMissiles = true;
    }

    public void DisablePrioritizeMissile() {
        prioritizeMissiles = false;
    }

    public IEnumerator EnableStageTwo() {
        currentStage = -1;
        AttackAnnounce.PlayOneShot(Stage2Announce);
        destructibleScript.SwitchInmune();
        while (doingSpecialAttack) {
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(0.25f);
        LiftOffEffect.SetActive(true);
        Destroy(LiftOffEffect, 3f);
        ThrustEffect.SetActive(true);
        TronsbonkSprite1.GetComponent<Renderer>().material.EnableKeyword("SHAKEUV_ON");
        yield return new WaitForSeconds(1.25f);
        transform.DOMove(new Vector2(transform.position.x - 2, transform.position.y + 4), 1.5f);
        yield return new WaitForSeconds(1.5f);
        BackTrigger.SetActive(false);
        prioritizeMissiles = false;
        destructibleScript.SwitchInmune();
        currentStage = 2;
        StartCoroutine(DoMovement());
        AppearingPlaforms.SetActive(true);
    }

    public IEnumerator EnableStageThree() {
        currentStage = -1;
        destructibleScript.SwitchInmune();
        while (doingSpecialAttack || doingMovement) {
            yield return new WaitForSeconds(0.5f);
        }
        AttackAnnounce.PlayOneShot(Stage2Announce);
        for (int i = 0; i < 12; i++) {
            Vector2 spawnPosition = new Vector2(transform.position.x + Random.Range(-4f, 4f), 
                transform.position.y + Random.Range(-6f, 6f));
            Destroy(Instantiate(FireExplosion, spawnPosition, Quaternion.identity), 3f);
            AttackAnnounce.pitch *= 1.05f;
            AttackAnnounce.PlayOneShot(Stage3Voices[Random.Range(0, Stage3Voices.Length)]);
            yield return new WaitForSeconds(0.4f);
        }
        Destroy(Instantiate(SoulExplosionBig, transform.position, Quaternion.identity), 3f);
        AttackAnnounce.pitch = 0.65f;
        AttackAnnounce.volume = 1f;
        TronsbonkSprite1.SetActive(false);
        ThrustEffect.SetActive(false);
        TronsbonkSprite2.SetActive(true);
        FindObjectOfType<GlobalMusicController>().bossStage3effect();
        yield return new WaitForSeconds(1f);
        AttackAnnounce.PlayOneShot(Stage3VoicePresentation);
        yield return new WaitForSeconds(1f);
        destructibleScript.SwitchInmune();
        currentStage = 3;
    }

    public IEnumerator DefeatStage() {
        currentStage = -1;
        destructibleScript.SwitchInmune();
        while (doingSpecialAttack || doingMovement) {
            yield return new WaitForSeconds(0.25f);
        }
        TronsbonkSprite2.GetComponent<Renderer>().material.SetFloat("_GlitchAmount", 10f);
        for (int i = 0; i < 12; i++) {
            Vector2 spawnPosition = new Vector2(transform.position.x + Random.Range(-4f, 4f),
                transform.position.y + Random.Range(-6f, 6f));
            Destroy(Instantiate(FireExplosion, spawnPosition, Quaternion.identity), 3f);
            AttackAnnounce.pitch = Random.Range(0.5f, 1.7f);
            AttackAnnounce.PlayOneShot(Stage3Voices[Random.Range(0, Stage3Voices.Length)]);
            yield return new WaitForSeconds(0.4f);
        }
        AttackAnnounce.pitch = 1f;
        AttackAnnounce.PlayOneShot(ExplosionLoad);
        Destroy(Instantiate(MegaExplosion, transform.position, Quaternion.identity), 3f);
        yield return new WaitForSeconds(2f);
        GlobalMusicController gmc = FindObjectOfType<GlobalMusicController>();
        gmc.altSource.PlayOneShot(ExplosionExplode);
        yield return new WaitForSeconds(0.1f);
        gmc.WinEffect();
        EnableOnDeath.SetActive(true);
        Destroy(gameObject);
    }

}
