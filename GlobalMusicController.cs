using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class GlobalMusicController : MonoBehaviour
{
    public AudioSource source;
    public AudioSource altSource;

    public AudioClip backgroundMusic;
    public AudioClip defeatMusic;
    public AudioClip timewarpStart;
    public AudioClip timewarpEnd;
    public AudioClip newEnemy;
    public AudioClip WinMusic;
    public PostProcessVolume ppv;

    public PostProcessProfile normalProfile;
    public PostProcessProfile deathProfile;
    public PostProcessProfile timewarpProfile;
    public PostProcessProfile bossStage3ppp;
    public PostProcessProfile victoryProfile;

    public GameObject CanvasUI;
    public GameObject PresentationUI;
    public GameObject PresentationUIForeground;
    public Text Name;
    public Text Flavour;
    public Text Flavour2;
    public Text Flavour3;
    public Image Background;

    private PostProcessProfile previous;

    void Start()
    {
        source.loop = true;
        source.clip = backgroundMusic;
        source.Play();
        CanvasUI.SetActive(true);
    }

    void Update()
    {
        
    }

    public void PlayDefeat() {
        source.Stop();
        source.clip = defeatMusic;
        source.Play();
        ppv.profile = deathProfile;
    }

    public void PlayTimeWarp(bool start) {
        if (start)
            altSource.PlayOneShot(timewarpStart);
        else
            altSource.PlayOneShot(timewarpEnd);
    }

    public void TimeWarpEffect(bool state) {
        if (state) {
            previous = ppv.profile;
            ppv.profile = timewarpProfile;
        }
        else
            ppv.profile = previous;
    }

    public IEnumerator RunPresentation(GameObject target, string name, string flav1, string flav2, string flav3, Color nameColor, Color flavColor, Color backgroundColor, bool isBig = false) {
        source.Pause();
        altSource.PlayOneShot(newEnemy);
        Time.timeScale = 0f;
        Camera cam = Camera.main;
        Vector3 initialPosition = cam.transform.position;
        float initialSize = cam.orthographicSize;
        Vector3 targetPosition = target.transform.position;
        float targetSize = 1f;
        AudioSource presentationAudio = PresentationUI.GetComponent<AudioSource>();
        Name.gameObject.SetActive(false);
        Flavour.gameObject.SetActive(false);
        Flavour2.gameObject.SetActive(false);
        Flavour3.gameObject.SetActive(false);

        Name.text = name;
        Name.color = nameColor;
        Flavour.text = flav1;
        Flavour.color = flavColor;
        Flavour2.text = flav2;
        Flavour2.color = flavColor;
        Flavour3.text = flav3;
        Flavour3.color = flavColor;
        Background.color = backgroundColor;

        //NOTE: Disables z movement of camera
        targetPosition.x -= 0.95f;
        targetPosition.y += 0.9f;
        targetPosition.z = initialPosition.z;
        if (isBig) {
            targetSize = 4f;
            targetPosition.x -= 2.25f;
            targetPosition.y += 2.25f;
        }

        Tweener t = cam.transform.DOMove(targetPosition, 1f);
        t.SetUpdate(true);
        t = cam.DOOrthoSize(targetSize, 1f);
        t.SetUpdate(true);
        t = cam.transform.DORotateQuaternion(Quaternion.Euler(0f, 0f, -7.5f), 1f);
        t.SetUpdate(true);
        yield return new WaitForSecondsRealtime(1f);
        PresentationUI.SetActive(true);
        //presentationAudio.Play();
        yield return new WaitForSecondsRealtime(0.3f);
        PresentationUIForeground.SetActive(true);
        Name.gameObject.SetActive(true);
        presentationAudio.Play();
        yield return new WaitForSecondsRealtime(0.5f);
        Flavour.gameObject.SetActive(true);
        presentationAudio.Play();
        yield return new WaitForSecondsRealtime(0.4f);
        Flavour2.gameObject.SetActive(true);
        presentationAudio.Play();
        yield return new WaitForSecondsRealtime(0.4f);
        Flavour3.gameObject.SetActive(true);
        presentationAudio.Play();
        yield return new WaitForSecondsRealtime(2f);
        PresentationUI.SetActive(false);
        PresentationUIForeground.SetActive(false);

        t = cam.transform.DOMove(initialPosition, 0.7f);
        t.SetUpdate(true);
        t = cam.DOOrthoSize(initialSize, 0.7f);
        t.SetUpdate(true);
        t = cam.transform.DORotateQuaternion(Quaternion.Euler(0f, 0f, 0f), 0.7f);
        t.SetUpdate(true);
        yield return new WaitForSecondsRealtime(0.7f);
        Time.timeScale = 1f;
        source.UnPause();
    }

    public void bossStage3effect() {
        ppv.profile = bossStage3ppp;
    }

    public void WinEffect() {
        ppv.profile = victoryProfile;
        source.clip = WinMusic;
        source.Play();
        //Maybe some effect?
        //Maybe show the allies and their messages?
    }
}
