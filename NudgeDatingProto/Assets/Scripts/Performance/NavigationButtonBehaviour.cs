using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NavigationButtonBehaviour : MonoBehaviour
{
    [SerializeField] private Scrollbar scrollBar;
    [SerializeField] private float targetScrollPoition;
    [SerializeField] private float animationDuration;
    [SerializeField] private AnimationCurve animationCurve;
    [SerializeField] private NavigationButtonBehaviour[] navigationButtons;
    private Coroutine animationRoutine;

    public void StopCoroutines()
    {
        if(animationRoutine != null)
        {
            StopCoroutine(animationRoutine);
            animationRoutine = null;
        }
    }

    public void StartNavigate()
    {
        for (int i = 0; i < navigationButtons.Length; i++)
        {
            navigationButtons[i].StopCoroutines();
        }
        animationRoutine = StartCoroutine(NavigationIE());
    }

    private IEnumerator NavigationIE()
    {
        float timeValue = 0;
        float startScrollPosition = scrollBar.value;

        while (timeValue < 1)
        {
            timeValue += Time.deltaTime / animationDuration;
            float evaluatedTimeValue = animationCurve.Evaluate(timeValue);
            float newScrollPosition = Mathf.Lerp(startScrollPosition, targetScrollPoition, evaluatedTimeValue);
            scrollBar.value = newScrollPosition;

            yield return null;
        }

        yield return null;

        animationRoutine = null;
    }
}
