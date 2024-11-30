using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SquareLoading : MonoBehaviour
{
    [SerializeField] Vector2 dir;
    [SerializeField] float timeMovement = 0.5f;

    private Vector3 startPos;

    private float timerMove;
    private bool isMoving;

    private Tween moveTween;

    private void OnDestroy()
    {
        if(moveTween != null)
        {
            moveTween.Kill();
        }
    }

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        if(timerMove <= 0 && !isMoving)
        {
            isMoving = true;

            MoveSquare();

            timerMove = timeMovement + 0.5f;
        }
        else if(timerMove > 0 && !isMoving)
        {
            timerMove -= Time.deltaTime;
        }
    }

    private void MoveSquare()
    {
        if(dir.x == 0)
        {
            moveTween = transform.DOMoveY(transform.position.y + dir.y, timeMovement).SetEase(Ease.InOutSine).OnComplete(() =>
            {
                transform.position = startPos;
                isMoving = false;
            });
        }
        else
        {
            moveTween = transform.DOMoveX(transform.position.x + dir.x, timeMovement).SetEase(Ease.InOutSine).OnComplete(() =>
            {
                transform.position = startPos;
                isMoving = false;
            });
        }
    }
}
