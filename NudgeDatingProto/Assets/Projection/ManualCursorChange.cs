using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public class ManualCursorChange : MonoBehaviour
{
    private CursorIconManager cursorManager;
    private ColorManager colorManager;
    private NewDataManager dataManager;
    [SerializeField] private TrackerVisual trackerManager;
    [SerializeField] private KeyCode pointerKey;
    [SerializeField] private KeyCode openKey;
    [SerializeField] private KeyCode closedKey;
    [SerializeField] private KeyCode textKey;

    [SerializeField] private KeyCode colorSchemeWC;
    [SerializeField] private KeyCode colorSchemePH;
    [SerializeField] private KeyCode colorSchemeTT;
    [SerializeField] private KeyCode colorSchemeLH;
    [SerializeField] private KeyCode colorSchemeDD;
    [SerializeField] private KeyCode colorSchemeSS;

    [SerializeField] private KeyCode triggerFakeScrollNotification;

    private void Start()
    {
        cursorManager = CursorIconManager.instance;
        colorManager = ColorManager.instance;
        dataManager = NewDataManager.instance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(pointerKey))
        {
            cursorManager.targetCursorState = 1;
        }
        else if (Input.GetKeyDown(openKey))
        {
            cursorManager.targetCursorState = 2;
        }
        else if (Input.GetKeyDown(closedKey))
        {
            cursorManager.targetCursorState = 3;
        }
        else if (Input.GetKeyDown(textKey))
        {
            cursorManager.targetCursorState = 4;
        }

        if (Input.GetKeyDown(colorSchemeWC)) { colorManager.CutToColorScheme(0); AssignNewID(); }
        else if (Input.GetKeyDown(colorSchemePH)) { colorManager.CutToColorScheme(1); AssignNewID(); }
        else if (Input.GetKeyDown(colorSchemeTT)) { colorManager.CutToColorScheme(2); AssignNewID(); }
        else if (Input.GetKeyDown(colorSchemeLH)) { colorManager.CutToColorScheme(3); AssignNewID(); }
        else if (Input.GetKeyDown(colorSchemeDD)) { colorManager.CutToColorScheme(4); AssignNewID(); }
        else if (Input.GetKeyDown(colorSchemeSS)) { colorManager.CutToColorScheme(5); AssignNewID(); }

        if (Input.GetKey(pointerKey) || Input.GetKey(openKey) || Input.GetKey(closedKey) || Input.GetKey(textKey))
        {
            cursorManager.isHovering = true;
        }
        else
        {
            cursorManager.isHovering = false;
            cursorManager.targetCursorState = 0;
        }

        if (Input.GetKeyDown(triggerFakeScrollNotification))
        {
            trackerManager.SpawnNotification($"Scrolled for {UnityEngine.Random.Range(300, 3000)} px", 2f, 10f);
        }
    }

    private void AssignNewID()
    {
        string ownUniqueID = Guid.NewGuid().ToString();
        string displayID = ownUniqueID.Substring(ownUniqueID.Length - 6);
        Debug.Log($"new player ID = {ownUniqueID}, displayID = {displayID}");
        dataManager.ownProfileData.id = ownUniqueID;
        trackerManager.SetUserIdTagText(displayID);
    }
}
