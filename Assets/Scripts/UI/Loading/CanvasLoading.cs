using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasLoading : MonoBehaviour
{
    [SerializeField] float cooldownNextScene = 2f;

    [Space(10)]
    [SerializeField] TMP_Text textLoading;
    [SerializeField] float cooldownDots = 0.3f;
    private float timerDots;
    private string startTextLoading;
    private int countDots;

    [Space(10)]
    [SerializeField] CanvasGroup blackImage;
    [SerializeField] float timeToFade = 2f;
    [SerializeField] float offsetAfterFade = 1f;

    private void Start()
    {
        startTextLoading = textLoading.text;

        StartCoroutine(NextScene());
    }

    private void Update()
    {
        if(timerDots <= 0)
        {
            if(countDots < 3)
            {
                textLoading.text += ".";
                countDots++;
            }
            else
            {
                textLoading.text = startTextLoading;
                countDots = 0;
            }

            timerDots = cooldownDots;
        }
        else
        {
            timerDots -= Time.deltaTime;
        }
    }

    private IEnumerator NextScene()
    {
        yield return new WaitForSeconds(cooldownNextScene);

        Fade();

        yield return new WaitForSeconds(timeToFade + offsetAfterFade);

        string nextScene = Helper.dictIDToLevelName[GameManager.Instance.levelToLoadAfterFirstLoading];
        SceneManager.LoadScene(nextScene);
    }

    private void Fade()
    {
        blackImage.gameObject.SetActive(true);

        blackImage.alpha = 0f;
        blackImage.blocksRaycasts = true;
        blackImage.interactable = true;

        blackImage.DOFade(1, timeToFade).SetEase(Ease.InOutSine);
    }
}
