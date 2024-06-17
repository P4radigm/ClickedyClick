using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectPosPlayback : MonoBehaviour
{
    private PlaybackController controller;
    [SerializeField] private RectTransform rectPosPlayedBack;
    [SerializeField] private string uniqueId;

    private void Start()
    {
        controller = GetComponentInParent<PlaybackController>();
    }

    private void Update()
    {
        if (controller != null)
        {
            if (controller.vectorDataPoints.Count > 0)
            {
                rectPosPlayedBack.anchoredPosition = controller.vectorDataPoints[uniqueId][controller.currentFrame];
            }
        }
    }

    private Vector2 NormalisePositions(Vector2 input)
    {
        float normalisedX = input.x / controller.PlaybackResolution.x * 1920f;
        float normalisedY = input.y / controller.PlaybackResolution.y * 1080f;

        return new Vector2(normalisedX, normalisedY);
    }
}
