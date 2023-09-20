using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollPlayback : MonoBehaviour
{
    private PlaybackController controller;
    [SerializeField] private Scrollbar scrollbarPlayedBack;
    [SerializeField] private RectTransform content;
    [SerializeField] private string uniqueId;

    private void Start()
    {
        controller = GetComponentInParent<PlaybackController>();
    }

    private void Update()
    {
        if (controller != null)
        {
            if (controller.intDataPoints.Count > 0)
            {
                scrollbarPlayedBack.value = 1 - ((float)controller.intDataPoints[uniqueId][controller.currentFrame] / (float)content.sizeDelta.y);
            }
        }
    }
}
