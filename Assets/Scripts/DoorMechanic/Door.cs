using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Door : MonoBehaviour
{
    [SerializeField] int keyRequired = 1;
    [SerializeField] DoorSwitch doorSwitch;

    [Space(10)]
    [SerializeField] Light2D signLight;
    [SerializeField] float stepLight = 0.5f;
    private bool lightTurningOff = true;



    private bool isOpen;

    public int KeyRequired => keyRequired;
    public bool IsOpen => isOpen;


    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(lightTurningOff)
        {
            signLight.intensity -= stepLight * Time.deltaTime;
            if (signLight.intensity <= 0f)
                lightTurningOff = false;
        }
        else
        {
            signLight.intensity += stepLight * Time.deltaTime;
            if (signLight.intensity >= 3f)
                lightTurningOff = true;
        }
    }

    public void OpenDoor(bool open)
    {
        AudioManager.Instance.PlayEffect("Door");
        isOpen = open;
        doorSwitch.SetOpen(open);

        if (isOpen)
        {
            animator.SetTrigger("Open");
            animator.ResetTrigger("Close");
        }
        else
        {
            animator.SetTrigger("Close");
            animator.ResetTrigger("Open");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out PlayerMovement player))
        {
            if(isOpen)
                player.CanGoNextLevel = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerMovement player))
        {
            if(isOpen)
                player.CanGoNextLevel = true;
        }
    }
}
