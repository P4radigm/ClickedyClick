using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class DropAreaBehaviour : MonoBehaviour
{
    public static DropAreaBehaviour instance;
    public bool mouseInArea = false;
    private BoxCollider2D col;
    private RectTransform trans;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public RectTransform rectTransform;

    void Update()
    {
        if (IsCursorInsideRectTransform(rectTransform))
        {
            // Cursor is inside the RectTransform.
            mouseInArea = true;
        }
        else
        {
            // Cursor is outside the RectTransform.
            mouseInArea = false;
        }
    }

    bool IsCursorInsideRectTransform(RectTransform rt)
    {
        Vector2 localCursor;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, Input.mousePosition, Camera.main, out localCursor);

        Rect rect = new Rect(rt.rect.position, rt.rect.size);
        return rect.Contains(localCursor);
    }
}
