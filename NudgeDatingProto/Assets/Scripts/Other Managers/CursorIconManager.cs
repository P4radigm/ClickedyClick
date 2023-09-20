using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorIconManager : MonoBehaviour
{
    public static CursorIconManager instance = null;

    [SerializeField] private Texture2D[] cursors;
    [SerializeField] private Vector2 offsetVector;
    [HideInInspector] public bool isHovering = false;
    public int targetCursorState;
    public int currentCursorState;
    public bool overriden;
    //0 = pointer (default)
    //1 = index hand (clickable)
    //2 = open hand (grabbable)
    //3 = closed hand (grabbing)
    //4 = i-beam (hovering text)

    public enum CursorStates
    {
        pointer,
        index,
        open,
        closed,
        text
    }

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

    private void Update()
    {
        Cursor.SetCursor(isHovering == false ? cursors[0] : cursors[targetCursorState], offsetVector, CursorMode.Auto);
        currentCursorState = isHovering ? targetCursorState : 0;
        isHovering = false;
    }
}
