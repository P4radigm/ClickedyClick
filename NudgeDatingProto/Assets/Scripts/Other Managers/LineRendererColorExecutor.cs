using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LineRenderer))]
public class LineRendererColorExecutor : MonoBehaviour
{
    public ColorManager.ColorType colorType;
    private LineRenderer ownLine;

    public void UpdateColor(Color[] newColors)
    {
        if (ownLine == null) { ownLine = GetComponent<LineRenderer>(); }

        Color currentStartColor = ownLine.startColor;
        Color currentEndColor = ownLine.endColor;
        ownLine.startColor = new Color(newColors[(int)colorType].r, newColors[(int)colorType].g, newColors[(int)colorType].b, currentStartColor.a);
        ownLine.endColor = new Color(newColors[(int)colorType].r, newColors[(int)colorType].g, newColors[(int)colorType].b, currentEndColor.a);
    }
}
