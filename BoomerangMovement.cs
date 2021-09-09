using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangMovement : MonoBehaviour
{
    public Vector2 targetPosition;
    public Vector2 originPosition;
    public GameObject entityCaster;
    public bool isReturning = false;
    public float TimeToTarget = 0.5f;

    private Vector2 _direction;
    private Tweener tweener;

    void Start()
    {
        originPosition = transform.position;
        _direction = (targetPosition - (Vector2)transform.position).normalized;
        targetPosition = targetPosition + (_direction * 2);
        float distance = Vector2.Distance(targetPosition, transform.position);
        TimeToTarget *= distance;
        tweener = transform.DOMove(targetPosition, TimeToTarget);
    }

    void Update()
    {
        if (!isReturning && Vector2.Distance(targetPosition, transform.position) <= 0.1f) {
            if (entityCaster != null)
                targetPosition = entityCaster.transform.position;
            else
                targetPosition = originPosition;
            isReturning = true;
            tweener = transform.DOMove(targetPosition, TimeToTarget);
        }
        else if (isReturning && Vector2.Distance(transform.position, targetPosition) <= 0.1f) {
            tweener.Complete();
            Destroy(gameObject);
        }
    }
}
