using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RouterManager : MonoBehaviour
{
    public static RouterManager instance = null;
    private TransitionManager transitionManager;
    private Coroutine transitionRoutine = null;

    [SerializeField] private float transitionTime;
    public Slider progressSlider;
    [SerializeField] private GameObject[] progressLabel;
    [Space(20)]
    [SerializeField] private GameObject[] experienceSegments;
    [Space(20)]
    [SerializeField] private PerformanceLoader performanceLoader;
    [SerializeField] private NewPerformanceManager performanceManager;
    [SerializeField] private SelectionLoader selectionLoader;
    public MatchMaker selectionManager;
    [SerializeField] private ResultsManager resultsManager;
    [SerializeField] private TrackerVisual ownTrackerVisual;
    [SerializeField] private NewDataManager newDataManager;

    public ExperienceSegment loadedSegment;

    public enum ExperienceSegment
    {
        Introduction,
        LoadToPerformance,
        Performance,
        LoadToSelection,
        Selection,
        Results,
        About
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        transitionManager = TransitionManager.instance;
        newDataManager = NewDataManager.instance;
        loadedSegment = ExperienceSegment.Introduction;

        ownTrackerVisual.StartInit(newDataManager.displayID);

        SetProgressIcon(0, false);
        SetProgressIcon(1, false);
        SetProgressIcon(2, false);
        SetProgressIcon(3, false);
        SetProgressIcon(4, false);
    }

    

    public void LoadExperienceSegment(int newSegment)
    {
        if(transitionRoutine != null) { StopCoroutine(transitionRoutine); transitionRoutine = null; }
        StartCoroutine(IEtransition(newSegment));
    }

    private IEnumerator IEtransition(int newSegment)
    {
        switch ((ExperienceSegment)newSegment)
        {
            case ExperienceSegment.Introduction:
                transitionManager.TransitionFull(transitionTime);
                break;
            case ExperienceSegment.LoadToPerformance:
                transitionManager.TransitionExludingTimer(transitionTime);
                break;
            case ExperienceSegment.Performance:
                transitionManager.TransitionExludingTimer(transitionTime);
                ColorManager.instance.StartFadeToColorScheme(1);
                ownTrackerVisual.PerformanceInit();
                break;
            case ExperienceSegment.LoadToSelection:
                ColorManager.instance.StartFadeToColorScheme(2);
                transitionManager.TransitionExludingTimer(transitionTime);
                ownTrackerVisual.PerformanceFinished();
                break;
            case ExperienceSegment.Selection:
                transitionManager.TransitionExludingTimer(transitionTime);
                break;
            case ExperienceSegment.Results:
                transitionManager.TransitionExludingTimer(transitionTime);
                break;
            case ExperienceSegment.About:
                transitionManager.TransitionFull(transitionTime);
                break;
            default:
                transitionManager.TransitionFull(transitionTime);
                break;
        }
        yield return new WaitForSeconds(transitionTime/2);
        DisableAllSegments();
        experienceSegments[newSegment].SetActive(true);
        switch ((ExperienceSegment)newSegment)
        {
            case ExperienceSegment.Introduction:
                break;
            case ExperienceSegment.LoadToPerformance:
                performanceLoader.Initialise();
                break;
            case ExperienceSegment.Performance:
                performanceManager.Initialise();
                break;
            case ExperienceSegment.LoadToSelection:
                selectionLoader.Initialise();
                break;
            case ExperienceSegment.Selection:
                selectionManager.Initialise();
                break;
            case ExperienceSegment.Results:
                resultsManager.Initialise();
                break;
            case ExperienceSegment.About:
                break;
            default:
                break;
        }

        yield return new WaitForSeconds(transitionTime / 2);

        loadedSegment = (ExperienceSegment)newSegment;

        transitionRoutine = null;
    }

    public void SetProgressSlider(float newValue)
    {
        progressSlider.value = newValue;
    }

    public void SetProgressIcon(int index, bool colored)
    {
        progressLabel[index].SetActive(colored);
    }

    private void DisableAllSegments()
    {
        foreach (GameObject segment in experienceSegments)
        {
            segment.SetActive(false);
        }
    }
}
