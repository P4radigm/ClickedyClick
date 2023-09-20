using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Shapes;

public class AdBehvaiour : MonoBehaviour
{
    [SerializeField] private Texture[] patterns;
    [SerializeField] private RawImage image;
    [SerializeField] private Vector2 animationSpeed;
    [SerializeField] private TextMeshProUGUI AdText;
    [SerializeField] private GameObject closeButton;
    [SerializeField] private RectTransform[] possibleButtonSet;
    [SerializeField] private TextCycler adTextCycler;

    public void ToggleCloseClick()
    {
        possibleButtonSet[Random.Range(0, possibleButtonSet.Length)].gameObject.SetActive(true);
        closeButton.GetComponent<Button>().interactable = false;
    }

    public void wrongOption()
    {
        closeButton.GetComponent<Button>().interactable = true;
    }

    public void ResetAd()
    {
        image.texture = patterns[Random.Range(0, patterns.Length)];
        image.enabled = true;
        closeButton.SetActive(true);
        AdText.gameObject.SetActive(true);
        adTextCycler.StartCycling();
    }

    public void DisableAd()
    {
        image.enabled = false;
        closeButton.SetActive(false);
        AdText.gameObject.SetActive(false);
    }

    private void Update()
    {
        image.uvRect = new Rect(image.uvRect.x + Time.deltaTime * animationSpeed.x, image.uvRect.y + Time.deltaTime * animationSpeed.y, image.uvRect.width, image.uvRect.height);
    }
}
