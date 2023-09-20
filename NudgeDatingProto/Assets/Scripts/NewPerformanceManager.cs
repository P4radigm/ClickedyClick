using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NewPerformanceManager : MonoBehaviour
{
    private RouterManager routerManager;
    private NewDataManager dataManager;
    private TrackerManager trackerManager;

    private float performanceDuration;
    private float timer;
    [SerializeField] private Animator performanceAnimator;
    [SerializeField] private TextMeshProUGUI timeLeftText;
    [SerializeField] private Vector2 progressBarMinMax;

    [Header("Debug")]
    [SerializeField] private bool addDataEarly;
    [SerializeField] private string debugMail;

    public void Initialise()
    {
        routerManager = RouterManager.instance;
        dataManager = NewDataManager.instance;
        trackerManager = TrackerManager.Instance;

        performanceDuration = dataManager.totalPerformanceTime;
        timer = performanceDuration;

        performanceAnimator.SetTrigger("InitialiseTrigger");

        //dataManager.currentUserData.screenResolution = new Vector2(Screen.width, Screen.height);
    }

    private void Update()
    {
        if(timer < 0)
        {
            trackerManager.SendDataToDataManager();
            dataManager.AddToAverages();

            performanceAnimator.SetTrigger("CloseTrigger");

            //Assign category
            AssignCategory();

            //DEBUG
            if (addDataEarly)
            {
                dataManager.ownProfileData.email = debugMail;
                dataManager.SaveUserDataLocal(dataManager.ownProfileData);
            }

            routerManager.SetProgressIcon(2, true);
            routerManager.LoadExperienceSegment((int)RouterManager.ExperienceSegment.LoadToSelection);
            timer = performanceDuration;
        }

        // Convert timer to TimeSpan
        TimeSpan timeSpan = TimeSpan.FromSeconds(timer);

        // Format the time as m:ss
        string formattedTime = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);

        timeLeftText.text = formattedTime;

        routerManager.SetProgressSlider(progressBarMinMax.x + ((1-timer / performanceDuration) * (progressBarMinMax.y - progressBarMinMax.x)));

        timer -= Time.deltaTime;
    }

    private void AssignCategory()
    {
        /*
         * 0 - Wild Clicker
         * 1 - Pro Marker
         * 2 - Tender Typer
         * 3 - Lazy Hoverer
         * 4 - Draggy Dropper
         * 5 - Steady Scroller
         */

        List<int> categoryIds = new();

        if(dataManager.ownProfileData.totalClicks > 30)
        {
            categoryIds.Add(0);
        }

        if(dataManager.ownProfileData.pixelsSelected > 1000000)
        {
            categoryIds.Add(1);
        }

        if (dataManager.ownProfileData.totalCharacters > 12 && dataManager.ownProfileData.totalCharacters < 50)
        {
            categoryIds.Add(2);
        }

        if (dataManager.ownProfileData.secondsHovered > 30)
        {
            categoryIds.Add(3);
        }

        if (dataManager.ownProfileData.pixelsDragged > 1500)
        {
            categoryIds.Add(4);
        }

        if (dataManager.ownProfileData.pixelsScrolled >= 4050 && dataManager.ownProfileData.pixelsScrolled < 20000)
        {
            categoryIds.Add(5);
        }

        if(categoryIds.Count == 0) { categoryIds.Add(3); }

        dataManager.ownProfileData.assignedCategoryId = categoryIds[UnityEngine.Random.Range(0, categoryIds.Count)];

        //List<int> categoryIds = new();

        //if(dataManager.currentUserData.totalClicks > 15)
        //{
        //    categoryIds.Add(0);
        //}

        //if(dataManager.currentUserData.totalCharacters > 5)
        //{
        //    categoryIds.Add(1);
        //}

        //if(dataManager.currentUserData.pixelsScrolled > 1000)
        //{
        //    categoryIds.Add(2);
        //}

        //categoryIds.Add(3);

        //dataManager.currentUserData.categoryId = categoryIds[UnityEngine.Random.Range(0, categoryIds.Count)];
    }
}
