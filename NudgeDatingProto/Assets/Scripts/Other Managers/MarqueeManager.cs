using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarqueeManager : MonoBehaviour
{
    [SerializeField] private CursorIconManager cursorManager;
    [SerializeField] private TrackerVisual trackerManager;
    [SerializeField] private Canvas parentCanvas;
    [SerializeField] private GameObject fill;
    [SerializeField] private GameObject outline;
    [SerializeField] private int squarePixelSelectionCutOff;
    private RouterManager routerManager;
    private RectTransform rectTransform;
    private Vector2 firstMousePosition;
    private Vector2 currentMousePosition;
    private bool isSelecting = false;
    private int totalAreaSelected = 0;

    void Start()
    {
        routerManager = RouterManager.instance;
        rectTransform = GetComponent<RectTransform>();
        isSelecting = false;
        fill.SetActive(false);
        outline.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && cursorManager.currentCursorState == 0)
        {
            // On the first click, store the first mouse position
            firstMousePosition = Input.mousePosition / parentCanvas.scaleFactor;
            fill.SetActive(true);
            outline.SetActive(true);
            isSelecting = true;
        }

        if (isSelecting)
        {
            // Update the current mouse position
            currentMousePosition = Input.mousePosition / parentCanvas.scaleFactor;

            // Calculate the position and size of the rectangle
            Vector2 center = (firstMousePosition + currentMousePosition) * 0.5f;
            Vector2 size = new Vector2(Mathf.Abs(currentMousePosition.x - firstMousePosition.x), Mathf.Abs(currentMousePosition.y - firstMousePosition.y));

            // Update the RectTransform's size and position
            rectTransform.sizeDelta = size;
            rectTransform.anchoredPosition = center;

            if (Input.GetMouseButtonUp(0))
            {
                int selectionArea = Mathf.RoundToInt(size.x * size.y);
                if(routerManager.loadedSegment == RouterManager.ExperienceSegment.Performance)
                {
                    totalAreaSelected += selectionArea;
                    if (TrackerManager.Instance != null) { TrackerManager.Instance.totalSelectedArea = totalAreaSelected; }

                    if (selectionArea > squarePixelSelectionCutOff)
                    {
                        trackerManager.SpawnNotification($"Marked {selectionArea} px²", 1.5f, 10f);
                    }
                }
            
                isSelecting = false;
                fill.SetActive(false);
                outline.SetActive(false);
            }
        }
    }
}
