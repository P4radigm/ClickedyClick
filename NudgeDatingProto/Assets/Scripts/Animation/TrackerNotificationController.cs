using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

//[RequireComponent(typeof(ImageColorExecutor))]
public class TrackerNotificationController : MonoBehaviour
{
    [SerializeField] private Image background;
    [SerializeField] private TextMeshProUGUI notificationText;
    [SerializeField] private float fadeDuration;
    [SerializeField] private AnimationCurve fadeCurve;
    private Coroutine disappearRoutine = null;
    private float upwardSpeed = 0;
    private RectTransform ownTransform;
    private ColorManager colorManager;
    private ImageColorExecutor iCE;
    private TmpColorExecutor tCE;

    public void SpawnInit(string text, float displayDuration, float upwardMovement)
    {
        colorManager = ColorManager.instance;

        tCE = notificationText.GetComponent<TmpColorExecutor>();
        iCE = background.GetComponent<ImageColorExecutor>();

        background.color = new Color(colorManager.currentMidTwoColor.r, colorManager.currentMidTwoColor.g, colorManager.currentMidTwoColor.b, background.color.a);
        notificationText.color = new Color(colorManager.currentBwColor.r, colorManager.currentBwColor.g, colorManager.currentBwColor.b, notificationText.color.a);

        colorManager.text.Add(tCE);
        colorManager.images.Add(iCE);

        ownTransform = GetComponent<RectTransform>();
        notificationText.text = text;
        upwardSpeed = upwardMovement;
        StartDisappearAnim(displayDuration);
    }

    private void Update()
    {
        ownTransform.anchoredPosition += Time.deltaTime * upwardSpeed * Vector2.up;
    }

    private void StartDisappearAnim(float displayDuration)
    {
        if(disappearRoutine != null) { return; }
        disappearRoutine = StartCoroutine(DisappearIE(displayDuration));
    }

    private IEnumerator DisappearIE(float displayDuration)
    {
        if(displayDuration > fadeDuration)
        {
            yield return new WaitForSeconds(displayDuration - fadeDuration);
        }

        float timeValue = 0;
        float startOpacityBg = background.color.a;
        float startOpacityText = notificationText.color.a;

        while (timeValue < 1)
        {
            timeValue += Time.deltaTime / fadeDuration;
            float evaluatedTimeValue = fadeCurve.Evaluate(timeValue);
            float newOpacityBg = Mathf.Lerp(startOpacityBg, 0, evaluatedTimeValue);
            float newOpacityText = Mathf.Lerp(startOpacityText, 0, evaluatedTimeValue);

            background.color = new Color(background.color.r, background.color.g, background.color.b, newOpacityBg);
            notificationText.color = new Color(notificationText.color.r, notificationText.color.g, notificationText.color.b, newOpacityText);

            yield return null;
        }

        yield return null;

        if (colorManager.text.Contains(tCE)) { colorManager.text.Remove(tCE); }
        if (colorManager.images.Contains(iCE)) { colorManager.images.Remove(iCE); }

        Destroy(this.gameObject);

        disappearRoutine = null;
    }
}
