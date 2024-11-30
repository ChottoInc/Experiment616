using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PanelDialogue : MonoBehaviour
{
    [SerializeField] TMP_Text textDialogue;
    [SerializeField] float charEvery = 0.1f;

    [Space(10)]
    [SerializeField] bool autoUpdateDialogue = true;
    [SerializeField] float cooldownNextText = 5f;
    private float timerNextText;

    [SerializeField] GameObject textNext;

    [Space(10)]
    [SerializeField] LevelLoadingManager levelLoadingManager;

    private List<Helper.DialogueSettings> currentList = new List<Helper.DialogueSettings>();
    private int counterCurrentList;

    private string currentText;
    private string updatingText = string.Empty;
    private int indexCurrentText;

    private float timerNextChar;

    private bool needUpdateText;
    private bool isDialogueEnded;

    private void Update()
    {
        if (needUpdateText)
        {
            if (updatingText.Length < currentText.Length)
            {
                if (timerNextChar <= 0)
                {
                    // copy next char
                    updatingText += currentText[indexCurrentText];

                    // update ui
                    textDialogue.text = updatingText;

                    // next char
                    indexCurrentText++;

                    // reset timer
                    timerNextChar = charEvery;
                }
                else
                {
                    timerNextChar -= Time.deltaTime;
                }
            }
            else
            {
                if (!isDialogueEnded)
                    textNext.SetActive(true);
                else
                    textNext.SetActive(false);

                needUpdateText = false;
            }
        }
        else
        {
            // check auto update
            if (autoUpdateDialogue)
            {
                if(timerNextText <= 0)
                {
                    if (!isDialogueEnded)
                    {
                        NextText(currentList[counterCurrentList]);
                    }
                    else
                    {
                        // autoclose
                        levelLoadingManager.FadeOutImage();
                        gameObject.SetActive(false);
                    }

                    timerNextText = cooldownNextText;
                }
                else
                {
                    timerNextText -= Time.deltaTime;
                }
            }
        }
    }

    public void StartDialogue(List<Helper.DialogueSettings> settings)
    {
        gameObject.SetActive(true);

        isDialogueEnded = false;

        if(currentList == null)
            currentList = new List<Helper.DialogueSettings>();
        currentList.Clear();

        currentList.AddRange(settings);

        timerNextText = cooldownNextText;

        counterCurrentList = 0;
        NextText(currentList[counterCurrentList]);
    }

    public void NextText(Helper.DialogueSettings nextSetting)
    {
        if(nextSetting.idCharacter == Helper.ID_NARRATOR)
        {
            currentText = string.Format("{0}", nextSetting.text);
        }
        else
        {
            currentText = string.Format("{0}: {1}", Helper.dictIDToName[nextSetting.idCharacter], nextSetting.text);
        }

        // resets
        updatingText = string.Empty;
        indexCurrentText = 0;
        timerNextChar = 0;

        // next text in list
        counterCurrentList++;

        // has to update ui
        needUpdateText = true;

        // if ended dialogue
        if(counterCurrentList == currentList.Count)
        {
            isDialogueEnded = true;
        }
    }

    public void OnClickDialogue()
    {
        AudioManager.Instance.PlayButtonUIEffect();
        if (updatingText.Length < currentText.Length)
        {
            updatingText = currentText;
            textDialogue.text = currentText;

            if (!isDialogueEnded)
                textNext.SetActive(true);
            else
                textNext.SetActive(false);

            needUpdateText = false;
        }
        else
        {
            if (!isDialogueEnded)
            {
                NextText(currentList[counterCurrentList]);
            }
            else
            {
                // autoclose
                levelLoadingManager.FadeOutImage();
                gameObject.SetActive(false);
            }

            timerNextText = cooldownNextText;
        }  
    }
}
