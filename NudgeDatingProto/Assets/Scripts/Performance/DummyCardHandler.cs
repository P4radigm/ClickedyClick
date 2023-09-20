using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Shapes;

public class DummyCardHandler : MonoBehaviour
{
    public PerformanceSearch performanceSearch;
    [SerializeField] private TextMeshProUGUI cardID;
    [SerializeField] private TextMeshProUGUI cardTitle;
    [SerializeField] private TextMeshProUGUI thumbnailTitle;
    [SerializeField] private TextMeshProUGUI previewTextMain;
    [SerializeField] private TextMeshProUGUI previewTextHighlighted;
    [SerializeField] private TextMeshProUGUI sponsoredText;
    [SerializeField] private Rectangle thumbnail;
    [SerializeField] private Rectangle outline;
    [SerializeField] private Rectangle fill;

    public void UpdateID(string newID)
    {
        cardID.text = newID;
    }

    public void DisplaySponsored(bool display)
    {
        sponsoredText.enabled = display;
    }

    public void Clicked()
    {
        performanceSearch.DisappearObject(this.gameObject);
    }

    public void Recolor(Color highlight, Color fillColored, Color textBW, Color fillBW)
    {
        cardID.color = highlight;
        cardTitle.color = textBW;
        thumbnailTitle.color = highlight;
        previewTextMain.color = textBW;
        previewTextHighlighted.color = highlight;
        sponsoredText.color = highlight;
        thumbnail.Color = fillColored;
        outline.Color = highlight;
        fill.Color = fillBW;
    }
}
