using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleStateTracker : MonoBehaviour
{
    public string uniqueID;
    [SerializeField] private PerformanceToggle toggle;

    private void Start()
    {
        //toggle = GetComponent<PerformanceToggle>();
        TrackerManager.Instance.AddTrackedInt(uniqueID);
        TrackerManager.Instance.sampleEvent.AddListener(HandleTracking);
    }

    private void HandleTracking()
    {
        int state = toggle.isOn ? 1 : 0;
        TrackerManager.Instance.TrackInt(uniqueID, state);
    }
}
