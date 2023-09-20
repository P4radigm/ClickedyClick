using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TogglePlayback : MonoBehaviour
{
    private PlaybackController controller;
    [SerializeField] private GameObject sliderOnObjects;
    [SerializeField] private GameObject sliderOffObjects;
    [SerializeField] private string uniqueId;
    public bool isOn = false;

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
                sliderOnObjects.SetActive(controller.intDataPoints[uniqueId][controller.currentFrame] == 1);
                sliderOffObjects.SetActive(controller.intDataPoints[uniqueId][controller.currentFrame] != 1);
                isOn = controller.intDataPoints[uniqueId][controller.currentFrame] == 1;
            }
        }
    }
}