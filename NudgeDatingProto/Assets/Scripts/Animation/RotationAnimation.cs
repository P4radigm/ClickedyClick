using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class RotationAnimation : MonoBehaviour
{
    private RectTransform ownTransform;
    [SerializeField] private bool loop;
    [SerializeField] private bool onStart;
    [SerializeField] private float onStartTargetDegrees;
    [SerializeField] private float rotateTime;
    public float RotateTime { get { return rotateTime; } set { rotateTime = value; } }
    [SerializeField] private float delayRoutine = 0;
    public float DelayRoutine { get { return delayRoutine; } set { delayRoutine = value; } }
    [SerializeField] private AnimationCurve rotateCurve;
    private Coroutine rotateRoutine;

    private void Start()
    {
        ownTransform = GetComponent<RectTransform>();
        if (onStart) { RotateTo(onStartTargetDegrees); }
    }

    public void CutTo(float targetDegrees)
    {
        if (ownTransform == null)
        {
            ownTransform = GetComponent<RectTransform>();
        }
        ownTransform.eulerAngles = new Vector3(0, 0, targetDegrees);
    }

    public void RotateTo(float targetDegrees)
    {
        if (ownTransform == null) { ownTransform = GetComponent<RectTransform>(); }
        if (rotateRoutine != null) { StopCoroutine(rotateRoutine); }
        rotateRoutine = StartCoroutine(RotateToIE(targetDegrees));
    }

    public IEnumerator RotateToIE(float targetDegrees)
    {
        float timeValue = 0;
        float startRotationDegrees = ownTransform.eulerAngles.z;

        while (timeValue < 1)
        {
            timeValue += Time.deltaTime / rotateTime;
            float evaluatedTimeValue = rotateCurve.Evaluate(timeValue);
            float newDegrees = Mathf.Lerp(startRotationDegrees, targetDegrees, evaluatedTimeValue);
            ownTransform.eulerAngles = new Vector3(0, 0, newDegrees);

            yield return null;
        }

        if (loop) { RotateTo(targetDegrees); }

        yield return null;

        rotateRoutine = null;
    }
}
