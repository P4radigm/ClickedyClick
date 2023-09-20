using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PerformanceManager : MonoBehaviour
{
    private RouterManager routerManager;
    private OwnDataManager dataManager;

    private float performanceDuration;
    private float timer;
    [SerializeField] private Animator performanceAnimator;
    [SerializeField] private TextMeshProUGUI timeLeftText;
    [SerializeField] private PerformanceSearch performanceSearch;
    [SerializeField] private Vector2 progressBarMinMax;
    [SerializeField] private Scrollbar performanceScrollBar;
    private float performanceScrollBarLastPosition;
    private Vector3 cursorLastPosition;
    private float totalScrollDistance;
    [SerializeField] private KeyCode[] blacklistedKeys;
    [SerializeField] private RectTransform performanceContent;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private int framesOfActivy;
    private bool[] previousActiveFrames;
    private float nextSampleTime;
    private float deltaTime;

    public void Initialise()
    {
        routerManager = RouterManager.instance;
        dataManager = OwnDataManager.instance;

        performanceDuration = dataManager.totalPerformanceTime;
        timer = performanceDuration;
        previousActiveFrames = new bool[framesOfActivy];
        for (int i = 0; i < previousActiveFrames.Length; i++)
        {
            previousActiveFrames[i] = false;
        }

        performanceAnimator.SetTrigger("InitialiseTrigger");

        performanceSearch.GenerateCardList();
        performanceSearch.HandleNewSearchInput();

        nextSampleTime = Time.time + (1.0f / dataManager.sampleRate);

        //GenericStateTracker[] availableTrackers = GetComponentsInChildren<GenericStateTracker>();

        //for (int i = 0; i < availableTrackers.Length; i++)
        //{
        //    trackers.Add(availableTrackers[i]);
        //}

        //trackers.Sort((x, y) => x.id.CompareTo(y.id));

        //for (int i = 0; i < trackers.Count; i++)
        //{
        //    APImanager.GenericState newStateTracker = new();
        //    newStateTracker.id = trackers[i].id;
        //    newStateTracker.state.Clear();
        //    dataManager.currentUserData.genericStates.Add(newStateTracker);
        //}

        dataManager.currentUserData.screenResolution = new Vector2(Screen.width, Screen.height);
    }

    private void Update()
    {
        if(timer < 0)
        {
            performanceAnimator.SetTrigger("CloseTrigger");

            //Round total scroll distance to pixels and send to datamanager
            dataManager.currentUserData.pixelsScrolled = Mathf.RoundToInt(totalScrollDistance);

            //Assign category
            AssignCategory();

            routerManager.SetProgressIcon(2, true);
            routerManager.LoadExperienceSegment((int)RouterManager.ExperienceSegment.LoadToSelection);
        }

        // Convert timer to TimeSpan
        TimeSpan timeSpan = TimeSpan.FromSeconds(timer);

        // Format the time as m:ss
        string formattedTime = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);

        timeLeftText.text = formattedTime;

        routerManager.SetProgressSlider(progressBarMinMax.x + ((1-timer / performanceDuration) * (progressBarMinMax.y - progressBarMinMax.x)));

        HandleDataGathering();

        timer -= Time.deltaTime;
    }

    private void HandleDataGathering()
    {
        //send data to datamanager based on sample rate
        if (Time.time >= nextSampleTime && timer >= 0)
        {
            //Send data to curentUserData list
            //Send cursorPosition
            dataManager.currentUserData.cursorPositions.Add(Input.mousePosition);
            //Send scrollPosition
            dataManager.currentUserData.scrollPositions.Add(Mathf.RoundToInt((1-performanceScrollBar.value) * performanceContent.rect.height));
            //Send inputFieldState
            dataManager.currentUserData.inputFieldStates.Add(inputField.text);
            //Send genericStates
            //for (int i = 0; i < trackers.Count; i++)
            //{
            //    if(dataManager.currentUserData.genericStates[i].id != trackers[i].id) { Debug.LogWarning($"Generic tracker id does not match id in currentuserdata"); }
            //    dataManager.currentUserData.genericStates[i].state.Add(trackers[i].state);
            //}

            nextSampleTime += (1.0f / dataManager.sampleRate);
        }

        if(timer >= 0)
        {
            //Gather statistics
            //totalClicks
            bool clickActivity = CalculateClickingTotal();
            //pixelsScrolled
            bool scrollActivity = CalculateScrollingTotal();   
            //totalCharacters
            bool keyActivity = CalculateKeyTotal();
            //secondsHovered
            CalculateInactivityTotal(clickActivity, scrollActivity, keyActivity);
        }


        // Calculate the FPS value
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float fps = 1f / deltaTime;

        // Check if the FPS falls under the threshold
        if (fps < dataManager.sampleRate)
        {
            // Perform actions when the FPS is below the threshold
            Debug.Log("FPS is below the threshold!, data is dirty");
            dataManager.dirtyData = true;
        }
    }

    private void AssignCategory()
    {
        List<int> categoryIds = new();

        if(dataManager.currentUserData.totalClicks > 15)
        {
            categoryIds.Add(0);
        }
        
        if(dataManager.currentUserData.totalCharacters > 5)
        {
            categoryIds.Add(1);
        }
        
        if(dataManager.currentUserData.pixelsScrolled > 1000)
        {
            categoryIds.Add(2);
        }

        categoryIds.Add(3);

        dataManager.currentUserData.categoryId = categoryIds[UnityEngine.Random.Range(0, categoryIds.Count)];
    }

    private bool CalculateScrollingTotal()
    {
        bool returnBool = false;
        if(performanceScrollBarLastPosition != performanceScrollBar.value)
        {
            float changeInPos = Mathf.Abs(performanceScrollBarLastPosition - performanceScrollBar.value);
            float pixelsScrolled = changeInPos * performanceContent.rect.height;
            totalScrollDistance += pixelsScrolled;
            returnBool = true;
        }

        performanceScrollBarLastPosition = performanceScrollBar.value;
        return returnBool;
    }

    private bool CalculateClickingTotal()
    {
        bool returnBool = false;
        if (Input.GetMouseButtonDown(0)) { dataManager.currentUserData.totalClicks++; returnBool = true; }

        if (Input.GetMouseButtonDown(1)) { dataManager.currentUserData.totalClicks++; returnBool = true; }
        else if (Input.GetMouseButtonDown(2)) { dataManager.currentUserData.totalClicks++; returnBool = true; }
        else if (Input.GetMouseButtonDown(3)) { returnBool = true; }
        else if (Input.GetMouseButtonDown(5)) { returnBool = true; }
        else if (Input.GetMouseButtonDown(6)) { returnBool = true; }
        return returnBool;
    }

    private bool CalculateKeyTotal()
    {
        bool returnBool = false;

        foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(keyCode))
            {
                if (!IsKeyBlacklisted(keyCode))
                {
                    dataManager.currentUserData.totalCharacters++;
                }

                returnBool = true;
            }
        }

        return returnBool;
    }

    private void CalculateInactivityTotal(bool clickActivity, bool scrollActivity, bool keyActivity)
    {
        bool wasActiveInPreviousFrames = false;
        bool activeThisFrame = false;
        for (int i = 0; i < previousActiveFrames.Length; i++)
        {
            if (previousActiveFrames[i]) { wasActiveInPreviousFrames = true; }
        }

        if(cursorLastPosition == Input.mousePosition && !clickActivity && !scrollActivity && !keyActivity)
        {
            if (!wasActiveInPreviousFrames)
            {
                dataManager.currentUserData.secondsHovered += Time.deltaTime;
                //Debug.Log($"Inactive");
            }
        }
        else
        {
            activeThisFrame = true;
            //Debug.Log($"Active");
        }
        cursorLastPosition = Input.mousePosition;

        bool[] newActiveFrameArray = new bool[previousActiveFrames.Length];

        for (int i = 0; i < newActiveFrameArray.Length; i++)
        {
            if(i != 0)
            {
                newActiveFrameArray[i] = previousActiveFrames[i-1];
            }
            else
            {
                newActiveFrameArray[i] = activeThisFrame;
            }
        }

        previousActiveFrames = newActiveFrameArray;
    }

    private bool IsKeyBlacklisted(KeyCode keyCode)
    {
        // Check if the provided key code is in the blacklist
        for (int i = 0; i < blacklistedKeys.Length; i++)
        {
            if (blacklistedKeys[i] == keyCode)
            {
                return true;
            }
        }
        return false;
    }
}
