using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class OutroManager : MonoBehaviour
{
    [SerializeField] CanvasGroup textOutro;
    [SerializeField] float timeToFade = 2f;

    public void StartOutro()
    {
        textOutro.gameObject.SetActive(true);
        textOutro.DOFade(1, timeToFade).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            textOutro.alpha = 1f;
            textOutro.blocksRaycasts = true;
            textOutro.interactable = true;
        });

        StartCoroutine(CoFadeOutAndTitle());
    }

    private IEnumerator CoFadeOutAndTitle()
    {
        yield return new WaitForSeconds(timeToFade + 3f);

        textOutro.DOFade(0, timeToFade).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            textOutro.alpha = 0f;
            textOutro.blocksRaycasts = false;
            textOutro.interactable = false;
        });

        yield return new WaitForSeconds(timeToFade + 2f);

        SceneManager.LoadScene("Title");
    }
}
