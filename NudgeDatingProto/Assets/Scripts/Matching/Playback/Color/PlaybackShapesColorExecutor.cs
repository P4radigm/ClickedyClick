using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;
using System.Linq;

[RequireComponent(typeof(ShapeRenderer))]
public class PlaybackShapesColorExecutor : MonoBehaviour
{
    public ColorManager.ColorType colorType;
    private ShapeRenderer ownShape;

    public void UpdateColor(Color[] newColors)
    {
        if(ownShape == null) { ownShape = GetComponent<ShapeRenderer>(); }

        Color currentColor = ownShape.Color;
        ownShape.Color = new Color(newColors[(int)colorType].r, newColors[(int)colorType].g, newColors[(int)colorType].b, currentColor.a);
    }
}
