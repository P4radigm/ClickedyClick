using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextPlayback : MonoBehaviour
{
    private PlaybackController controller;
    [SerializeField] private TextMeshProUGUI textPlayedBack;
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
                textPlayedBack.text = controller.stringDataPoints[uniqueId][controller.currentFrame];
            }
        }
    }
}
