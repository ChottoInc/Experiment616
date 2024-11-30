using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MovingPlatform : Laserable
{
    [Header("Movement")]
    [SerializeField] float offsetX;
    [SerializeField] float offsetY;
    [SerializeField] float timeToOffset;

    [Header("Loop")]
    [SerializeField] bool isLoop;

    private Vector2 startPos;
    //private Vector2 endPos;

    private Tween moveTween;

    private void OnDestroy()
    {
        if (moveTween != null)
            moveTween.Kill();
    }

    private void Start()
    {
        startPos = transform.position;
        //endPos = startPos + new Vector2(offsetX, offsetY);
    }

    protected override void Activate()
    {
        if(moveTween == null)
        {
            PlatformMovement();
        }
        else
        {
            moveTween.Play();
        }
    }

    protected override void DeActivate()
    {
        if (moveTween == null)
        {
            // nothing, should not happen
        }
        else
        {
            moveTween.Pause();
        }
    }

    protected override void ActivateOnce()
    {
        PlatformMovement();
    }

    private void PlatformMovement()
    {
        if (offsetX != 0 && offsetY != 0)
        {
            if (isLoop)
            {
                moveTween = transform.DOMove((Vector2)transform.position + new Vector2(offsetX, offsetY), timeToOffset).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
            }
            else
            {
                moveTween = transform.DOMove((Vector2)transform.position + new Vector2(offsetX, offsetY), timeToOffset).SetEase(Ease.InOutSine);
            }
        }
        else if (offsetX != 0)
        {
            if (isLoop)
            {
                moveTween = transform.DOMoveX(transform.position.x + offsetX, timeToOffset).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
            }
            else
            {
                moveTween = transform.DOMoveX(transform.position.x + offsetX, timeToOffset).SetEase(Ease.InOutSine);
            }
        }
        else
        {
            if (isLoop)
            {
                moveTween = transform.DOMoveY(transform.position.y + offsetY, timeToOffset).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
            }
            else
            {
                moveTween = transform.DOMoveY(transform.position.y + offsetY, timeToOffset).SetEase(Ease.InOutSine);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerMovement playerMovement))
        {
            if (playerMovement.IsGrounded())
            {
                playerMovement.transform.SetParent(transform);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerMovement playerMovement))
        {
            playerMovement.transform.SetParent(null);
        }
    }
}
