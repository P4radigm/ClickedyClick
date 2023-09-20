using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectSizePlayback : MonoBehaviour
{
    private PlaybackController controller;
    [SerializeField] private RectTransform rectSizePlayedBack;
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
                rectSizePlayedBack.sizeDelta = controller.vectorDataPoints[uniqueId][controller.currentFrame];
            }
        }
    }
}
