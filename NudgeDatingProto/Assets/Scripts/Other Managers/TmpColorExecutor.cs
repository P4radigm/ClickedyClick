using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TmpColorExecutor : MonoBehaviour
{
    public ColorManager.ColorType colorType;
    private TextMeshProUGUI ownText;

    public void UpdateColor(Color[] newColors)
    {
        if (ownText == null) { ownText = GetComponent<TextMeshProUGUI>(); }

        Color currentColor = ownText.color;
        ownText.color = new Color(newColors[(int)colorType].r, newColors[(int)colorType].g, newColors[(int)colorType].b, currentColor.a);
    }
}
