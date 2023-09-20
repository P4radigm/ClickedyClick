using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TmpOpacityAnimation : MonoBehaviour
{
    private TextMeshProUGUI ownText;
    [SerializeField] private bool loop;
    [SerializeField] private bool onStart;
    [SerializeField] private float onStartTargetOpacity;
    [SerializeField] private float animTime;
    public float AnimTime { get { return animTime; } set { animTime = value; } }
    [SerializeField] private float delayRoutine = 0;
    public float DelayRoutine { get { return delayRoutine; } set { delayRoutine = value; } }
    [SerializeField] private AnimationCurve animCurve;
    private Coroutine animRoutine;

    private void Start()
    {
        ownText = GetComponent<TextMeshProUGUI>();
        if (onStart) { AnimateOpacityTo(onStartTargetOpacity); }
    }

    public void AnimateOpacityTo(float targetOpacity)
    {
        if (ownText == null) { ownText = GetComponent<TextMeshProUGUI>(); }
        if (animRoutine != null) { StopCoroutine(animRoutine); }
        animRoutine = StartCoroutine(AnimateOpacityToIE(targetOpacity));
    }

    public void CutTo(float targetOpacity)
    {
        if (ownText == null) { ownText = GetComponent<TextMeshProUGUI>(); }
        ownText.color = new Color(ownText.color.r, ownText.color.g, ownText.color.b, targetOpacity);
    }

    public IEnumerator AnimateOpacityToIE(float targetOpacity)
    {
        float timeValue = 0;
        float startOpacity = ownText.color.a;

        while (timeValue < 1)
        {
            timeValue += Time.deltaTime / animTime;
            float evaluatedTimeValue = animCurve.Evaluate(timeValue);
            float newOpacity = Mathf.Lerp(startOpacity, targetOpacity, evaluatedTimeValue);
            ownText.color = new Color(ownText.color.r, ownText.color.g, ownText.color.b, newOpacity);

            yield return null;
        }

        if (loop) { AnimateOpacityTo(targetOpacity); }

        yield return null;

        animRoutine = null;
    }
}
