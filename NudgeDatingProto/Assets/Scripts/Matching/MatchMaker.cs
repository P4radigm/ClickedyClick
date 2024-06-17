using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;

public class MatchMaker : MonoBehaviour
{
    private NewDataManager dataManager;
    private RouterManager routerManager;

    [Header("Options")]
    private int displayCounter = 0;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI ownCatOneText;
    [SerializeField] private TextMeshProUGUI ownCatTwoText;
    [SerializeField] private TextMeshProUGUI ownIdText;
    [SerializeField] private TextMeshProUGUI ownStatOneText;
    [SerializeField] private TextMeshProUGUI ownStatTwoText;
    [SerializeField] private TextMeshProUGUI ownStatThreeText;
    [SerializeField] private TextMeshProUGUI ownStatFourText;
    [SerializeField] private TextMeshProUGUI ownStatFiveText;
    [SerializeField] private TextMeshProUGUI ownStatSixText;

    [SerializeField] private MatchController[] displayControllers;

    [SerializeField] private GameObject[] noMatchesLeftObjects;

    [SerializeField] private GameObject allOutButton;

    public List<string> availableFiles = new List<string>();

    public void Initialise()
    {
        routerManager = RouterManager.instance;
        dataManager = NewDataManager.instance;

        for (int i = 0; i < displayControllers.Length; i++)
        {
            displayControllers[i].gameObject.SetActive(true);
        }

        for (int i = 0; i < displayControllers.Length; i++)
        {
            displayControllers[i].Initialise(i);
        }

        ownCatOneText.text = dataManager.categoryStrings[dataManager.ownProfileData.assignedCategoryId].Split(' ')[0];
        ownCatTwoText.text = dataManager.categoryStrings[dataManager.ownProfileData.assignedCategoryId].Split(' ')[1];
        ownIdText.text = $"user<br><b>#</b>{dataManager.ownProfileData.id.Substring(dataManager.ownProfileData.id.Length - 6)}";
        ownStatOneText.text = dataManager.ownProfileData.totalCharacters.ToString();
        ownStatTwoText.text = dataManager.ownProfileData.totalClicks.ToString();
        ownStatThreeText.text = dataManager.ownProfileData.pixelsScrolled.ToString();
        ownStatFourText.text = dataManager.ownProfileData.secondsHovered.ToString("F1");
        ownStatFiveText.text = $"{Mathf.RoundToInt((float)dataManager.ownProfileData.pixelsSelected / 1000f)} K";
        ownStatSixText.text = dataManager.ownProfileData.pixelsDragged.ToString();

        ScanFolder();

        for (int i = 0; i < displayControllers.Length; i++)
        {
            LoadControllerWithNewMatch(i);
            if(i == displayCounter)
            {
                displayControllers[i].ToActive();
            }
            else
            {
                displayControllers[i].ToDeactive();
            }
        }

        allOutButton.SetActive(false);

        for (int i = 0; i < noMatchesLeftObjects.Length; i++)
        {
            noMatchesLeftObjects[i].SetActive(false);
        }

        displayCounter++;
        displayCounter %= displayControllers.Length;

        routerManager.SetProgressSlider(0.8f);
    }

    public void ScanFolder()
    {
        if (!dataManager.useFakeData)
        {
            string folderPath = Path.Combine(Application.persistentDataPath, $"{dataManager.saveFolderName}");
            string[] files = Directory.GetFiles(folderPath, "*.json");

            foreach (string file in files)
            {
                string filename = Path.GetFileName(file);
                availableFiles.Add(filename);
            }
        }
        else
        {
            foreach (NewDataManager.ProfileData profile in dataManager.fakeProfiles)
            {
                availableFiles.Add(profile.id + ".JSON");
            }
        }
    }

    public void HandleRejection(int controllerIndex)
    {
        displayControllers[controllerIndex].ToBlank(false);
        Debug.Log($"{controllerIndex} controller to blank");
        if(!displayControllers[(controllerIndex + 1) % displayControllers.Length].gameObject.activeInHierarchy)
        {
            allOutButton.SetActive(true);
            return;
        }
        displayControllers[(controllerIndex + 1) % displayControllers.Length].ToActive();
        Debug.Log($"{(controllerIndex + 1) % displayControllers.Length} controller to active");
    }

    public void HandleAccept(int controllerIndex)
    {
        dataManager.matchedProfileData = displayControllers[controllerIndex].loadedProfileData;

        routerManager.SetProgressSlider(1f);
        routerManager.SetProgressIcon(4, true);
        routerManager.LoadExperienceSegment((int)RouterManager.ExperienceSegment.Results);
    }

    public void LoadControllerWithNewMatch(int controllerIndex)
    {
        if(availableFiles.Count == 0)
        {
            Debug.Log("No more unique matches available");
            noMatchesLeftObjects[controllerIndex].SetActive(true);
            displayControllers[controllerIndex].gameObject.SetActive(false);

            //check if all noleft objects are active -> if so activate go through available pool again button
            bool allOut = true;
            for (int i = 0; i < noMatchesLeftObjects.Length; i++)
            {
                if (noMatchesLeftObjects[i].activeInHierarchy)
                {
                    allOut = false;
                }
            }
            if (allOut)
            {
                allOutButton.SetActive(true);
            }

            return;
        }

        int fileIndex = Random.Range(0, availableFiles.Count);
        NewDataManager.ProfileData newMatch = dataManager.GetProfileData(availableFiles[fileIndex].Substring(0, availableFiles[fileIndex].Length - 5));

        displayControllers[controllerIndex].LoadNewMatch(newMatch);

        if (availableFiles.Contains(availableFiles[fileIndex]))
        {
            availableFiles.Remove(availableFiles[fileIndex]);
        }
    }

    public void HandleAllOut()
    {
        Initialise();
    }
}
