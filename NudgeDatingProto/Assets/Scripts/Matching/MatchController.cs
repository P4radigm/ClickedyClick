using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MatchController : MonoBehaviour
{
    [SerializeField] private MatchMaker matchManager;
    public int assignedControllerIndex;

    [Header("Playback")]
    public PlaybackController playbackController;
    public NewDataManager.ProfileData loadedProfileData;
    private NewDataManager dataManager;
    private ColorManager colorManager;

    [Header("Animation Refs")]
    [SerializeField] private SizeAnimation ownSizeAnimator;
    [SerializeField] private ImageOpacityAnimation opacityFilterAnimator;
    [SerializeField] private ImageOpacityAnimation[] catImageAnimators;
    [SerializeField] private TmpOpacityAnimation[] catTextAnimators;
    [SerializeField] private ImageOpacityAnimation[] rejectImageAnimators;
    [SerializeField] private ShapesOpacityAnimation[] rejectShapesAnimators;
    [SerializeField] private ImageOpacityAnimation[] acceptImageAnimators;
    [SerializeField] private ShapesOpacityAnimation[] acceptShapesAnimators;
    [SerializeField] private Button acceptButton;
    [SerializeField] private Button rejectButton;

    [Header("Animation To Active Settings")]
    [SerializeField] private float buttonsTimingIn;
    [SerializeField] private float buttonsDurationIn;
    [SerializeField] private float labelTimingIn;
    [SerializeField] private float labelDurationIn;
    [SerializeField] private float sizeTimingIn;
    [SerializeField] private float sizeDurationIn;
    [SerializeField] private float filterTimingIn;
    [SerializeField] private float filterDurationIn;

    [Header("Animation To Blank Settings")]
    [SerializeField] private float filterTimingBlank;
    [SerializeField] private float filterDurationBlank;
    [SerializeField] private float sizeTimingBlank;
    [SerializeField] private float sizeDurationBlank;
    [SerializeField] private float labelTimingBlank;
    [SerializeField] private float labelDurationBlank;
    //buttons instant disappear on animation start

    [Header("Animation To Deactive Settings")]
    [SerializeField] private float filterTimingDeactive;
    [SerializeField] private float filterDurationDeactive;
    //buttons are already gone
    //label is already gone
    //size is already normal

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI idText;
    [SerializeField] private TextMeshProUGUI catTextOne;
    [SerializeField] private TextMeshProUGUI catTextTwo;

    [Header("Total Texts")]
    [SerializeField] private TextMeshProUGUI valueCharacters;
    [SerializeField] private TextMeshProUGUI valueClicks;
    [SerializeField] private TextMeshProUGUI valueScrolled;
    [SerializeField] private TextMeshProUGUI valueHovered;
    [SerializeField] private TextMeshProUGUI valueSelected;
    [SerializeField] private TextMeshProUGUI valueDragged;

    [Header("Average Texts")]
    [SerializeField] private TextMeshProUGUI averageCharacters;
    [SerializeField] private TextMeshProUGUI averageClicks;
    [SerializeField] private TextMeshProUGUI averageScrolled;
    [SerializeField] private TextMeshProUGUI averageHovered;
    [SerializeField] private TextMeshProUGUI averageSelected;
    [SerializeField] private TextMeshProUGUI averageDragged;

    [Header("Coloring")]
    [SerializeField] private Image labelOutline;
    [SerializeField] private TextMeshProUGUI labelTextOne;
    [SerializeField] private TextMeshProUGUI labelTextTwo;

    private Coroutine animationRoutine;
    private int status = 0; //0 - deactivated, //1 - activated, //2 - blank

    public void Initialise(int index)
    {
        dataManager = NewDataManager.instance;
        colorManager = ColorManager.instance;
        assignedControllerIndex = index;

        CutToBlank();
    }

    public void LoadNewMatch(NewDataManager.ProfileData profileData)
    {
        //Send data to playbackController
        loadedProfileData = profileData;

        playbackController.UpdateDataLists(profileData);

        FillOutVisuals();
    }

    private void FillOutVisuals()
    {
        //Fill in texts
        idText.text = $"user #{loadedProfileData.id.Substring(loadedProfileData.id.Length - 6)}";
        catTextOne.text = dataManager.categoryStrings[loadedProfileData.assignedCategoryId].Split(' ')[0];
        catTextTwo.text = dataManager.categoryStrings[loadedProfileData.assignedCategoryId].Split(' ')[1];

        //Fill in total texts
        valueCharacters.text = Mathf.RoundToInt(loadedProfileData.totalCharacters).ToString();
        valueClicks.text = Mathf.RoundToInt(loadedProfileData.totalClicks).ToString();
        valueScrolled.text = Mathf.RoundToInt(loadedProfileData.pixelsScrolled).ToString();
        valueHovered.text = Mathf.RoundToInt(loadedProfileData.secondsHovered).ToString("F1");
        valueSelected.text = $"{Mathf.RoundToInt((float)loadedProfileData.pixelsSelected / 1000f)} K"; ;
        valueDragged.text = Mathf.RoundToInt(loadedProfileData.pixelsDragged).ToString();

        //Fill in average values
        string lowerHigher = "below";

        lowerHigher = dataManager.currentAverages.averageCharacters > loadedProfileData.totalCharacters ? "below" : "above";
        averageCharacters.text = $"({Mathf.RoundToInt(Mathf.Abs(dataManager.currentAverages.averageCharacters - loadedProfileData.totalCharacters))} {lowerHigher} average)";

        lowerHigher = dataManager.currentAverages.averageClicks > loadedProfileData.totalClicks ? "below" : "above";
        averageClicks.text = $"({Mathf.RoundToInt(Mathf.Abs(dataManager.currentAverages.averageClicks - loadedProfileData.totalClicks))} {lowerHigher} average)";

        lowerHigher = dataManager.currentAverages.averageScrolled > loadedProfileData.pixelsScrolled ? "below" : "above";
        averageScrolled.text = $"({Mathf.RoundToInt(Mathf.Abs(dataManager.currentAverages.averageScrolled - loadedProfileData.pixelsScrolled))} {lowerHigher} average)";

        lowerHigher = dataManager.currentAverages.averageHovered > loadedProfileData.secondsHovered ? "below" : "above";
        averageHovered.text = $"({Mathf.Abs(dataManager.currentAverages.averageHovered - loadedProfileData.secondsHovered).ToString("F1")} {lowerHigher} average)";

        lowerHigher = dataManager.currentAverages.averageSelected > loadedProfileData.pixelsSelected ? "below" : "above";
        averageSelected.text = $"({Mathf.RoundToInt(Mathf.Abs(dataManager.currentAverages.averageSelected - loadedProfileData.pixelsSelected) / 1000f)} K {lowerHigher} average)";

        lowerHigher = dataManager.currentAverages.averageDragged > loadedProfileData.pixelsDragged ? "below" : "above";
        averageDragged.text = $"({Mathf.RoundToInt(Mathf.Abs(dataManager.currentAverages.averageDragged - loadedProfileData.pixelsDragged))} {lowerHigher} average)";

        //Handle color
        Color midOne = colorManager.colorSchemes[loadedProfileData.assignedCategoryId].colors[1];

        labelOutline.color = new Color(midOne.r, midOne.g, midOne.b, labelOutline.color.a);
        labelTextOne.color = new Color(midOne.r, midOne.g, midOne.b, labelTextOne.color.a);
        labelTextTwo.color = new Color(midOne.r, midOne.g, midOne.b, labelTextTwo.color.a);
    }

    public void ToActive()
    {
        if (status == 1) { return; }
        if(animationRoutine != null) { StopCoroutine(animationRoutine); }
        animationRoutine = StartCoroutine(ToActiveIE());
    }

    private IEnumerator ToActiveIE()
    {
        yield return new WaitForSeconds(filterTimingIn);
        opacityFilterAnimator.AnimTime = filterDurationIn;
        opacityFilterAnimator.AnimateOpacityTo(0f);
        yield return new WaitForSeconds(sizeTimingIn-filterTimingIn);
        ownSizeAnimator.MoveTime = sizeDurationIn;
        ownSizeAnimator.TargetWidth = ownSizeAnimator.gameObject.GetComponent<RectTransform>().sizeDelta.x;
        ownSizeAnimator.TargetHeight = 755f;
        ownSizeAnimator.MoveTo();
        yield return new WaitForSeconds(buttonsTimingIn-sizeTimingIn);

        for (int i = 0; i < rejectImageAnimators.Length; i++)
        {
            rejectImageAnimators[i].AnimTime = labelDurationIn;
            rejectImageAnimators[i].AnimateOpacityTo(1f);
        }

        for (int i = 0; i < rejectShapesAnimators.Length; i++)
        {
            rejectShapesAnimators[i].AnimTime = labelDurationIn;
            rejectShapesAnimators[i].AnimateOpacityTo(i == 0 ? 0.3f : 1f);
        }

        for (int i = 0; i < acceptImageAnimators.Length; i++)
        {
            acceptImageAnimators[i].AnimTime = labelDurationIn;
            acceptImageAnimators[i].AnimateOpacityTo(1f);
        }

        for (int i = 0; i < acceptShapesAnimators.Length; i++)
        {
            acceptShapesAnimators[i].AnimTime = labelDurationIn;
            acceptShapesAnimators[i].AnimateOpacityTo(i == 0 ? 0.3f : 1f);
        }

        yield return new WaitForSeconds(labelTimingIn-buttonsTimingIn);
        foreach(ImageOpacityAnimation animator in catImageAnimators)
        {
            animator.AnimTime = labelDurationIn;
            animator.AnimateOpacityTo(1f);
        }
        foreach (TmpOpacityAnimation animator in catTextAnimators)
        {
            animator.AnimTime = labelDurationIn;
            animator.AnimateOpacityTo(1f);
        }

        acceptButton.interactable = true;
        acceptButton.GetComponent<ChangeCursorOnHover>().enabled = true;
        rejectButton.interactable = true;
        rejectButton.GetComponent<ChangeCursorOnHover>().enabled = true;

        yield return new WaitForSeconds(labelDurationIn);

        playbackController.StartPlaying();
        playbackController.ActivateControls();

        animationRoutine = null;
        status = 1;
    }

    public void ToDeactive()
    {
        if (status == 0) { return; }
        if (animationRoutine != null) { StopCoroutine(animationRoutine); }
        animationRoutine = StartCoroutine(ToDeactiveIE());
    }

    private IEnumerator ToDeactiveIE()
    {
        playbackController.DeactivateControls();
        acceptButton.interactable = false;
        acceptButton.GetComponent<ChangeCursorOnHover>().enabled = false;
        rejectButton.interactable = false;
        rejectButton.GetComponent<ChangeCursorOnHover>().enabled = false;

        yield return new WaitForSeconds(filterTimingDeactive);

        opacityFilterAnimator.AnimTime = filterDurationDeactive;
        opacityFilterAnimator.AnimateOpacityTo(0.9f);

        yield return new WaitForSeconds(filterDurationDeactive);

        animationRoutine = null;
        status = 0;
    }

    public void ToBlank(bool isEnding)
    {
        if (status == 2) { return; }
        if (animationRoutine != null) { StopCoroutine(animationRoutine); }
        animationRoutine = StartCoroutine(ToBlankIE(isEnding));
    }

    private IEnumerator ToBlankIE(bool isEnding)
    {
        playbackController.DeactivateControls();

        foreach (ImageOpacityAnimation animator in rejectImageAnimators)
        {
            animator.AnimTime = 0.001f;
            animator.AnimateOpacityTo(0f);
        }

        foreach (ShapesOpacityAnimation animator in rejectShapesAnimators)
        {
            animator.AnimTime = 0.001f;
            animator.AnimateOpacityTo(0f);
        }

        foreach (ImageOpacityAnimation animator in acceptImageAnimators)
        {
            animator.AnimTime = 0.001f;
            animator.AnimateOpacityTo(0f);
        }

        foreach (ShapesOpacityAnimation animator in acceptShapesAnimators)
        {
            animator.AnimTime = 0.001f;
            animator.AnimateOpacityTo(0f);
        }

        acceptButton.interactable = false;
        acceptButton.GetComponent<ChangeCursorOnHover>().enabled = false;
        rejectButton.interactable = false;
        rejectButton.GetComponent<ChangeCursorOnHover>().enabled = false;

        playbackController.paused = true;

        foreach (ImageOpacityAnimation animator in catImageAnimators)
        {
            animator.AnimTime = labelDurationIn;
            animator.AnimateOpacityTo(0f);
        }
        foreach (TmpOpacityAnimation animator in catTextAnimators)
        {
            animator.AnimTime = labelDurationIn;
            animator.AnimateOpacityTo(0f);
        }

        yield return new WaitForSeconds(filterTimingBlank);
        
        opacityFilterAnimator.AnimTime = filterDurationBlank;
        opacityFilterAnimator.AnimateOpacityTo(1f);

        ownSizeAnimator.MoveTime = sizeDurationBlank;
        ownSizeAnimator.TargetWidth = ownSizeAnimator.gameObject.GetComponent<RectTransform>().sizeDelta.x;
        ownSizeAnimator.TargetHeight = 720f;
        ownSizeAnimator.MoveTo();

        yield return new WaitForSeconds(filterDurationBlank);

        if (isEnding) { animationRoutine = null; yield break; }

        matchManager.LoadControllerWithNewMatch(assignedControllerIndex);

        status = 2;

        yield return new WaitForSeconds(filterTimingDeactive);

        opacityFilterAnimator.AnimTime = filterDurationDeactive;
        opacityFilterAnimator.AnimateOpacityTo(0.9f);

        yield return new WaitForSeconds(filterDurationDeactive);

        animationRoutine = null;
        status = 0;
    }

    private void CutToBlank()
    {
        playbackController.DeactivateControls();
        opacityFilterAnimator.CutTo(1f);
        ownSizeAnimator.CutTo(new Vector2(ownSizeAnimator.GetComponent<RectTransform>().sizeDelta.x, 720f));

        for (int i = 0; i < catImageAnimators.Length; i++)
        {
            catImageAnimators[i].CutTo(0f);
        }

        for (int i = 0; i < catTextAnimators.Length; i++)
        {
            catTextAnimators[i].CutTo(0f);
        }

        for (int i = 0; i < acceptImageAnimators.Length; i++)
        {
            acceptImageAnimators[i].CutTo(0f);
        }

        for (int i = 0; i < acceptShapesAnimators.Length; i++)
        {
            acceptShapesAnimators[i].CutTo(0f);
        }

        for (int i = 0; i < rejectImageAnimators.Length; i++)
        {
            rejectImageAnimators[i].CutTo(0f);
        }

        for (int i = 0; i < rejectShapesAnimators.Length; i++)
        {
            rejectShapesAnimators[i].CutTo(0f);
        }

        acceptButton.interactable = false;
        acceptButton.GetComponent<ChangeCursorOnHover>().enabled = false;
        rejectButton.interactable = false;
        rejectButton.GetComponent<ChangeCursorOnHover>().enabled = false;

        playbackController.paused = true;
        playbackController.currentFrame = 0;

        animationRoutine = null;
        status = 2;
    }
}
