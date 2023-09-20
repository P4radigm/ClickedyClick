using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextTracker : MonoBehaviour
{
    public string uniqueID;
    [SerializeField] private TextMeshProUGUI trackedText;

    private void Start()
    {
        TrackerManager.Instance.AddTrackedString(uniqueID);
        TrackerManager.Instance.sampleEvent.AddListener(HandleTracking);
    }

    private void HandleTracking()
    {
        string text = trackedText.text;
        TrackerManager.Instance.TrackString(uniqueID, text);
    }
}
