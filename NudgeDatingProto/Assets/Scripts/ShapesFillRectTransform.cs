using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;

[RequireComponent(typeof(Rectangle))]
[RequireComponent(typeof(RectTransform))]
[ExecuteInEditMode]
public class ShapesFillRectTransform : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Rectangle rect;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        rect = GetComponent<Rectangle>();

    }

    private void OnRectTransformDimensionsChange()
    {
        rect.Height = rectTransform.rect.height;
        rect.Width = rectTransform.rect.width;
    }
}
