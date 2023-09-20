using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectTransformPosTracker : MonoBehaviour
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
        Vector2 position = rectTransformToTrack.anchoredPosition;
        TrackerManager.Instance.TrackVectorTwo(uniqueID, position);
    }
}
