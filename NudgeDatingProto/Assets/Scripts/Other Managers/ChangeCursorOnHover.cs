using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(RectTransform))]
public class ChangeCursorOnHover : MonoBehaviour
{
    public CursorIconManager.CursorStates targetCursorState;
    private CursorIconManager iconManager;
    private BoxCollider2D col;
    private RectTransform trans;

    private void Start()
    {
        iconManager = CursorIconManager.instance;
        col = GetComponent<BoxCollider2D>();
        trans = GetComponent<RectTransform>();
        col.size = trans.sizeDelta;
    }

    private void OnMouseOver()
    {
        if (iconManager.overriden) { return; }
        iconManager.targetCursorState = (int)targetCursorState;
        iconManager.isHovering = true;
    }
}
