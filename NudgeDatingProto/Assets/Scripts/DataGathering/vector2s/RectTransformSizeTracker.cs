using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectTransformSizeTracker : MonoBehaviour
{
    public string uniqueID;
    [SerializeField] private RectTransform rectTransformToTrack;

    private void Start()
    {
        TrackerManager.Instance.AddTrackedVectorTwo(uniqueID);
        TrackerManager.Instance.sampleEvent.AddListener(HandleTracking);
    }

    private void HandleTracking()
    {
        Vector2 size = rectTransformToTrack.sizeDelta;
        TrackerManager.Instance.TrackVectorTwo(uniqueID, size);
    }
}
