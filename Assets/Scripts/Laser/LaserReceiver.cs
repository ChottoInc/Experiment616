using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserReceiver : MonoBehaviour
{
    [SerializeField] Laserable laserable;

    private bool isReceiving;

    private float cooldownResetLaser = 0.15f;
    private float timerResetLaser;

    private bool canMakeSounds;

    private void Start()
    {
        StartCoroutine(CoEnableSounds());
    }

    private void Update()
    {
        if (timerResetLaser <= 0 && isReceiving)
        {
            EnableReceiving(false);
        }
        else
        {
            timerResetLaser -= Time.deltaTime;
        }
    }

    public void CheckLaser()
    {
        timerResetLaser = cooldownResetLaser;
        if (!isReceiving)
        {
            if(canMakeSounds)
                AudioManager.Instance.PlayEffect("LaserReceiver");

            EnableReceiving(true);
        }
    }

    private void EnableReceiving(bool enabled)
    {
        //Debug.Log("enabled received: " + enabled);
        isReceiving = enabled;

        laserable.ReceivedLaser(isReceiving);
    }

    private IEnumerator CoEnableSounds()
    {
        yield return new WaitForSeconds(1.5f);

        canMakeSounds = true;
    }
}
