using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DoorSwitch : MonoBehaviour
{
    [SerializeField] Sprite openSprite;
    [SerializeField] Sprite closeSprite;

    [Space(10)]
    [SerializeField] Light2D m_light;
    
    private Color32 closeLight = new Color32(254, 125, 88, 255);
    private Color32 openLight = new Color32(37, 253, 43, 255);

    private SpriteRenderer spriteRenderer;

    private bool isOpen;

    public bool IsOpen => isOpen;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetOpen(bool open)
    {
        isOpen = open;
        if(isOpen)
        {
            spriteRenderer.sprite = openSprite;
            m_light.color = openLight;
        }
        else
        {
            spriteRenderer.sprite = closeSprite;
            m_light.color = closeLight;
        }
    }
}
