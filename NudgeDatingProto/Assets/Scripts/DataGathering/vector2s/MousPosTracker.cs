using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousPosTracker : MonoBehaviour
{
    public string uniqueID;

    private void Start()
    {
        TrackerManager.Instance.AddTrackedVectorTwo(uniqueID);
        TrackerManager.Instance.sampleEvent.AddListener(HandleTracking);
    }

    private void HandleTracking()
    {
        Vector2 mousePos = Input.mousePosition;
        TrackerManager.Instance.TrackVectorTwo(uniqueID, mousePos);
    }
}
