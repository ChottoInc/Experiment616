using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PanelPause : MonoBehaviour
{
    [SerializeField] CloseIfTouchOutside closeIfTouchOutside;

    [Space(10)]
    [SerializeField] PanelSettings panelSettings;
    [SerializeField] PanelCloseGame panelCloseGame;

    [Space(10)]
    [SerializeField] PlayerMovement playerMovement;
    private Player player;

    [Space(10)]
    [SerializeField] TMP_Text textKeys;

    private Vector3 startScale;

    private bool isInitialized;

    private bool isPaused;


    private void Awake()
    {
        closeIfTouchOutside.OnTouchOutside += OnButtonClose;

        player = playerMovement.GetComponent<Player>();
    }

    private void OnDestroy()
    {
        closeIfTouchOutside.OnTouchOutside -= OnButtonClose;
    }

    public void CallFromManager()
    {
        if (!isPaused)
        {
            Setup();
        }
        else
        {
            OnButtonClose();
        }
    }

    public void Setup()
    {
        AudioManager.Instance.PlayButtonUIEffect();
        isPaused = true;

        PanelStackManager.Instance.AddBarrier(closeIfTouchOutside);

        if (!isInitialized)
            InitializeIfNeeded();

        CustomUpdate();

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

    private void CustomUpdate()
    {
        playerMovement.CanMove = false;

        textKeys.text = string.Format("{0}", player.NormalKeyCounter);

        panelSettings.Setup();
    }

    public void OnButtonTitleScreen()
    {
        AudioManager.Instance.PlayButtonUIEffect();
        panelCloseGame.Setup(PanelCloseGame.ID_TITLE_SCREEN);
    }

    public void OnButtonQuit()
    {
        AudioManager.Instance.PlayButtonUIEffect();
        panelCloseGame.Setup(PanelCloseGame.ID_QUIT);
    }

    public void OnButtonClose()
    {
        AudioManager.Instance.PlayButtonUIEffect();
        PanelStackManager.Instance.RemoveBarrier();

        transform.DOScale(new Vector3(0, 0, 0), 0.2f).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            gameObject.SetActive(false);

            isPaused = false;
            playerMovement.CanMove = true;
        });
    }
}
