using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TrackerVisual : MonoBehaviour
{
    //Checks
    [HideInInspector] public bool isAtPerformance = false;

    [Header("Follow Line Options")]
    [SerializeField] private LineRenderer followLine;
    [SerializeField] private Vector2 followLineOffset;
    [SerializeField] private float timeBetweenPoints;
    [SerializeField] private int totalPoints;
    [SerializeField] private float zDistance;
    [SerializeField] private float startDashedLineSize;
    [SerializeField] private float startLineThickness;
    private List<Vector3> mousePositionsWorld = new();
    private float pointTimer;
    //endAnim
    [Space(20)]
    [SerializeField] private float dwindleDuration;
    [SerializeField] private AnimationCurve dwindleCurve;

    [Header("User ID Tag Options")]
    [SerializeField] private RectTransform userIdElement;
    [SerializeField] private TextMeshProUGUI idText;
    [SerializeField] private Canvas idCanvas;
    [SerializeField] private Vector2 idOffset;

    [Header("Notification Options")]
    [SerializeField] private RectTransform notificationSpawnParent;
    [SerializeField] private GameObject notificationPrefab;
    [SerializeField] private Vector2 notificationOffset;
    private int totalClicks = 0;

    [Header("Keyboard Input Notifications Options")]
    [SerializeField] private List<KeyCode> whiteListKeys;
    [SerializeField] private Vector2 minMaxSpawnVert;
    [SerializeField] private Vector2 minMaxSpawnHorz;
    [SerializeField] private float keyNotificationDuration;
    [SerializeField] private float keyNotificationSpeed;
    private int totalCharactersTyped = 0;

    [Header("Idle Input Notifications Options")]
    [SerializeField] float minIdleTime;
    [SerializeField] private int framesOfActivy;
    private float idleTime;
    private bool[] previousActiveFrames;
    private Vector3 cursorLastPosition;
    private float totalSecondsHovered = 0;

    public void StartInit(string userID)
    {
        isAtPerformance = false;
        SetUserIdTagText(userID);
    }

    public void PerformanceInit()
    {
        isAtPerformance = true;
        pointTimer = 0;
        SetLineDashSize(startDashedLineSize);
        SetLineThickness(startLineThickness);

        previousActiveFrames = new bool[framesOfActivy];
        for (int i = 0; i < previousActiveFrames.Length; i++)
        {
            previousActiveFrames[i] = false;
        }
    }

    public void PerformanceFinished()
    {
        StartCoroutine(DwindleFollowLine());
    }

    void Update()
    {
        UpdateFollowLine();
        UpdateIdTagPosition();

        //click
        // drag & drop - Vector2
        // marking - pixels^2
        // idle - seconds
        // type - letter
        // scroll - pixels down or up

        if (!isAtPerformance) { return; }
        CheckForMouseNotificationOutput();
        CheckForKeyboardNotificationOutput();
        CheckForIdleNotificationOutput();
    }

    private void UpdateFollowLine()
    {
        if (!isAtPerformance) { return; }

        if(pointTimer > 0)
        { 
            pointTimer -= Time.deltaTime; 
        }
        else
        {
            Vector2 newWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePositionsWorld.Add(new Vector3(newWorldPos.x + followLineOffset.x, newWorldPos.y + followLineOffset.y, zDistance));

            if(mousePositionsWorld.Count > totalPoints)
            {
                for (int i = 0; i < mousePositionsWorld.Count - totalPoints; i++)
                {
                    mousePositionsWorld.RemoveAt(0);
                }
            }
            pointTimer = timeBetweenPoints;
        }

        followLine.positionCount = mousePositionsWorld.Count;
        for (int i = 0; i < mousePositionsWorld.Count; i++)
        {
            followLine.SetPosition(i, mousePositionsWorld[i]);
        }
    }

    private void UpdateIdTagPosition()
    {
        userIdElement.anchoredPosition = new Vector2((Input.mousePosition.x + idOffset.x) / idCanvas.scaleFactor, (Input.mousePosition.y + idOffset.y) / idCanvas.scaleFactor);
    }

    private void CheckForMouseNotificationOutput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SpawnNotification($"click (left)", 0.75f, 12.5f);
            totalClicks++;
        }
        
        if (Input.GetMouseButtonDown(1))
        {
            SpawnNotification($"click (right)", 0.75f, 12.5f);
            totalClicks++;
        }

        if (Input.GetMouseButtonDown(2))
        {
            SpawnNotification($"click (middle)", 0.75f, 12.5f);
            totalClicks++;
        }

        if (TrackerManager.Instance != null) { TrackerManager.Instance.totalClicks = totalClicks; }
    }

    private void CheckForKeyboardNotificationOutput()
    {
        if (Input.anyKeyDown)
        {
            foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(keyCode))
                {
                    string key = keyCode.ToString();
                    //Debug.Log(key);
                    if (whiteListKeys.Contains(keyCode))
                    {
                        Vector2 spawnLocation = new Vector2(Random.Range(minMaxSpawnHorz.x, minMaxSpawnHorz.y), Random.Range(minMaxSpawnVert.x, minMaxSpawnVert.y));
                        SpawnSpecificLocationNotification(key, keyNotificationDuration, keyNotificationSpeed, spawnLocation);
                        totalCharactersTyped++;
                        if (TrackerManager.Instance != null) { TrackerManager.Instance.totalCharactersTyped = totalCharactersTyped; }
                    }
                }
            }
        }
    }

    private void CheckForIdleNotificationOutput()
    {
        bool wasActiveInPreviousFrames = false;
        bool activeThisFrame = false;
        for (int i = 0; i < previousActiveFrames.Length; i++)
        {
            if (previousActiveFrames[i]) { wasActiveInPreviousFrames = true; }
        }

        if (cursorLastPosition == Input.mousePosition)
        {
            if (!wasActiveInPreviousFrames)
            {
                idleTime += Time.deltaTime;
            }
        }
        else
        {
            activeThisFrame = true;
            if (idleTime > minIdleTime)
            {
                SpawnNotification($"Hovered for {idleTime:F2} s", 1.5f, 10f);
            }
            totalSecondsHovered += idleTime;
            if (TrackerManager.Instance != null) { TrackerManager.Instance.totalSecondsHovered = totalSecondsHovered; };
            idleTime = 0;
        }
        cursorLastPosition = Input.mousePosition;

        bool[] newActiveFrameArray = new bool[previousActiveFrames.Length];

        for (int i = 0; i < newActiveFrameArray.Length; i++)
        {
            if (i != 0)
            {
                newActiveFrameArray[i] = previousActiveFrames[i - 1];
            }
            else
            {
                newActiveFrameArray[i] = activeThisFrame;
            }
        }

        previousActiveFrames = newActiveFrameArray;


    }

    public void SpawnNotification(string text, float displayTime, float upwardSpeed)
    {
        GameObject newNotification = Instantiate(notificationPrefab, notificationSpawnParent);
        RectTransform newNotificationT = newNotification.GetComponent<RectTransform>();
        newNotificationT.anchoredPosition = new Vector2((Input.mousePosition.x + notificationOffset.x) / idCanvas.scaleFactor, (Input.mousePosition.y + notificationOffset.y) / idCanvas.scaleFactor);
        TrackerNotificationController newNotificationC = newNotification.GetComponent<TrackerNotificationController>();
        newNotificationC.SpawnInit(text, displayTime, upwardSpeed);
    }

    public void SpawnSpecificLocationNotification(string text, float displayTime, float upwardSpeed, Vector2 location)
    {
        GameObject newNotification = Instantiate(notificationPrefab, notificationSpawnParent);
        RectTransform newNotificationT = newNotification.GetComponent<RectTransform>();
        newNotificationT.anchoredPosition = location;
        TrackerNotificationController newNotificationC = newNotification.GetComponent<TrackerNotificationController>();
        newNotificationC.SpawnInit(text, displayTime, upwardSpeed);
    }

    private IEnumerator DwindleFollowLine()
    {
        float timeValue = 0;

        while (timeValue < 1)
        {
            timeValue += Time.deltaTime / dwindleDuration;
            float evaluatedTimeValue = dwindleCurve.Evaluate(timeValue);
            int newMaxPoints = (int)Mathf.Lerp(followLine.positionCount, 0, evaluatedTimeValue);
            totalPoints = newMaxPoints;
            yield return null;
        }

        isAtPerformance = false;
    }

    public void SetLineDashSize(float size)
    {
        followLine.textureScale = new Vector2(size, 1);
    }

    public void SetLineThickness(float thickness)
    {
        followLine.startWidth = thickness;
        followLine.endWidth = thickness;
    }

    public void SetUserIdTagText(string id)
    {
        idText.text = $"User#{id}";
    }
}
