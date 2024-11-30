using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelSettings : MonoBehaviour
{
    [SerializeField] CloseIfTouchOutside closeIfTouchOutside;
    [SerializeField] bool isSettingsTitle;

    [Header("Video")]
    [SerializeField] GameObject panelVideo;
    [SerializeField] Toggle toggleFullscreen;
    [SerializeField] Toggle toggleWindowed;

    [Header("Audio")]
    [SerializeField] GameObject panelAudio;
    [SerializeField] Button buttonAudio;
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider effectsSlider;

    [Header("Controls")]
    [SerializeField] GameObject panelControls;

    private Vector3 startScale;

    private bool isInitialized;

    public const int ID_SETTING_AUDIO = 0;
    public const int ID_SETTING_CONTROLS = 1;
    public const int ID_SETTING_VIDEO = 2;

    private int currentSetting;
    private GameObject currentSettingObj;


    private void Awake()
    {
        if(isSettingsTitle)
            closeIfTouchOutside.OnTouchOutside += OnButtonClose;
    }

    private void OnDestroy()
    {
        if (isSettingsTitle)
            closeIfTouchOutside.OnTouchOutside -= OnButtonClose;
    }

    public void Setup()
    {
        if (isSettingsTitle)
        {
            PanelStackManager.Instance.AddBarrier(closeIfTouchOutside);

            if (!isInitialized)
                InitializeIfNeeded();

            CustomUpdate();

            gameObject.SetActive(true);

            ChangeCurrentPanelSetting(ID_SETTING_AUDIO);
            buttonAudio.Select();

            transform.DOScale(startScale, 0.2f).SetEase(Ease.InOutSine);
        }
        else
        {
            //gameObject.SetActive(true);
            CustomUpdate();

            ChangeCurrentPanelSetting(ID_SETTING_AUDIO);
            buttonAudio.Select();
        }
    }

    private void InitializeIfNeeded()
    {
        gameObject.SetActive(true);

        closeIfTouchOutside.Initialize();

        ResetTransform();

        gameObject.SetActive(false);


        isInitialized = true;
    }

    private void CustomUpdate()
    {
        float masterVolume = PlayerPrefs.GetFloat(Helper.KEY_MASTER_VOLUME);
        float musicVolume = PlayerPrefs.GetFloat(Helper.KEY_MUSIC_VOLUME);
        float effectsVolume = PlayerPrefs.GetFloat(Helper.KEY_EFFECTS_VOLUME);

        masterSlider.SetValueWithoutNotify(masterVolume);
        musicSlider.SetValueWithoutNotify(musicVolume);
        effectsSlider.SetValueWithoutNotify(effectsVolume);

        int fullscreen = PlayerPrefs.GetInt(Helper.KEY_FULLSCREEN);

        if(fullscreen == 1)
        {
            toggleFullscreen.SetIsOnWithoutNotify(true);
        }
        else
        {
            toggleWindowed.SetIsOnWithoutNotify(true);
        }
    }

    private void ResetTransform()
    {
        startScale = transform.localScale;
        transform.localScale = new Vector3(0, 0, 0);
    }

    public void ChangeCurrentPanelSetting(int idSetting)
    {
        AudioManager.Instance.PlayButtonUIEffect();
        if (currentSettingObj != null)
        {
            currentSettingObj.SetActive(false);
        }

        switch(idSetting)
        {
            default:
            case ID_SETTING_AUDIO: currentSettingObj = panelAudio; break;
            case ID_SETTING_CONTROLS: currentSettingObj = panelControls; break;
            case ID_SETTING_VIDEO: currentSettingObj = panelVideo; break;
        }

        currentSetting = idSetting;
        currentSettingObj.SetActive(true);
    }

    #region BUTTONS

    public void OnButtonAudio()
    {
        ChangeCurrentPanelSetting(ID_SETTING_AUDIO);
    }

    public void OnButtonVideo()
    {
        ChangeCurrentPanelSetting(ID_SETTING_VIDEO);
    }

    public void OnButtonControls()
    {
        ChangeCurrentPanelSetting(ID_SETTING_CONTROLS);
    }

    public void OnButtonClose()
    {
        AudioManager.Instance.PlayButtonUIEffect();
        if (isSettingsTitle)
        {
            PanelStackManager.Instance.RemoveBarrier();

            transform.DOScale(new Vector3(0, 0, 0), 0.2f).SetEase(Ease.InOutSine).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        }
    }

    #endregion

    #region AUDIO

    public void OnSliderMaster(float value)
    {
        AudioManager.Instance.SetMasterVolume(value);
        PlayerPrefs.SetFloat(Helper.KEY_MASTER_VOLUME, value);
    }

    public void OnSliderMusic(float value)
    {
        AudioManager.Instance.SetMusicVolume(value);
        PlayerPrefs.SetFloat(Helper.KEY_MUSIC_VOLUME, value);
    }

    public void OnSliderEffects(float value)
    {
        AudioManager.Instance.SetEffectsVolume(value);
        PlayerPrefs.SetFloat(Helper.KEY_EFFECTS_VOLUME, value);
    }

    #endregion

    #region VIDEO

    public void OnToggleFullscreen(bool isOn)
    {
        if (!isOn)
            return;

        AudioManager.Instance.PlayButtonUIEffect();
        PlayerPrefs.SetInt(Helper.KEY_FULLSCREEN, 1);
        Screen.fullScreen = true;
    }

    public void OnToggleWindowed(bool isOn)
    {
        if (!isOn)
            return;

        AudioManager.Instance.PlayButtonUIEffect();
        PlayerPrefs.SetInt(Helper.KEY_FULLSCREEN, 0);
        Screen.fullScreen = false;
    }


    #endregion
}
