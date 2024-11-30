using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    [SerializeField] float timePortalTeleportAnimation = 1.5f;


    private int normalKeyCounter;

    public int NormalKeyCounter => normalKeyCounter;

    private Vector3 scaleBeforeTeleport;

    private PlayerMovement playerMovement;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();    
    }

    public void AddKey(Helper.KeyType keyType)
    {
        switch (keyType)
        {
            default:
            case Helper.KeyType.NORMAL: normalKeyCounter++; break;
        }
    }

    public void PortalTeleport(Portal portalReceiver, Transform playerSpawn)
    {
        StartCoroutine(CoPortalTeleport(portalReceiver, playerSpawn));
    }

    private IEnumerator CoPortalTeleport(Portal receiver, Transform playerSpawn)
    {
        // save scale
        scaleBeforeTeleport = transform.localScale;

        // can't move
        playerMovement.CanMove = false;

        transform.DOScale(0f, timePortalTeleportAnimation).SetEase(Ease.InBounce);

        yield return new WaitForSeconds(timePortalTeleportAnimation + 0.05f);

        transform.position = playerSpawn.position;

        // reset scale
        transform.DOScale(scaleBeforeTeleport, 0.5f).SetEase(Ease.OutBounce);

        yield return new WaitForSeconds(0.51f);

        // reset movement
        playerMovement.CanMove = true;
    }
}
