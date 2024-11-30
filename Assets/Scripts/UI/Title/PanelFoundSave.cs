using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelFoundSave : MonoBehaviour
{
    [SerializeField] CloseIfTouchOutside closeIfTouchOutside;
    [SerializeField] CanvasTitle canvasTitle;

    private Vector3 startScale;

    private bool isInitialized;


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

        transform.DOScale(startScale, 0.2f).SetEase(Ease.InOutSine);
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

    public void OnButtonYes()
    {
        AudioManager.Instance.PlayButtonUIEffect();
        canvasTitle.ConfirmEraseSave();
    }

    public void OnButtonClose()
    {
        AudioManager.Instance.PlayButtonUIEffect();
        PanelStackManager.Instance.RemoveBarrier();

        transform.DOScale(new Vector3(0, 0, 0), 0.2f).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
