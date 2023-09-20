using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorStateTracker : MonoBehaviour
{
    public string uniqueID;

    private void Start()
    {
        TrackerManager.Instance.AddTrackedInt(uniqueID);
        TrackerManager.Instance.sampleEvent.AddListener(HandleTracking);
    }

    private void HandleTracking()
    {
        int state = CursorIconManager.instance.currentCursorState;
        TrackerManager.Instance.TrackInt(uniqueID, state);
    }
}
