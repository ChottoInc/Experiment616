using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasTitle : MonoBehaviour
{
    [SerializeField] PanelFoundSave panelFoundSave;
    [SerializeField] PanelNoSave panelNoSave;
    [SerializeField] PanelSettings panelSettings;
    [SerializeField] PanelCredits panelCredits;

    public void OnButtonNew()
    {
        AudioManager.Instance.PlayButtonUIEffect();
        int levelToPlay = PlayerPrefs.GetInt(Helper.KEY_LEVEL_TO_PLAY, -1);
        if (levelToPlay > -1)
        {
            panelFoundSave.Setup();
        }
        else
        {
            StartNewGame();
        }
    }

    public void ConfirmEraseSave()
    {
        StartNewGame();
    }

    private void StartNewGame()
    {
        GameManager.Instance.levelToLoadAfterFirstLoading = 0;
        SceneManager.LoadScene("LoadingScene");
    }

    public void OnButtonContinue()
    {
        AudioManager.Instance.PlayButtonUIEffect();
        int levelToPlay = PlayerPrefs.GetInt(Helper.KEY_LEVEL_TO_PLAY, -1);
        if (levelToPlay < 0)
        {
            panelNoSave.Setup();
        }
        else
        {
            GameManager.Instance.levelToLoadAfterFirstLoading = levelToPlay;
            SceneManager.LoadScene("LoadingScene");
        }
    }

    public void OnButtonSettings()
    {
        panelSettings.Setup();
        AudioManager.Instance.PlayButtonUIEffect();
    }

    public void OnButtonCredits()
    {
        panelCredits.Setup();
        AudioManager.Instance.PlayButtonUIEffect();
    }

    public void OnButtonQuit()
    {
        Application.Quit();
    }

    public void OnButtonResetKeys()
    {
        PlayerPrefs.DeleteAll();
    }
}
