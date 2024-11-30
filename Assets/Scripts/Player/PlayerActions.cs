using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerActions : MonoBehaviour
{
    [SerializeField] LayerMask interactableTileLayer;

    [Space(10)]
    [SerializeField] Light2D playerLight;
    [SerializeField] Light2D releaseLight;

    private PlayerMovement playerMovement;

    private Helper.SpecialTileState currentPower;

    private Color32 colorEmpty = new Color(173f/255f, 173f/255f, 173f/255f , 255f/255f);
    private Color32 colorSolid = new Color(227f/255f, 21f/255f, 21f/255f , 255f/255f);
    private Color32 colorEthereal = new Color(65f/255f, 230f/255f, 62f/255f , 255f/255f);
    private Color32 colorReflective = new Color(0f/255f, 2f/255f, 255f/255f, 255f/255f);


    private void OnDestroy()
    {
        playerMovement.OnAbsorb -= OnAbsorb;
        playerMovement.OnRelease -= OnRelease;
    }

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();

        playerMovement.OnAbsorb += OnAbsorb;
        playerMovement.OnRelease += OnRelease;

        currentPower = Helper.SpecialTileState.EMPTY;
    }

    private void Start()
    {
        ChangeLightColor();
    }

    private void OnAbsorb()
    {
        if (currentPower != Helper.SpecialTileState.EMPTY)
            return;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D collider = Physics2D.OverlapCircle(mousePos, 1f, interactableTileLayer);

        if(collider != null)
        {
            if(collider.TryGetComponent(out InteractableTile collidedTile))
            {
                AbsorbResult absorbResult = collidedTile.AbsorbState();

                if (absorbResult.isAbsorbed)
                {
                    currentPower = absorbResult.state;
                    Debug.Log("current absorbed: " + currentPower.ToString());

                    ChangeLightColor();
                }
                else
                {
                    Debug.Log("tile empty");
                }
            }
        }
    }

    private void OnRelease()
    {
        if (currentPower == Helper.SpecialTileState.EMPTY)
            return;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D collider = Physics2D.OverlapCircle(mousePos, 1.5f, interactableTileLayer);

        if (collider != null)
        {
            if (collider.TryGetComponent(out InteractableTile collidedTile))
            {
                if (collidedTile.ReleaseState(currentPower))
                {
                    Debug.Log("current released: " + currentPower.ToString());
                    currentPower = Helper.SpecialTileState.EMPTY;
                    ChangeLightColor();
                }
            }
        }
    }

    private void ChangeLightColor()
    {
        switch (currentPower)
        {
            default:
            case Helper.SpecialTileState.EMPTY:
                Debug.Log("Set empty color");
                SetAllLighColors(colorEmpty);
                break;

            case Helper.SpecialTileState.SOLID:
                SetAllLighColors(colorSolid);
                break;

            case Helper.SpecialTileState.ETHEREAL:
                SetAllLighColors(colorEthereal);
                break;

            case Helper.SpecialTileState.REFLECTIVE:
                SetAllLighColors(colorReflective);
                break;
        }
    }

    private void SetAllLighColors(Color32 newColor)
    {
        playerLight.color = newColor;
        releaseLight.color = newColor;
    }
}
