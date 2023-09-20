using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollPositionTracker : MonoBehaviour
{
    public string uniqueID;
    [SerializeField] private Scrollbar performanceScrollBar;
    [SerializeField] private RectTransform performanceContent;
    [SerializeField] private TrackerVisual trackerVisualManager;
    [SerializeField] private int noScrollLength;
    [SerializeField] private int scrollNotificationCutoff;
    public int totalPixelsScrolled = 0;
    public int pixelsScrolledInSession = 0;
    private List<int> lastScrollPositions = new();

    private void Start()
    {
        TrackerManager.Instance.AddTrackedInt(uniqueID);
        TrackerManager.Instance.sampleEvent.AddListener(HandleTracking);
        for (int i = 0; i < noScrollLength; i++)
        {
            lastScrollPositions.Add(0);
        }
    }

    private void HandleTracking()
    {
        int scrollPos = Mathf.RoundToInt((1 - performanceScrollBar.value) * performanceContent.rect.height);
        TrackerManager.Instance.TrackInt(uniqueID, scrollPos);

        totalPixelsScrolled += Mathf.Abs(scrollPos - lastScrollPositions[0]);
        TrackerManager.Instance.totalScrollDistance = totalPixelsScrolled;
        pixelsScrolledInSession += Mathf.Abs(scrollPos - lastScrollPositions[0]);

        lastScrollPositions.Insert(0, scrollPos);

        if(lastScrollPositions.Count > noScrollLength)
        {
            lastScrollPositions.RemoveAt(noScrollLength);
        }

        bool sameCheck = true;
        for (int i = 0; i < lastScrollPositions.Count; i++)
        {
            if (lastScrollPositions[0] != lastScrollPositions[i])
            {
                sameCheck = false;
            }
        }

        if (sameCheck)
        {
            if(pixelsScrolledInSession > scrollNotificationCutoff)
            {
                trackerVisualManager.SpawnNotification($"Scrolled {pixelsScrolledInSession} px", 1.5f, 10f);
            }
            pixelsScrolledInSession = 0;
        }
    }
}
