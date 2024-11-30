using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorPlayerSensor : MonoBehaviour
{
    private Door attachedDoor;

    private void Awake()
    {
        attachedDoor = GetComponentInParent<Door>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out Player player))
        {
            if(player.NormalKeyCounter == attachedDoor.KeyRequired)
            {
                attachedDoor.OpenDoor(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            if (attachedDoor.IsOpen)
            {
                attachedDoor.OpenDoor(false);
            }
        }
    }
}
