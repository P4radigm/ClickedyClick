using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageOpacityAnimation : MonoBehaviour
{
    private Image ownImage;
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
        ownImage = GetComponent<Image>();
        if (onStart) { AnimateOpacityTo(onStartTargetOpacity); }
    }
    public void CutTo(float targetOpacity)
    {
        if (ownImage == null) { ownImage = GetComponent<Image>(); }
        ownImage.color = new Color(ownImage.color.r, ownImage.color.g, ownImage.color.b, targetOpacity);
    }

    public void AnimateOpacityTo(float targetOpacity)
    {
        if (ownImage == null) { ownImage = GetComponent<Image>(); }
        if (animRoutine != null) { StopCoroutine(animRoutine); }
        animRoutine = StartCoroutine(AnimateOpacityToIE(targetOpacity));
    }

    public IEnumerator AnimateOpacityToIE(float targetOpacity)
    {
        float timeValue = 0;
        float startOpacity = ownImage.color.a;

        while (timeValue < 1)
        {
            timeValue += Time.deltaTime / animTime;
            float evaluatedTimeValue = animCurve.Evaluate(timeValue);
            float newOpacity = Mathf.Lerp(startOpacity, targetOpacity, evaluatedTimeValue);
            ownImage.color = new Color(ownImage.color.r, ownImage.color.g, ownImage.color.b, newOpacity);

            yield return null;
        }

        if (loop) { AnimateOpacityTo(targetOpacity); }

        yield return null;

        animRoutine = null;
    }
}
