using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PanelStackManager : MonoBehaviour
{
    public static PanelStackManager Instance { get; private set; }

    private List<CloseIfTouchOutside> barriers;

    private int currentBarrier;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        barriers = new List<CloseIfTouchOutside>();
    }

    public void AddBarrier(CloseIfTouchOutside barrier)
    {
        if(barriers != null)
        {
            if(barriers.Count > 0)
            {
                barriers[currentBarrier].enabled = false;

                currentBarrier++;
            }

            barrier.enabled = true;

            barriers.Add(barrier);
        }
    }

    public void RemoveBarrier()
    {
        if (barriers != null)
        {
            if (barriers.Count > 0)
            {
                barriers[currentBarrier].enabled = false;
                barriers.RemoveAt(currentBarrier);

                if(barriers.Count > 0)
                {
                    currentBarrier--;

                    barriers[currentBarrier].enabled = true;
                }
            }
        }
    }
}
