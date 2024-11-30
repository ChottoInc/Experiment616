using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class InteractableTile : MonoBehaviour
{
    [SerializeField] Helper.SpecialTileState startingState;
    private Helper.SpecialTileState currentState;

    [Space(10)]
    [SerializeField] GameObject groundObject;

    [Space(10)]
    [SerializeField] bool canReflect;
    [SerializeField] Vector2 reflectToPos;
    [SerializeField] LaserRedirectSettings[] laserRedirectSettings;
    [SerializeField] LaserGun laserGun;
    private bool isReflecting;

    private float cooldownResetLaser = 0.15f;
    private float timerResetLaser;

    [Space(10)]
    [SerializeField] InteractableTileChild[] childrenSprites;

    [Space(10)]
    [SerializeField] Light2D tileLight;

    private Color32 colorEmpty = new Color(173f / 255f, 173f / 255f, 173f / 255f, 255f / 255f);
    private Color32 colorSolid = new Color(170f / 255f, 100f / 255f, 77f / 255f, 255f / 255f);
    private Color32 colorEthereal = new Color(47f / 255f, 229f / 255f, 48f / 255f, 255f / 255f);
    private Color32 colorReflective = new Color(107f / 255f, 97f / 255f, 255f / 208f, 255f / 255f);



    private BoxCollider2D boxCollider2D;


    public Helper.SpecialTileState CurrentState => currentState;
    public Vector2 ReflectToPos => reflectToPos;
    public Vector2 ReceivingLaserFromDir { get; set; }

    private bool canMakeSounds;



    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        tileLight.intensity = 4.5f;

        currentState = startingState;
        CheckState(startingState);

        StartCoroutine(CoEnableSounds());
    }

    private void Update()
    {
        if(timerResetLaser <= 0 && isReflecting)
        {
            EnableReflecting(false);
        }
        else
        {
            timerResetLaser -= Time.deltaTime;
        }
    }

    public void CheckLaser()
    {
        timerResetLaser = cooldownResetLaser;
        if (!isReflecting)
        {
            if(canMakeSounds)
                AudioManager.Instance.PlayEffect("RedirectLaser");

            EnableReflecting(true);
        }
    }

    private void CheckState(Helper.SpecialTileState state)
    {
        switch (state)
        {
            default:
            case Helper.SpecialTileState.EMPTY: SetEmpty(); break;
            case Helper.SpecialTileState.SOLID: SetSolid(); break;
            case Helper.SpecialTileState.ETHEREAL: SetEthereal(); break;
            case Helper.SpecialTileState.REFLECTIVE: SetReflective(); break;
        }

        SetChildrenSprites();
    }

    private void SetChildrenSprites()
    {
        for (int i = 0; i < childrenSprites.Length; i++)
        {
            childrenSprites[i].SetCurrentState(currentState);
        }
    }

    private void SetEmpty()
    {
        boxCollider2D.isTrigger = false;

        gameObject.layer = 7;

        groundObject.layer = 6;
        groundObject.SetActive(true);

        tileLight.color = colorEmpty;
    }

    private void SetSolid()
    {
        boxCollider2D.isTrigger = false;

        gameObject.layer = 7;

        groundObject.layer = 10;
        groundObject.SetActive(true);

        tileLight.color = colorSolid;
    }

    private void SetEthereal()
    {
        boxCollider2D.isTrigger = true;

        gameObject.layer = 11;

        groundObject.layer = 6;
        groundObject.SetActive(false);

        tileLight.color = colorEthereal;
    }

    private void SetReflective()
    {
        boxCollider2D.isTrigger = false;

        gameObject.layer = 7;

        groundObject.layer = 6;
        groundObject.SetActive(true);

        tileLight.color = colorReflective;
    }

    public void SetCurrentState(Helper.SpecialTileState newState)
    {
        currentState = newState;
        CheckState(currentState);
    }

    public AbsorbResult AbsorbState()
    {
        if(currentState != Helper.SpecialTileState.EMPTY)
        {
            AbsorbResult absorbResult = new AbsorbResult();
            absorbResult.isAbsorbed = true;
            absorbResult.state = currentState;

            SetCurrentState(Helper.SpecialTileState.EMPTY);

            return absorbResult;
        }
        else
        {
            AbsorbResult absorbResult = new AbsorbResult();
            absorbResult.isAbsorbed = false;
            absorbResult.state = currentState;

            return absorbResult;
        }
    }

    public bool ReleaseState(Helper.SpecialTileState newState)
    {
        if (currentState != Helper.SpecialTileState.EMPTY)
            return false;

        if (newState == Helper.SpecialTileState.REFLECTIVE && !canReflect)
            return false;

        SetCurrentState(newState);
        for (int i = 0; i < childrenSprites.Length; i++)
        {
            childrenSprites[i].SetCurrentState(currentState);
        }
        return true;
    }

    private void EnableReflecting(bool enabled)
    {
        //Debug.Log("enabled reflect: " + enabled);
        isReflecting = enabled;

        if (isReflecting)
        {
            Vector2 newDir = Vector2.right;
            for (int i = 0; i < laserRedirectSettings.Length; i++)
            {
                if (laserRedirectSettings[i].laserFrom == ReceivingLaserFromDir)
                    newDir = laserRedirectSettings[i].laserTo;
            }
            laserGun.SetNewDirection(newDir);
        }
        
        laserGun.gameObject.SetActive(enabled);
    }


    private IEnumerator CoEnableSounds()
    {
        yield return new WaitForSeconds(1.5f);

        canMakeSounds = true;
    }
}

[System.Serializable]
public class LaserRedirectSettings
{
    public Vector2 laserFrom;
    public Vector2 laserTo;
}

public struct AbsorbResult
{
    public bool isAbsorbed;
    public Helper.SpecialTileState state;
}