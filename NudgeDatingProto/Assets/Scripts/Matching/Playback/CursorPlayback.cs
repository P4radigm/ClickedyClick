using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorPlayback : MonoBehaviour
{
    [SerializeField] private RectTransform playbackCursor;
    [SerializeField] private Image playbackCursorRenderer;
    [SerializeField] private Sprite[] cursorSprites;
    [SerializeField] private string idState;
    [SerializeField] private string idPositions;
    private PlaybackController controller;

    private void Start()
    {
        controller = GetComponentInParent<PlaybackController>();
    }

    private void Update()
    {
        if (controller != null)
        {
            if (controller.vectorDataPoints.Count > 0 && controller.intDataPoints.Count > 0)
            {
                playbackCursorRenderer.sprite = cursorSprites[controller.intDataPoints[idState][controller.currentFrame]];
                playbackCursor.anchoredPosition = controller.vectorDataPoints[idPositions][controller.currentFrame];
            }
        }
    }
}
