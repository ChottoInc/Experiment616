using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PanelCloseGame : MonoBehaviour
{
    [SerializeField] CloseIfTouchOutside closeIfTouchOutside;
    [SerializeField] TMP_Text textWindow;

    private Vector3 startScale;

    private bool isInitialized;

    public const int ID_TITLE_SCREEN = 0;
    public const int ID_QUIT = 1;

    private int currentScreen;


    private void Awake()
    {
        closeIfTouchOutside.OnTouchOutside += OnButtonClose;
    }

    private void OnDestroy()
    {
        closeIfTouchOutside.OnTouchOutside -= OnButtonClose;
    }

    public void Setup(int id)
    {
        PanelStackManager.Instance.AddBarrier(closeIfTouchOutside);

        if (!isInitialized)
            InitializeIfNeeded();

        switch (id)
        {
            default:
            case ID_TITLE_SCREEN:
                textWindow.text = "The progress in this level will be erased, continue to the title screen anyway?";
                break;

            case ID_QUIT:
                textWindow.text = "The progress in this level will be erased, close anyway?"; 
                break;
        }

        currentScreen = id;

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
        switch (currentScreen)
        {
            default:
            case ID_TITLE_SCREEN:
                SceneManager.LoadScene("Title");
                break;

            case ID_QUIT:
                Application.Quit();
                break;
        }
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
