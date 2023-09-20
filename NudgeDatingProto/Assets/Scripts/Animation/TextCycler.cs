using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextCycler : MonoBehaviour
{
    private TextMeshProUGUI textComponent;
    [TextArea][SerializeField] private string[] texts;
    [SerializeField] private float staticTime;
    [SerializeField] private float inTime;
    [SerializeField] private AnimationCurve inCurve;
    [SerializeField] private float outTime;
    [SerializeField] private AnimationCurve outCurve;
    private Coroutine cycleRoutine;
    private Color textColor;
    private int curIndex;
    [SerializeField] private float minOpacity;
    [SerializeField] private float maxOpacity;

    // Start is called before the first frame update
    private void Start()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
        textColor = textComponent.color;
        StartCycling();
        curIndex = Random.Range(0, texts.Length);
    }

    public void StartCycling()
    { 
        if (cycleRoutine != null) { StopCoroutine(cycleRoutine); }

        textComponent = GetComponent<TextMeshProUGUI>();
        textColor = textComponent.color;

        curIndex++;
        curIndex%=texts.Length;
        textComponent.text = texts[curIndex];
        
        cycleRoutine = StartCoroutine(IETextCycle());
    }

    private IEnumerator IETextCycle()
    {
        float timeValueIn = 0;
        float timeValueOut = 0;
        textComponent.color = new Color(textColor.r, textColor.g, textColor.b, minOpacity);

        while (timeValueIn < 1)
        {
            timeValueIn += Time.deltaTime / inTime;
            float evaluatedTimeValue = inCurve.Evaluate(timeValueIn);
            float newAlpha = Mathf.Lerp(minOpacity, maxOpacity, evaluatedTimeValue);

            textComponent.color = new Color(textColor.r, textColor.g, textColor.b, newAlpha);

            yield return null;
        }

        textComponent.color = new Color(textColor.r, textColor.g, textColor.b, maxOpacity);

        yield return new WaitForSeconds(staticTime);

        while (timeValueOut < 1)
        {
            timeValueOut += Time.deltaTime / outTime;
            float evaluatedTimeValue = outCurve.Evaluate(timeValueOut);
            float newAlpha = Mathf.Lerp(maxOpacity, minOpacity, evaluatedTimeValue);

            textComponent.color = new Color(textColor.r, textColor.g, textColor.b, newAlpha);

            yield return null;
        }

        textComponent.color = new Color(textColor.r, textColor.g, textColor.b, minOpacity);

        cycleRoutine = null;
        StartCycling();
    }
}
