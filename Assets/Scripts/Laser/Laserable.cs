using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laserable : MonoBehaviour
{
    [SerializeField] protected bool oneTimeActivation;

    protected bool activated;

    public virtual void ReceivedLaser(bool isReceiving)
    {
        if (isReceiving)
        {
            // only one time activation
            if (oneTimeActivation && !activated)
            {
                activated = true;
                ActivateOnce();
            }
            // always activate
            else if (!oneTimeActivation && !activated)
            {
                activated = true;
                Activate();
            }
        }
        else
        {
            if(!oneTimeActivation && activated)
            {
                activated = false;
                DeActivate();
            }
        }
        
    }

    protected virtual void ActivateOnce()
    {

    }


    protected virtual void Activate()
    {

    }

    protected virtual void DeActivate()
    {

    }
}
