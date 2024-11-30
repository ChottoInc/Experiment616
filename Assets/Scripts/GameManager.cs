using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int levelToLoadAfterFirstLoading { get; set; }


    // SETTINGS

    public float SavedMasterVolume { get; private set; }
    public float SavedMusicVolume { get; private set; }
    public float SavedEffectsVolume { get; private set; }

    private bool isFullscreen;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            //Init();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Init()
    {
        int fullscreen = PlayerPrefs.GetInt(Helper.KEY_FULLSCREEN, 1);
        if(fullscreen == 1)
        {
            Screen.fullScreen = true;

            PlayerPrefs.SetInt(Helper.KEY_FULLSCREEN, 1);
        }
        else
        {
            Screen.fullScreen = false;
        }
    }
}
