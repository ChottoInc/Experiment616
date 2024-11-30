using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

public class AudioManager : MonoBehaviour
{
#pragma warning disable 0618

    public static AudioManager Instance { get; private set; }

    public bool IsInitialized { get; private set; }


    [SerializeField] Sound[] musics = null;
    [SerializeField] Sound[] effects = null;

    [Space(10)]
    [SerializeField] AudioMixer mixer = null;

    private int currentPlayingIndex;

    private int currentPlayingEffect;


    [SerializeField] float reducingTime;

    // change music variable
    private bool isLoweringMusic;
    private string nextMusic;

    private string titleMusic = "Title";
    private string tutorial1Music = "Tutorial1";
    private string level1Music = "Level1";
    private string level5Music = "Level5";
    private string level8Music = "Level8";

    private bool playedOnce;
    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);

            Init();
        }
        else
        {
            Destroy(this);
        }
    }

    private void Init()
    {
        float masterVolume = PlayerPrefs.GetFloat(Helper.KEY_MASTER_VOLUME);
        float musicVolume = PlayerPrefs.GetFloat(Helper.KEY_MUSIC_VOLUME);
        float effectsVolume = PlayerPrefs.GetFloat(Helper.KEY_EFFECTS_VOLUME);

        if (masterVolume == 0f)
        {
            masterVolume = 1f;
            PlayerPrefs.SetFloat(Helper.KEY_MASTER_VOLUME, masterVolume);
        }
       

        if (musicVolume == 0f)
        {
            musicVolume = 1f;
            PlayerPrefs.SetFloat(Helper.KEY_MUSIC_VOLUME, musicVolume);
        }
        

        if (effectsVolume == 0f)
        {
            effectsVolume = 1f;
            PlayerPrefs.SetFloat(Helper.KEY_EFFECTS_VOLUME, effectsVolume);
        }

        /*
        Debug.Log("master: " + masterVolume);
        Debug.Log("music: " + musicVolume);
        Debug.Log("effects: " + effectsVolume);
        */

        StartCoroutine(SetMixer(masterVolume, musicVolume, effectsVolume));

        InitializeUnityAudios();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private IEnumerator SetMixer(float masterVol, float musicVol, float effectsVol)
    {
        yield return new WaitForSeconds(0.5f);

        SetMasterVolume(masterVol);
        SetMusicVolume(musicVol);
        SetEffectsVolume(effectsVol);

        // todo play music here
        PlayMusic(titleMusic);
        playedOnce = true;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (playedOnce)
        {
            string tempNextMusic;
            switch (scene.name)
            {
                default:
                case "Title": tempNextMusic = titleMusic; break;
                case "Tutorial1":
                case "Tutorial2":
                case "Tutorial3": tempNextMusic = tutorial1Music; break;
                case "Level1": 
                case "Level2": 
                case "Level3": 
                case "Level4": tempNextMusic = level1Music; break;
                case "Level5": 
                case "Level6": 
                case "Level7": tempNextMusic = level5Music; break;
                case "Level8": tempNextMusic = level8Music; break;
            }

            if(GetMusicPlayingName() != tempNextMusic) 
            {
                ChangeMusic(tempNextMusic);
            }
        }
    }

    private void Start()
    {
        IsInitialized = true;
    }

    private void Update()
    {
        if (isLoweringMusic)
        {
            if (musics[currentPlayingIndex].source.volume > 0)
                musics[currentPlayingIndex].source.volume -= Time.deltaTime / reducingTime;
            else
            {
                isLoweringMusic = false;
                musics[currentPlayingIndex].source.Stop();

                PlayMusic(nextMusic);
            }
        }
    }

    #region TEST METHODS

    private int countMusic = 0;
    public void PlayTestNextMusic()
    {
        if(countMusic < musics.Length)
        {
            ChangeMusic(musics[countMusic].name);
            countMusic++;
        }
        
    }

    #endregion

    #region INITIALIZERS

    private void InitializeUnityAudios()
    {
        foreach (var sound in musics)
        {
            sound.source = gameObject.AddComponent<AudioSource>();

            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
            sound.source.outputAudioMixerGroup = sound.group;
        }

        foreach (var sound in effects)
        {
            sound.source = gameObject.AddComponent<AudioSource>();

            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
            sound.source.outputAudioMixerGroup = sound.group;
        }
    }

    #endregion

    public void PlayEffect(string name)
    {
        //Sound s = Array.Find(effects, sound => sound.name == name);
        Sound s = null;
        int index = -1;

        for (int i = 0; i < effects.Length; i++)
        {
            if (effects[i].name == name)
            {
                s = effects[i];
                index = i;
            }
        }

        if (s == null)
            return;

        currentPlayingEffect = index;

        s.source.Play();
    }

    public void PlayMusic(string name)
    {
        Sound s = null;
        int index = -1;

        for (int i = 0; i < musics.Length; i++)
        {
            if (musics[i].name == name)
            {
                s = musics[i];
                index = i;
            }
        }

        if (s == null)
            return;

        currentPlayingIndex = index;
        s.source.volume = s.volume;
        s.source.Play();
    }
    
    public void StopMusic()
    {
        musics[currentPlayingIndex].source.Stop();
    }

    public void StopEffect(int index)
    {
        effects[index].source.Stop();
    }

    public void ResumeMusic()
    {
        musics[currentPlayingIndex].source.Play();
    }

    public void ChangeMusicVolume(float value)
    {
        musics[currentPlayingIndex].source.volume = value;
    }

    public string GetMusicPlayingName()
    {
        return musics[currentPlayingIndex].name;
    }

    public void ChangeMusic(string name, bool first = false)
    {
        isLoweringMusic = true;
        nextMusic = name;
    }

    public string GetRandomMusicName(bool differentFromCurrent = true)
    {
        int rand;
        if (differentFromCurrent)
        {
            do
            {
                rand = UnityEngine.Random.Range(0, musics.Length);
            } while (rand == currentPlayingIndex);
        }
        else
        {
            rand = UnityEngine.Random.Range(0, musics.Length);
        }

        return musics[rand].name;
    }

    public void PlayButtonUIEffect()
    {
        PlayEffect("UIClick");
    }

    public void StartPlayerEffectDelayed(float timer, string name)
    {
        StartCoroutine(PlayEffectDelayed(timer, name));
    }

    private IEnumerator PlayEffectDelayed(float timer, string name)
    {
        yield return new WaitForSeconds(timer);

        PlayEffect(name);
    }


    public void SetMasterVolume(float volume)
    {
        mixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
    }

    public void SetMusicVolume(float volume)
    {
        mixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
    }

    public void SetEffectsVolume(float volume)
    {
        mixer.SetFloat("EffectsVolume", Mathf.Log10(volume) * 20);
    }


#pragma warning restore 0618
}