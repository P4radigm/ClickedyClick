using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class MoveAnimation : MonoBehaviour
{
    private RectTransform ownTransform;
    [SerializeField] private bool loop;
    [SerializeField] private bool onStart;
    [SerializeField] private Vector2 onStartTargetLocation;
    [SerializeField] private float moveTime;
    public float MoveTime { get { return moveTime; } set { moveTime = value; } }
    [SerializeField] private float delayRoutine = 0;
    public float DelayRoutine { get { return delayRoutine; } set { delayRoutine = value; } }
    [SerializeField] private AnimationCurve moveCurve;
    private Coroutine moveRoutine;

    private float targetLocationX;
    public float TargetLocationX { get { return targetLocationX; } set { targetLocationX = value; targetLocationSetX = true; } }

    private float targetLocationY;
    public float TargetLocationY { get { return targetLocationY; } set { targetLocationY = value; targetLocationSetY = true; } }

    private bool targetLocationSetX = false;
    private bool targetLocationSetY = false;

    private void Start()
    {
        ownTransform = GetComponent<RectTransform>();
        targetLocationX = ownTransform.anchoredPosition.x;
        targetLocationY = ownTransform.anchoredPosition.y;
        if (onStart) { targetLocationX = onStartTargetLocation.x; targetLocationY = onStartTargetLocation.y; MoveTo(); }
    }

    public void CutTo(Vector2 targetPosition)
    {
        if (ownTransform == null)
        {
            ownTransform = GetComponent<RectTransform>();
        }
        ownTransform.anchoredPosition = targetPosition;
    }

    public void MoveTo()
    {
        if (ownTransform == null)
        { 
            ownTransform = GetComponent<RectTransform>();
            if (!targetLocationSetX) { targetLocationX = ownTransform.anchoredPosition.x; }
            if (!targetLocationSetY) { targetLocationY = ownTransform.anchoredPosition.y; }
        }
        if (moveRoutine != null) { StopCoroutine(moveRoutine); }
        moveRoutine = StartCoroutine(MoveToIE());
    }

    public IEnumerator MoveToIE()
    {
        float timeValue = 0;
        Vector2 startLocation = ownTransform.anchoredPosition;
        Vector2 targetLocation = new Vector2(targetLocationX, targetLocationY);

        while (timeValue < 1)
        {
            timeValue += Time.deltaTime / moveTime;
            float evaluatedTimeValue = moveCurve.Evaluate(timeValue);
            Vector2 newLocation = Vector2.Lerp(startLocation, targetLocation, evaluatedTimeValue);
            ownTransform.anchoredPosition = newLocation;

            yield return null;
        }

        if (loop) { MoveTo(); }

        yield return null;

        moveRoutine = null;
    }
}
