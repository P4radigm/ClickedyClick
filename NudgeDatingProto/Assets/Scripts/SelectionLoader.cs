using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class SelectionLoader : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Slider loadingVisual;
    [SerializeField] private TextMeshProUGUI loadingVisualPercentage;
    [SerializeField] private Vector2 progressBarMinMax;
    [SerializeField] private TextMeshProUGUI catOneText;
    [SerializeField] private TextMeshProUGUI catTwoText;
    [SerializeField] private TextMeshProUGUI ownCatOneText;
    [SerializeField] private TextMeshProUGUI ownCatTwoText;
    [SerializeField] private TextMeshProUGUI ownIdText;
    [SerializeField] private TextMeshProUGUI ownStatOneText;
    [SerializeField] private TextMeshProUGUI ownStatTwoText;
    [SerializeField] private TextMeshProUGUI ownStatThreeText;
    [SerializeField] private TextMeshProUGUI ownStatFourText;
    [SerializeField] private TextMeshProUGUI ownStatFiveText;
    [SerializeField] private TextMeshProUGUI ownStatSixText;
    [SerializeField] private string actionText;
    [SerializeField] private TextMeshProUGUI actionPrompt;

    [Header("Animators")]
    [SerializeField] private TmpOpacityAnimation messageOneOpacity;
    [SerializeField] private TmpOpacityAnimation messageTwoOpacity;
    [SerializeField] private TmpOpacityAnimation messageThreeOpacity;
    [SerializeField] private TmpOpacityAnimation youAreAOpacity;
    [SerializeField] private TmpOpacityAnimation catTagOneOpacity;
    [SerializeField] private TmpOpacityAnimation catTagTwoOpacity;
    [SerializeField] private TmpOpacityAnimation ownIdOpacity;
    [SerializeField] private TmpOpacityAnimation statOneOneOpacity;
    [SerializeField] private TmpOpacityAnimation statOneTwoOpacity;
    [SerializeField] private TmpOpacityAnimation statOneThreeOpacity;
    [SerializeField] private TmpOpacityAnimation statTwoOneOpacity;
    [SerializeField] private TmpOpacityAnimation statTwoTwoOpacity;
    [SerializeField] private TmpOpacityAnimation statTwoThreeOpacity;
    [SerializeField] private TmpOpacityAnimation statThreeOneOpacity;
    [SerializeField] private TmpOpacityAnimation statThreeTwoOpacity;
    [SerializeField] private TmpOpacityAnimation statThreeThreeOpacity;
    [SerializeField] private TmpOpacityAnimation statFourOneOpacity;
    [SerializeField] private TmpOpacityAnimation statFourTwoOpacity;
    [SerializeField] private TmpOpacityAnimation statFourThreeOpacity;
    [SerializeField] private TmpOpacityAnimation statFiveOneOpacity;
    [SerializeField] private TmpOpacityAnimation statFiveTwoOpacity;
    [SerializeField] private TmpOpacityAnimation statFiveThreeOpacity;
    [SerializeField] private TmpOpacityAnimation statSixOneOpacity;
    [SerializeField] private TmpOpacityAnimation statSixTwoOpacity;
    [SerializeField] private TmpOpacityAnimation statSixThreeOpacity;
    [SerializeField] private TmpOpacityAnimation ownCatTextOneOpacity;
    [SerializeField] private TmpOpacityAnimation ownCatTextTwoOpacity;
    [SerializeField] private ImageOpacityAnimation ownCatFillOpacity;
    [SerializeField] private ImageOpacityAnimation ownCatOutlineOpacity;
    [SerializeField] private TmpOpacityAnimation actionPromptOpacity;
    
    [Header("Timings")]
    [SerializeField] private float loadingTime;
    [SerializeField] private AnimationCurve loadingCurve;
    [SerializeField] private float messageTwoTiming;

    [SerializeField] private float ownIdTiming;
    [SerializeField] private float statOneTiming;
    [SerializeField] private float statTwoTiming;
    [SerializeField] private float statThreeTiming;
    [SerializeField] private float statFourTiming;
    [SerializeField] private float statFiveTiming;
    [SerializeField] private float statSixTiming;

    [SerializeField] private float youAreATiming;
    [SerializeField] private float catTagOneTiming;
    [SerializeField] private float catTagTwoTiming;
    [SerializeField] private float ownCatTiming;

    [SerializeField] private float messageThreeTiming;
    [SerializeField] private float actionPromptTiming;

    private float timer = 0;
    private RouterManager routerManager;
    private NewDataManager dataManager;
    private bool isActive = false;

    public void Initialise()
    {
        routerManager = RouterManager.instance;
        dataManager = NewDataManager.instance;
        timer = 0;
        isActive = true;

        //Set Text to correct stats
        catOneText.text = dataManager.categoryStrings[dataManager.ownProfileData.assignedCategoryId].Split(' ')[0];
        catTwoText.text = dataManager.categoryStrings[dataManager.ownProfileData.assignedCategoryId].Split(' ')[1];
        ownCatOneText.text = dataManager.categoryStrings[dataManager.ownProfileData.assignedCategoryId].Split(' ')[0];
        ownCatTwoText.text = dataManager.categoryStrings[dataManager.ownProfileData.assignedCategoryId].Split(' ')[1];
        ownIdText.text = $"user<br><b>#</b>{dataManager.ownProfileData.id.Substring(dataManager.ownProfileData.id.Length - 6)}";
        ownStatOneText.text = dataManager.ownProfileData.totalCharacters.ToString();
        ownStatTwoText.text = dataManager.ownProfileData.totalClicks.ToString();
        ownStatThreeText.text = dataManager.ownProfileData.pixelsScrolled.ToString();
        ownStatFourText.text = dataManager.ownProfileData.secondsHovered.ToString("F1");
        ownStatFiveText.text = $"{Mathf.RoundToInt((float)dataManager.ownProfileData.pixelsSelected / 1000f)} K";
        ownStatSixText.text = dataManager.ownProfileData.pixelsDragged.ToString();
        actionPrompt.text = actionText;

        //Start timing routines
        StartCoroutine(MessageTimingIE());
        StartCoroutine(OwnStatsTimingIE());
        StartCoroutine(CatTimingIE());
    }

    private void Update()
    {
        if (!isActive) { return; }

        if(timer > loadingTime)
        {
            routerManager.SetProgressSlider(0.75f);
            routerManager.SetProgressIcon(3, true);
            routerManager.LoadExperienceSegment((int)RouterManager.ExperienceSegment.Selection);
            isActive = false;
        }

        timer+=Time.deltaTime;

        loadingVisual.value = loadingCurve.Evaluate(timer/loadingTime);
        loadingVisualPercentage.text = $"{(int)(loadingCurve.Evaluate(timer / loadingTime)*100)}%";

        routerManager.SetProgressSlider(progressBarMinMax.x + (loadingCurve.Evaluate(timer / loadingTime) * (progressBarMinMax.y - progressBarMinMax.x)));
    }

    private IEnumerator MessageTimingIE()
    {
        messageOneOpacity.AnimateOpacityTo(1f);
        yield return new WaitForSeconds(messageTwoTiming);
        messageOneOpacity.AnimateOpacityTo(0.4f);
        messageTwoOpacity.AnimateOpacityTo(1f);
        yield return new WaitForSeconds(messageThreeTiming - messageTwoTiming);
        messageTwoOpacity.AnimateOpacityTo(0.4f);
        messageThreeOpacity.AnimateOpacityTo(1f);
    }

    private IEnumerator OwnStatsTimingIE()
    {
        yield return new WaitForSeconds(ownIdTiming);
        ownIdOpacity.AnimateOpacityTo(1f);
        yield return new WaitForSeconds(statOneTiming-ownIdTiming);
        statOneOneOpacity.AnimateOpacityTo(1f);
        statOneTwoOpacity.AnimateOpacityTo(1f);
        statOneThreeOpacity.AnimateOpacityTo(1f);
        yield return new WaitForSeconds(statTwoTiming-statOneTiming);
        statTwoOneOpacity.AnimateOpacityTo(1f);
        statTwoTwoOpacity.AnimateOpacityTo(1f);
        statTwoThreeOpacity.AnimateOpacityTo(1f);
        yield return new WaitForSeconds(statThreeTiming- statTwoTiming);
        statThreeOneOpacity.AnimateOpacityTo(1f);
        statThreeTwoOpacity.AnimateOpacityTo(1f);
        statThreeThreeOpacity.AnimateOpacityTo(1f);
        yield return new WaitForSeconds(statFourTiming- statThreeTiming);
        statFourOneOpacity.AnimateOpacityTo(1f);
        statFourTwoOpacity.AnimateOpacityTo(1f);
        statFourThreeOpacity.AnimateOpacityTo(1f);
        yield return new WaitForSeconds(statFiveTiming - statFourTiming);
        statFiveOneOpacity.AnimateOpacityTo(1f);
        statFiveTwoOpacity.AnimateOpacityTo(1f);
        statFiveThreeOpacity.AnimateOpacityTo(1f);
        yield return new WaitForSeconds(statSixTiming - statFiveTiming);
        statSixOneOpacity.AnimateOpacityTo(1f);
        statSixTwoOpacity.AnimateOpacityTo(1f);
        statSixThreeOpacity.AnimateOpacityTo(1f);
    }

    private IEnumerator CatTimingIE()
    {
        yield return new WaitForSeconds(youAreATiming);
        youAreAOpacity.AnimateOpacityTo(1f);
        yield return new WaitForSeconds(catTagOneTiming-youAreATiming);
        ColorManager.instance.StartFadeToColorScheme(dataManager.ownProfileData.assignedCategoryId);
        catTagOneOpacity.AnimateOpacityTo(1f);
        yield return new WaitForSeconds(catTagTwoTiming-catTagOneTiming);
        catTagTwoOpacity.AnimateOpacityTo(1f);
        yield return new WaitForSeconds(ownCatTiming- catTagTwoTiming);
        ownCatTextOneOpacity.AnimateOpacityTo(1f);
        ownCatTextTwoOpacity.AnimateOpacityTo(1f);
        ownCatFillOpacity.AnimateOpacityTo(1f);
        ownCatOutlineOpacity.AnimateOpacityTo(1f);
        yield return new WaitForSeconds(actionPromptTiming - ownCatTiming);
        actionPromptOpacity.AnimateOpacityTo(1f);
    }
}
