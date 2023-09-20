using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InputFieldPlayback : MonoBehaviour
{
    private PlaybackController controller;
    [SerializeField] private TMP_InputField inputFieldPlayedBack;
    [SerializeField] private string uniqueId;

    private void Start()
    {
        controller = GetComponentInParent<PlaybackController>();
    }

    private void Update()
    {
        if (controller != null)
        {
            if(controller.stringDataPoints.Count > 0)
            {
                inputFieldPlayedBack.text = controller.stringDataPoints[uniqueId][controller.currentFrame];
            }
        }
    }
}
