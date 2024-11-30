using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelCredits : MonoBehaviour
{
    [SerializeField] CloseIfTouchOutside closeIfTouchOutside;
    [SerializeField] ScrollRect scrollCredits;
    [SerializeField] float scrollSpeed = 0.05f;

    [Space(10)]
    [SerializeField] CanvasGroup canvasText;

    private Vector3 startScale;

    private bool isInitialized;

    private bool rollCredits;


    private void Awake()
    {
        closeIfTouchOutside.OnTouchOutside += OnButtonClose;
    }

    private void OnDestroy()
    {
        closeIfTouchOutside.OnTouchOutside -= OnButtonClose;
    }

    public void Setup()
    {
        PanelStackManager.Instance.AddBarrier(closeIfTouchOutside);

        if (!isInitialized)
            InitializeIfNeeded();

        gameObject.SetActive(true);

        StartCoroutine(StartScrollCredits());

        transform.DOScale(startScale, 0.2f).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            scrollCredits.verticalNormalizedPosition = 1f;

            canvasText.DOFade(1f, 1f).SetEase(Ease.InOutSine).OnComplete(() =>
            {
                canvasText.alpha = 1f;
                canvasText.blocksRaycasts = true;
                canvasText.interactable = true;
            });
        });

        
    }

    private void InitializeIfNeeded()
    {
        gameObject.SetActive(true);

        closeIfTouchOutside.Initialize();

        ResetTransform();

        gameObject.SetActive(false);

        isInitialized = true;
    }

    private void ResetTransform()
    {
        startScale = transform.localScale;
        transform.localScale = new Vector3(0, 0, 0);
    }

    private void FixedUpdate()
    {
        if (rollCredits && scrollCredits.verticalNormalizedPosition > 0)
        {
            scrollCredits.verticalNormalizedPosition -= scrollSpeed * Time.fixedDeltaTime;
        }
    }

    public void OnButtonClose()
    {
        AudioManager.Instance.PlayButtonUIEffect();
        PanelStackManager.Instance.RemoveBarrier();

        transform.DOScale(new Vector3(0, 0, 0), 0.2f).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            gameObject.SetActive(false);
            rollCredits = false;

            canvasText.alpha = 0f;
            canvasText.blocksRaycasts = false;
            canvasText.interactable = false;

            StopAllCoroutines();
        });
    }

    private IEnumerator StartScrollCredits()
    {
        yield return new WaitForSeconds(2f);

        rollCredits = true;
    }
}
