using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : MonoBehaviour
{
    [SerializeField] float defDistanceRay = 100f;
    [SerializeField] LineRenderer lineRenderer;

    [Space(10)]
    [SerializeField] Transform raycastPointUp;
    [SerializeField] Transform raycastPointRight;
    [SerializeField] Transform raycastPointDown;
    [SerializeField] Transform raycastPointLeft;

    [Space(10)]
    [SerializeField] Transform firePoint;
    [SerializeField] Vector2 startDirection;
    [SerializeField] LayerMask stopMask;

    private Vector2 currentDirection;
    private bool changedDirection;

    private void Start()
    {
        if(!changedDirection)
            currentDirection = startDirection;
    }

    private void Update()
    {
        ShootLaser();
    }

    private void ShootLaser()
    {
        if(Physics2D.Raycast(transform.position, currentDirection))
        {
            Transform raycastPoint;
            if(currentDirection == Vector2.up)
            {
                raycastPoint = raycastPointUp;
            }
            else if(currentDirection == Vector2.right)
            {
                raycastPoint = raycastPointRight;
            }
            else if(currentDirection == Vector2.down)
            {
                raycastPoint = raycastPointDown;
            }
            else
            {
                raycastPoint = raycastPointLeft;
            }
            
            RaycastHit2D _hit = Physics2D.Raycast(raycastPoint.position, currentDirection, Mathf.Infinity, stopMask);

            if(_hit)
            {
                if (_hit.transform.TryGetComponent(out InteractableTile tile))
                {
                    if (tile.CurrentState == Helper.SpecialTileState.EMPTY ||
                        tile.CurrentState == Helper.SpecialTileState.SOLID)
                    {
                        //Debug.Log("empty or solid tile");
                        Draw2DRay(firePoint.position, _hit.point);
                    }
                    else if (tile.CurrentState == Helper.SpecialTileState.REFLECTIVE)
                    {
                        // check for reflectiveness
                        tile.ReceivingLaserFromDir = currentDirection;
                        tile.CheckLaser();
                        Draw2DRay(firePoint.position, tile.transform.position);
                        //Debug.Log("reflective tile");
                    }
                    else
                    {
                        //Debug.Log("eth tile");
                        Draw2DRay(firePoint.position, (Vector2)transform.position + currentDirection * defDistanceRay);
                    }
                }
                else if(_hit.transform.TryGetComponent(out LaserReceiver receiver))
                {
                    //Debug.Log("Hiting receiver");
                    receiver.CheckLaser();
                    Draw2DRay(firePoint.position, receiver.transform.position);
                }
                else
                {
                    //Debug.Log("not a tile");
                    Draw2DRay(firePoint.position, _hit.point);
                }
            }
            else
            {
                //Debug.Log("no hit");
                Draw2DRay(firePoint.position, (Vector2)transform.position + currentDirection * defDistanceRay);
            }
        }
        else
        {
            //Debug.Log("boh");
            Draw2DRay(firePoint.position, (Vector2) transform.position + currentDirection * defDistanceRay);
        }
    }

    private void Draw2DRay(Vector2 startPos, Vector2 endPos)
    {
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);
    }

    public void SetNewDirection(Vector2 newDir)
    {
        currentDirection = newDir;
        changedDirection = true;
    }
}
