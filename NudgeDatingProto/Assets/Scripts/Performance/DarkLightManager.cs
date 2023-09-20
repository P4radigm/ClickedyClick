using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Shapes;
using TMPro;

public class DarkLightManager : MonoBehaviour
{
    public Color lightHighlightColor;
    public Color lightFillColor;
    public Color lightText;
    public Color lightBackground;
    public Color lightOutline;
    public Color darkHighlightColor;
    public Color darkFillColor;
    public Color darkText;
    public Color darkBackground;
    public Color darkOutline;

    public TextMeshProUGUI[] highlightTextObjects;
    public TextMeshProUGUI[] normalTextObjects;

    public Image[] highlightImages;
    public Image[] fillImages;
    public Image[] backgroundImages;

    public Rectangle[] highlightRect;
    public Rectangle[] fillRect;
    public Rectangle[] backgroundRect;
    public Rectangle[] OutlineRect;

    [SerializeField] private PerformanceSearch performanceSearch;

    public void Toggle(bool isLight)
    {
        for (int i = 0; i < highlightTextObjects.Length; i++)
        {
            highlightTextObjects[i].color = isLight ? lightHighlightColor : darkHighlightColor;
        }

        for (int i = 0; i < normalTextObjects.Length; i++)
        {
            normalTextObjects[i].color = isLight ? lightText : darkText;
        }

        for (int i = 0; i < highlightImages.Length; i++)
        {
            highlightImages[i].color = isLight ? lightHighlightColor : darkHighlightColor;
        }

        for (int i = 0; i < fillImages.Length; i++)
        {
            fillImages[i].color = isLight ? lightFillColor : darkFillColor;
        }

        for (int i = 0; i < backgroundImages.Length; i++)
        {
            backgroundImages[i].color = isLight ? lightBackground : darkBackground;
        }

        for (int i = 0; i < highlightRect.Length; i++)
        {
            highlightRect[i].Color = isLight ? lightHighlightColor : darkHighlightColor;
        }

        for (int i = 0; i < fillRect.Length; i++)
        {
            fillRect[i].Color = isLight ? lightFillColor : darkFillColor;
        }

        for (int i = 0; i < backgroundRect.Length; i++)
        {
            backgroundRect[i].Color = isLight ? lightBackground : darkBackground;
        }

        for (int i = 0; i < OutlineRect.Length; i++)
        {
            OutlineRect[i].Color = isLight ? lightOutline : darkOutline;
        }

        performanceSearch.ToggleLightDark(isLight);
    }
    
}
