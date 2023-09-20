using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Shapes;

public class AdBehaviour : MonoBehaviour
{
    [Header("patterns")]
    //[SerializeField] private Texture[] patternsTextures;
    [SerializeField] private RawImage patternRawImage;
    [SerializeField] private Vector2 patternAnimationSpeed;
    [Header("text")]
    [SerializeField] private GameObject[] AdTexts;
    [SerializeField] private float textAnimationSpeed;
    [SerializeField] private AnimationCurve textAnimationCurve;
    private float textAnimationTimer = 0;
    private bool isAnimating = true;
    [Header("clicking")]
    [SerializeField] private GameObject onParent;
    [SerializeField] private GameObject textParent;
    [SerializeField] private GameObject clickedParent;
    [SerializeField] private TextMeshProUGUI clickedTextCounter;
    [SerializeField] private float clickedTextDisplayDuration;
    private int clickedAmount;
    private float clickedDisplayTimer;
    private bool isClicked = false;

    private void Update()
    {
        if (!isAnimating) { return; }

        patternRawImage.uvRect = new Rect(patternRawImage.uvRect.x + Time.deltaTime * patternAnimationSpeed.x, patternRawImage.uvRect.y + Time.deltaTime * patternAnimationSpeed.y, patternRawImage.uvRect.width, patternRawImage.uvRect.height);

        if (isClicked)
        {
            if(clickedDisplayTimer > 0)
            {
                clickedDisplayTimer -= Time.deltaTime;
            }
            else
            {
                clickedParent.SetActive(false);
                textParent.SetActive(true);
                textAnimationTimer = 0;
                isClicked = false;
            }
            return;
        }

        float animationValue = textAnimationCurve.Evaluate(textAnimationTimer);
        for (int i = 0; i < AdTexts.Length; i++)
        {
            if ((float)(i+1) / (float)(AdTexts.Length+1) <= animationValue)
            {
                AdTexts[i].SetActive(true);
            }
            else
            {
                AdTexts[i].SetActive(false);
            }
        }

        textAnimationTimer += Time.deltaTime * textAnimationSpeed;
        textAnimationTimer %= 1f;
    }

    public void InitialiseAd()
    {
        onParent.SetActive(true);
        textParent.SetActive(true);
        clickedParent.SetActive(false);
        isAnimating = true;
    }

    public void ToggleCloseClick()
    {
        onParent.SetActive(false);
        textParent.SetActive(false);
        clickedParent.SetActive(false);
        isAnimating = false;
    }

    public void ToggleAdClick()
    {
        isClicked = true;
        clickedAmount++;
        clickedParent.SetActive(true);
        textParent.SetActive(false);
        clickedTextCounter.text = $"{clickedAmount}x";
        clickedDisplayTimer = clickedTextDisplayDuration;
    }
}
