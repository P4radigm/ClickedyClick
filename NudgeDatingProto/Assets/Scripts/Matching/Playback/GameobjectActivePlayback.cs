using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameobjectActivePlayback : MonoBehaviour
{
    private PlaybackController controller;
    [SerializeField] private GameObject gameObjectPlayedBack;
    [SerializeField] private string uniqueId;

    private void Start()
    {
        controller = GetComponentInParent<PlaybackController>();
    }

    private void Update()
    {
        if(controller != null)
        {
            if (controller.intDataPoints.Count > 0)
            {
                gameObjectPlayedBack.SetActive(controller.intDataPoints[uniqueId][controller.currentFrame] == 1 ? true : false);
            }
        }
    }
}
