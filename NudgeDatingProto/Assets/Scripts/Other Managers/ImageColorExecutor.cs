using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageColorExecutor : MonoBehaviour
{
    public ColorManager.ColorType colorType;
    private Image ownImage;

    public void UpdateColor(Color[] newColors)
    {
        if (ownImage == null) { ownImage = GetComponent<Image>(); }

        Color currentColor = ownImage.color;
        ownImage.color = new Color(newColors[(int)colorType].r, newColors[(int)colorType].g, newColors[(int)colorType].b, currentColor.a);
    }
}
