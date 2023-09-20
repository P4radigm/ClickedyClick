using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InputFieldTracker : MonoBehaviour
{
    public string uniqueID;
    [SerializeField] private TMP_InputField trackedInputfield;

    private void Start()
    {
        TrackerManager.Instance.AddTrackedString(uniqueID);
        TrackerManager.Instance.sampleEvent.AddListener(HandleTracking);
    }

    private void HandleTracking()
    {
        string text = trackedInputfield.text;
        TrackerManager.Instance.TrackString(uniqueID, text);
    }
}
