using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameobjectActiveStateTracker : MonoBehaviour
{
    public string uniqueID;
    [SerializeField] private GameObject gameObjectToTrack;

    private void Start()
    {
        TrackerManager.Instance.AddTrackedInt(uniqueID);
        TrackerManager.Instance.sampleEvent.AddListener(HandleTracking);
    }

    private void HandleTracking()
    {
        int state = gameObjectToTrack.activeInHierarchy ? 1 : 0;
        TrackerManager.Instance.TrackInt(uniqueID, state);
    }
}
