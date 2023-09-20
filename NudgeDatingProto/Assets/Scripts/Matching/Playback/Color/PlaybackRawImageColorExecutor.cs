using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class PlaybackRawImageColorExecutor : MonoBehaviour
{
    public ColorManager.ColorType colorType;
    private RawImage ownImage;

    public void UpdateColor(Color[] newColors)
    {
        if (ownImage == null) { ownImage = GetComponent<RawImage>(); }

        Color currentColor = ownImage.color;
        ownImage.color = new Color(newColors[(int)colorType].r, newColors[(int)colorType].g, newColors[(int)colorType].b, currentColor.a);
    }
}
