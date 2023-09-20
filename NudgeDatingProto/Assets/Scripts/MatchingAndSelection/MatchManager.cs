using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class MatchManager : MonoBehaviour
{
    [System.Serializable] public class PotentialMatch
    {
        public string userID;
        public int categoryID;
        public int matchPercentage;
        public VideoClip recording;
        public Sprite thumbnail;
        public bool isWindows;
        public string assosiatedEmail;
    }

    public List<PotentialMatch> presetMatches;
    public List<MatchObject> initialisedMatches;
    [SerializeField] private ScrollRect scrollArea;
    [SerializeField] private GameObject matchObjectPrefab;
    [SerializeField] private RectTransform parentTransform;
    [SerializeField] private int xPosMatches;
    [SerializeField] private int yStartPosMatches;
    [SerializeField] private int yDistanceMatches;
    public Sprite macCursor;
    public Sprite windowsCursor;
    [Space(40)]
    [SerializeField] private GameObject popUpMenu;
    [SerializeField] private VideoControls popUpVideoManager;
    [SerializeField] private Image popUpCursorDisplay;
    [SerializeField] private TextMeshProUGUI popUpUserID;
    [SerializeField] private PotentialMatch popUppedMatch;
    [SerializeField] private TextMeshProUGUI popUpCatOne;
    [SerializeField] private TextMeshProUGUI popUpCatTwo;
    [SerializeField] private TextMeshProUGUI ownCatOneText;
    [SerializeField] private TextMeshProUGUI ownCatTwoText;
    [SerializeField] private TextMeshProUGUI ownIdText;
    [SerializeField] private TextMeshProUGUI ownStatOneText;
    [SerializeField] private TextMeshProUGUI ownStatTwoText;
    [SerializeField] private TextMeshProUGUI ownStatThreeText;
    [SerializeField] private TextMeshProUGUI ownStatFourText;
    public PotentialMatch chosenMatch;

    private RouterManager routerManager;
    private OwnDataManager dataManager;

    public void Initialise()
    {
        dataManager = OwnDataManager.instance;
        routerManager = RouterManager.instance;

        ownCatOneText.text = dataManager.categoryStrings[dataManager.currentUserData.categoryId].Split(' ')[0];
        ownCatTwoText.text = dataManager.categoryStrings[dataManager.currentUserData.categoryId].Split(' ')[1];
        ownIdText.text = $"user<br><b>#</b>{dataManager.currentUserData.id.Substring(dataManager.currentUserData.id.Length - 6)}";
        ownStatOneText.text = dataManager.currentUserData.totalCharacters.ToString();
        ownStatTwoText.text = dataManager.currentUserData.totalClicks.ToString();
        ownStatThreeText.text = dataManager.currentUserData.secondsHovered.ToString("F1");
        ownStatFourText.text = dataManager.currentUserData.pixelsScrolled.ToString();

        foreach (PotentialMatch match in presetMatches)
        {
            match.userID = GenerateHexcode();
            match.matchPercentage = Random.Range(82, 100);
            match.categoryID = Random.Range(0, 4);
            //match.isWindows = Random.value  < 0.5f ? true : false;
        }

        SortByPercentageDescending(presetMatches);

        //Spawn objects
        for (int i = 0; i < presetMatches.Count; i++)
        {
            GameObject go = Instantiate(matchObjectPrefab, parentTransform);
            RectTransform rt = go.GetComponent<RectTransform>();
            //Set correct position
            rt.anchoredPosition = new Vector2(xPosMatches, -yStartPosMatches - i * yDistanceMatches);
            MatchObject mo = go.GetComponent<MatchObject>();

            mo.SetNewMatch(presetMatches[i], i, this);
            initialisedMatches.Add(mo);
        }

        scrollArea.content.sizeDelta = new Vector2(scrollArea.content.sizeDelta.x, yStartPosMatches + presetMatches.Count * yDistanceMatches);
        routerManager.SetProgressSlider(0.8f);
    }

    public static void SortByPercentageDescending(List<PotentialMatch> dataList)
    {
        dataList.Sort((x, y) => y.matchPercentage.CompareTo(x.matchPercentage));
    }

    public string GenerateHexcode()
    {
        Color randomColor = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
        string hexCode = ColorUtility.ToHtmlStringRGB(randomColor);
        return hexCode;
    }

    public void OpenPopUp(PotentialMatch matchToDisplay)
    {
        //Disable all colliders on buttons of matches below
        scrollArea.enabled = false;
        for (int i = 0; i < initialisedMatches.Count; i++)
        {
            initialisedMatches[i].button.enabled = false;
            initialisedMatches[i].buttonCollider.enabled = false;
            initialisedMatches[i].hoverController.enabled = false;
        }

        routerManager.SetProgressSlider(0.9f);

        //Set PopUp stuff to display correct info
        popUpVideoManager.player.clip = matchToDisplay.recording;
        popUpVideoManager.VideoPlayerPlay();

        popUpCursorDisplay.sprite = matchToDisplay.isWindows ? windowsCursor : macCursor;
        popUpUserID.text = $"user <b>#</b>{matchToDisplay.userID}";
        popUppedMatch = matchToDisplay;
        popUpCatOne.text = dataManager.categoryStrings[matchToDisplay.categoryID].Split(' ')[0];
        popUpCatTwo.text = dataManager.categoryStrings[matchToDisplay.categoryID].Split(' ')[1];
        popUpMenu.SetActive(true);
    }

    public void ClosePopUp()
    {
        routerManager.SetProgressSlider(0.8f);
        popUpMenu.SetActive(false);
        scrollArea.enabled = true;
        for (int i = 0; i < initialisedMatches.Count; i++)
        {
            initialisedMatches[i].button.enabled = true;
            initialisedMatches[i].buttonCollider.enabled = true;
            initialisedMatches[i].hoverController.enabled = true;
        }
    }

    public void SelectThisMatch()
    {
        chosenMatch = popUppedMatch;

        routerManager.SetProgressSlider(1f);
        routerManager.SetProgressIcon(4, true);
        routerManager.LoadExperienceSegment((int)RouterManager.ExperienceSegment.Results);
    }
}
