using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseIfTouchOutside : MonoBehaviour
{
    [SerializeField] RectTransform areaTransform;
    [SerializeField] bool isCanvasCamera;
    [SerializeField] bool isLateSet;

    private Vector3[] corners;
    private float minX, maxX;
    private float minY, maxY;

    private bool isInitialied;

    private bool isDragging;

    public event Action OnTouchOutside;

    private void Start()
    {
        if (isLateSet)
            return;

        Initialize();
    }

    public void Initialize()
    {
        corners = new Vector3[4];
        areaTransform.GetWorldCorners(corners);

        if (isCanvasCamera)
            corners[0] = Camera.main.WorldToScreenPoint(corners[0]);

        minX = corners[0].x;
        maxX = corners[0].x;
        minY = corners[0].y;
        maxY = corners[0].y;

        for (int i = 1; i < corners.Length; i++)
        {
            if (isCanvasCamera)
                corners[i] = Camera.main.WorldToScreenPoint(corners[i]);

            if (corners[i].x < minX)
                minX = corners[i].x;
            if (corners[i].x > maxX)
                maxX = corners[i].x;
            if (corners[i].y < minY)
                minY = corners[i].y;
            if (corners[i].y > maxY)
                maxY = corners[i].y;
        }
        /*
        for (int j = 0; j < corners.Length; j++)
        {
            Debug.Log("" + corners[j].ToString() + ":");
        }
        */
        isInitialied = true;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && isInitialied)
        {
            Vector2 mousePos = Input.mousePosition;

            if (mousePos.x >= minX &&
                mousePos.x <= maxX &&
                mousePos.y >= minY &&
                mousePos.y <= maxY)
            {
                //Debug.Log("Inside");
            }
            else
            {
                OnTouchOutside?.Invoke();
                //gameObject.SetActive(false);
                //Debug.Log("Close");
            }
        }
    }

    public void PrintCorners()
    {
        Debug.Log("Min X: " + minX + "Max X: " + maxX + "Min Y: " + minY + "Max Y: " + maxY);
    }
}
