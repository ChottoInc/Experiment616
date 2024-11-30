using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Portal : MonoBehaviour
{
    [SerializeField] float cooldownTeleport;
    private float timerTeleport;

    private bool isInsideTeleport;

    [SerializeField] Portal portalReceiver;

    [SerializeField] Light2D portalLight;

    [SerializeField] Transform playerSpawn;

    private Player player;

    private float startInnerRadiusLight;
    private float startOuterRadiusLight;

    [SerializeField] float cooldownPlayerSent;
    private float timerPlayerSent;
    private bool isPlayerSent;

    private void Awake()
    {
        player = FindFirstObjectByType<Player>();
    }

    private void Start()
    {
        timerTeleport = cooldownTeleport;

        startInnerRadiusLight = portalLight.pointLightInnerAngle;
        startOuterRadiusLight = portalLight.pointLightOuterAngle;
    }

    private void Update()
    {
        if (isInsideTeleport)
        {
            if (!isPlayerSent)
            {
                if (timerTeleport <= 0)
                {
                    Teleport();
                    timerTeleport = cooldownTeleport;
                }
                else
                {
                    timerTeleport -= Time.deltaTime;

                    portalLight.pointLightInnerAngle -= 10f * Time.deltaTime;
                    portalLight.pointLightOuterAngle -= 10f * Time.deltaTime;
                }
            }
        }

        if (isPlayerSent)
        {
            if (timerPlayerSent <= 0)
            {
                isPlayerSent = false;
            }
            else
            {
                timerPlayerSent -= Time.deltaTime;
            }
        }
    }

    private void Teleport()
    {
        player.PortalTeleport(portalReceiver, portalReceiver.playerSpawn);

        portalReceiver.SetPlayerSent();
    }

    private void SetPlayerSent()
    {
        timerPlayerSent = cooldownTeleport;
        isPlayerSent = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out Player player))
        {
            isInsideTeleport = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            isInsideTeleport = false;
            timerTeleport = cooldownTeleport;

            portalLight.pointLightInnerAngle = startInnerRadiusLight;
            portalLight.pointLightOuterAngle = startOuterRadiusLight;
        }
    }
}
