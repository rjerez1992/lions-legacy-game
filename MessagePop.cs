using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessagePop : MonoBehaviour
{
    public GameObject Message;
    private Tweener t;
    public AudioSource asrc;
    public AudioClip click;

    void Start()
    {
        Message.SetActive(false);
    }

    void Update()
    {
        
    }

    public void PopMessage() {
        asrc.PlayOneShot(click);
        Message.SetActive(true);
        Message.transform.localScale = Vector2.zero;
        if (t != null)
            t.Complete();
        t = Message.transform.DOScale(Vector2.one, 0.6f);
    }

    public void UnPopMessage() {
        asrc.PlayOneShot(click);
        if (t != null)
            t.Complete();
        t = Message.transform.DOScale(Vector2.zero, 0.6f);
        t.onComplete += () => {
            Message.SetActive(false);
        };
    }
}
