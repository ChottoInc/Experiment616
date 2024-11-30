using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class LevelLoadingManager : MonoBehaviour
{
    [Header("Save Settings")]
    [SerializeField] int levelID;

    [Header("Fade Settings")]
    [SerializeField] CanvasGroup blackImage;
    [SerializeField] float timeToFade = 1.5f;

    [Header("Dialogue Settings")]
    [SerializeField] PanelDialogue panelDialogue;
    [SerializeField] bool dialogueAtStart;
    [SerializeField] int idDialogueSettings;

    [Header("Player Settings")]
    [SerializeField] PlayerMovement playerMovement;

    [Header("Loading Settings")]
    [SerializeField] string nextLevelName;


    private PauseManager pauseManager;

    private void Awake()
    {
        pauseManager = FindFirstObjectByType<PauseManager>();
    }


    private void Start()
    {
        // save checkpoint playerprefs
        PlayerPrefs.SetInt(Helper.KEY_LEVEL_TO_PLAY, levelID);

        // default player can't move
        // black image covering
        playerMovement.CanMove = false;

        blackImage.gameObject.SetActive(true);
        blackImage.alpha = 1;
        blackImage.blocksRaycasts = true;
        blackImage.interactable = true;


        if (dialogueAtStart)
        {
            if (SceneManager.GetActiveScene().name == "Level8")
            {
                pauseManager.CanPause = false;
            }

            panelDialogue.StartDialogue(Helper.dictIDToDialogueSettings[idDialogueSettings]);
        }
        else
        {
            FadeOutImage();
        }
    }

    public void FadeOutImage()
    {
        blackImage.DOFade(0, timeToFade).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            playerMovement.CanMove = true;

            blackImage.alpha = 0f;
            blackImage.blocksRaycasts = false;
            blackImage.interactable = false;
            blackImage.gameObject.SetActive(false);

            if (SceneManager.GetActiveScene().name != "Level8")
            {
                pauseManager.CanPause = true;
            }
        });

        if(SceneManager.GetActiveScene().name == "Level8")
        {
            // new fade in after some time
            FindFirstObjectByType<BathIllumination>().StartCountdown(timeToFade);
        }
    }

    public void FadeInImage()
    {
        pauseManager.CanPause = false;
        playerMovement.CanMove = false;
        blackImage.gameObject.SetActive(true);

        blackImage.DOFade(1, timeToFade).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            playerMovement.CanMove = false;

            blackImage.alpha = 1f;
            blackImage.blocksRaycasts = true;
            blackImage.interactable = true;
        });
    }

    public void FadeInImageAndOutro()
    {
        playerMovement.CanMove = false;
        blackImage.gameObject.SetActive(true);

        blackImage.DOFade(1, timeToFade).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            playerMovement.CanMove = false;

            blackImage.alpha = 1f;
            blackImage.blocksRaycasts = true;
            blackImage.interactable = true;

            FindFirstObjectByType<OutroManager>().StartOutro();
        });
    }

    public void LoadNextLevel()
    {
        FadeInImage();
        StartCoroutine(CoNextLevel());
    }

    private IEnumerator CoNextLevel()
    {
        yield return new WaitForSeconds(timeToFade + 0.5f);

        SceneManager.LoadScene(nextLevelName);
    }
}
