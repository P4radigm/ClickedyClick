using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Shapes;

public class PerformanceLoader : MonoBehaviour
{
    private RouterManager routerManager;
    private bool isActive = false;

    [SerializeField] private PerformanceLoaderCard firstCard;
    [SerializeField] private float cardAnimationTime;
    [SerializeField] private AnimationCurve sliderCurve;
    private Coroutine sliderRoutine;
    private Coroutine lastCardRoutine;

    public void Initialise()
    {
        routerManager = RouterManager.instance;
        isActive = true;
        firstCard.AnimateToHighlighted(cardAnimationTime);
    }

    public void AnimateSliderToPosition(float endPos)
    {
        if(sliderRoutine != null) { StopCoroutine(sliderRoutine); }
        sliderRoutine = StartCoroutine(AnimateSliderIE(endPos)); 
    }

    private IEnumerator AnimateSliderIE(float endPos)
    {
        float timeValue = 0;
        float startPos = routerManager.progressSlider.value;

        while (timeValue < 1)
        {
            timeValue += Time.deltaTime / cardAnimationTime;
            float evaluatedTimeValue = sliderCurve.Evaluate(timeValue);
            float newValue = Mathf.Lerp(startPos, endPos, evaluatedTimeValue);
            routerManager.SetProgressSlider(newValue);

            yield return null;
        }

        yield return null;
    }

    public void StartLastCard()
    {
        if(lastCardRoutine != null) { return; }
        lastCardRoutine = StartCoroutine(LastCardIE());
    }

    private IEnumerator LastCardIE()
    {
        yield return new WaitForSeconds(cardAnimationTime);
        routerManager.SetProgressIcon(1, true);
        routerManager.LoadExperienceSegment((int)RouterManager.ExperienceSegment.Performance);
    }
}
