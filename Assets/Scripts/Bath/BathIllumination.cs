using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BathIllumination : MonoBehaviour
{
    [SerializeField] Light2D bathLight;
    [SerializeField] float maxLight = 0.2f;
    [SerializeField] float stepLight = 0.02f;

    [Space(10)]
    [SerializeField] float timeToWaitCountdown = 4f;
    [SerializeField] float timeToFadeIn = 2f;

    private bool isGoingUp;
    private bool isCountdownOver;

    private void Start()
    {
        isGoingUp = true;
    }

    private void Update()
    {
        if (!isCountdownOver)
        {
            if (isGoingUp)
            {
                if (bathLight.intensity < maxLight)
                {
                    bathLight.intensity += stepLight * Time.deltaTime;
                }
                else
                {
                    isGoingUp = false;
                }
            }
            else
            {
                if (bathLight.intensity > 0)
                {
                    bathLight.intensity -= stepLight * Time.deltaTime;
                }
                else
                {
                    isGoingUp = true;
                }
            }
        }
        else
        {
            if (bathLight.intensity > 0)
            {
                bathLight.intensity -= stepLight * Time.deltaTime;
            }
        }
    }

    public void StartCountdown(float timeToFade)
    {
        StartCoroutine(CoStartCountdown(timeToFade));
    }

    private IEnumerator CoStartCountdown(float timeToFade)
    {
        yield return new WaitForSeconds(timeToFade + timeToWaitCountdown);

        isCountdownOver = true;

        yield return new WaitForSeconds(timeToFadeIn);

        FindFirstObjectByType<LevelLoadingManager>().FadeInImageAndOutro();
    }
}
