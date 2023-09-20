using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class SizeAnimation : MonoBehaviour
{
    private RectTransform ownTransform;
    [SerializeField] private bool loop;
    [SerializeField] private bool onStart;
    [SerializeField] private Vector2 onStartTargetSize;
    [SerializeField] private float moveTime;
    public float MoveTime { get { return moveTime; } set { moveTime = value; } }
    [SerializeField] private float delayRoutine = 0;
    public float DelayRoutine { get { return delayRoutine; } set { delayRoutine = value; } }
    [SerializeField] private AnimationCurve moveCurve;
    private Coroutine moveRoutine;

    private float targetHeight;
    public float TargetHeight { get { return targetHeight; } set { targetHeight = value; targetHeightSet = true; } }

    private float targetWidth;
    public float TargetWidth { get { return targetWidth; } set { targetWidth = value; targetWidthSet = true; } }

    private bool targetHeightSet = false;
    private bool targetWidthSet = false;

    private void Start()
    {
        ownTransform = GetComponent<RectTransform>();
        targetWidth = ownTransform.sizeDelta.x;
        targetHeight = ownTransform.sizeDelta.y;
        if (onStart) { targetWidth = onStartTargetSize.x; targetHeight = onStartTargetSize.y; MoveTo(); }
    }

    public void MoveTo()
    {
        if (ownTransform == null)
        { 
            ownTransform = GetComponent<RectTransform>();
            if (!targetWidthSet) { targetWidth = ownTransform.sizeDelta.x; }
            if (!targetHeightSet) { targetHeight = ownTransform.sizeDelta.y; }
        }
        if (moveRoutine != null) { StopCoroutine(moveRoutine); }
        moveRoutine = StartCoroutine(MoveToIE());
    }

    public void CutTo(Vector2 targetSize)
    {
        if (ownTransform == null)
        {
            ownTransform = GetComponent<RectTransform>();
        }
        ownTransform.sizeDelta = targetSize;
    }

    public IEnumerator MoveToIE()
    {
        float timeValue = 0;
        Vector2 startSize = ownTransform.sizeDelta;
        Vector2 targetSize = new Vector2(targetWidth, targetHeight);

        while (timeValue < 1)
        {
            timeValue += Time.deltaTime / moveTime;
            float evaluatedTimeValue = moveCurve.Evaluate(timeValue);
            Vector2 newSize = Vector2.Lerp(startSize, targetSize, evaluatedTimeValue);
            ownTransform.sizeDelta = newSize;

            yield return null;
        }

        if (loop) { MoveTo(); }

        yield return null;

        moveRoutine = null;
    }
}
