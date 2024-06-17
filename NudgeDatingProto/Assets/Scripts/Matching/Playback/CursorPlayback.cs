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
                playbackCursor.anchoredPosition = NormalisePositions(controller.vectorDataPoints[idPositions][controller.currentFrame]);
            }
        }
    }

    private Vector2 NormalisePositions(Vector2 input)
    {
        float normalisedX = input.x / controller.PlaybackResolution.x * 1920f;
        float normalisedY = input.y / controller.PlaybackResolution.y * 1080f;

        return new Vector2 (normalisedX, normalisedY);
    }
}
