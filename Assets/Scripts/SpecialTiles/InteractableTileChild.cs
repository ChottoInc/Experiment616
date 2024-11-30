using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableTileChild : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [SerializeField] Sprite emptySprite;
    [SerializeField] Sprite solidSprite;
    [SerializeField] Sprite etherealSprite;
    [SerializeField] Sprite reflectiveSprite;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetCurrentState(Helper.SpecialTileState newState)
    {
        switch (newState)
        {
            default:
            case Helper.SpecialTileState.EMPTY: spriteRenderer.sprite = emptySprite; break;
            case Helper.SpecialTileState.SOLID: spriteRenderer.sprite = solidSprite; break;
            case Helper.SpecialTileState.ETHEREAL: spriteRenderer.sprite = etherealSprite; break;
            case Helper.SpecialTileState.REFLECTIVE: spriteRenderer.sprite = reflectiveSprite; break;
        }
    }
}
