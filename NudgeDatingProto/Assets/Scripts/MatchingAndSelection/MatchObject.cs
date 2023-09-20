using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using TMPro;
using System;

public class MatchObject : MonoBehaviour
{
    private OwnDataManager dataManager;
    public MatchManager.PotentialMatch displayedMatch;
    public int objectID;
    public int categoryID;

    [SerializeField] private TextMeshProUGUI IDtext;
    [SerializeField] private TextMeshProUGUI matchPercentageText;
    [SerializeField] private TextMeshProUGUI catOne;
    [SerializeField] private TextMeshProUGUI catTwo;
    [SerializeField] private Image cursorDisplay;
    [SerializeField] private Image thumbnailDisplay;
    public BoxCollider2D buttonCollider;
    public Button button;
    public ChangeCursorOnHover hoverController;

    private MatchManager mm;

    public void SetNewMatch(MatchManager.PotentialMatch matchToDisplay, int id, MatchManager matchManager)
    {
        dataManager = OwnDataManager.instance;
        mm = matchManager;
        IDtext.text = $"user <b>#</b>{matchToDisplay.userID}";
        matchPercentageText.text = $"{matchToDisplay.matchPercentage}% Match";
        cursorDisplay.sprite = matchToDisplay.isWindows ? mm.windowsCursor : mm.macCursor;
        thumbnailDisplay.sprite = matchToDisplay.thumbnail;

        catOne.text = dataManager.categoryStrings[matchToDisplay.categoryID].Split(' ')[0];
        catTwo.text = dataManager.categoryStrings[matchToDisplay.categoryID].Split(' ')[1];

        objectID = id;

        displayedMatch = matchToDisplay;
    }

    public void Clicked()
    {
        mm.OpenPopUp(displayedMatch);
    }
}
