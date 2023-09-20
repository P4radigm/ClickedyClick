using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public class NewDataManager : MonoBehaviour
{
    public static NewDataManager instance = null;

    //private const string AlphanumericCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    public string saveFolderName;
    public string[] categoryStrings;
    public string[] platforms;
    //public int sampleRate; //amount of times per second we get a datasample out of this to be used during playback
    public float totalPerformanceTime;
    //I should make sure the app runs on a higher framerate than the samplerate otherwise the data is gonna be a bit useless
    public bool dirtyData = false;

    private string ownUniqueID;
    public string displayID;

    private ResultsManager resultsManager;
    private APImanager apimanager;

    [System.Serializable]
    public class IntData
    {
        public string uniqueID;
        public List<int> ints;
    }

    [System.Serializable]
    public class StringData
    {
        public string uniqueID;
        public List<string> strings;
    }

    [System.Serializable]
    public class VectorTwoData
    {
        public string uniqueID;
        public List<Vector2> vectorTwos;
    }

    [System.Serializable]
    public class ProfileData
    {
        public string id;
        public int assignedCategoryId;
        public string platform;
        public int sampleRate;
        public float performanceDuration;
        public List<IntData> intDataLists = new();
        public List<StringData> stringDataLists = new();
        public List<VectorTwoData> vectorTwoDataLists = new();
        public int pixelsScrolled;
        public float secondsHovered;
        public int totalClicks;
        public int totalCharacters;
        public float pixelsDragged;
        public float pixelsSelected;
        public string timeOfAdding;
        public string email;
    }

    [System.Serializable]
    public class Averages
    {
        public int totalDataPoints;
        public float averageClicks;
        public float averageCharacters;
        public float averageScrolled;
        public float averageHovered;
        public float averageSelected;
        public float averageDragged;
    }

    public ProfileData ownProfileData;
    public ProfileData matchedProfileData;
    public Averages currentAverages;

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

        ownUniqueID = Guid.NewGuid().ToString();
        displayID = ownUniqueID.Substring(ownUniqueID.Length - 6);
        Debug.Log($"new player ID = {ownUniqueID}, displayID = {displayID}");
        ownProfileData.id = ownUniqueID;
    }

    private void Start()
    {
        apimanager = APImanager.instance;
        resultsManager = ResultsManager.instance;

        currentAverages = GetAverages();

        ownProfileData.platform = platforms[0]; //Should implement at some point that this actually checks the platform
        ownProfileData.performanceDuration = totalPerformanceTime; //Add settings
    }

    public void UploadCurrentUserToServer()
    {
        //Don't upload when data is dirty in some way
        if (dirtyData) { Debug.LogError($"Tried to add dirty data"); return; }

        //Check if email is valid
        if (!IsValidEmail(ownProfileData.email)) { Debug.Log($"Data contains invalid email: {ownProfileData.email}"); return; }
        //Maybe add a check if all data is complete before uploading?


        //Send to APImanager
        //StartCoroutine(apimanager.TryUploadProfilePlaybackData(currentUserData, resultsManager.HandleAfterUpload));
        //!!!!!!!!!!!!! Have to rewrite API manager at some point to serialise the ProfileData class

        //Save to local folder
        SaveUserDataLocal(ownProfileData);
    }

    public ProfileData GetProfileData(string id)
    {
        ProfileData data = new();
        string filePath = Path.Combine(Application.persistentDataPath, saveFolderName, $"{id}.json");

        string jsonString = File.ReadAllText(filePath);
        data = JsonUtility.FromJson<ProfileData>(jsonString);
        return data;
    }

    public void SaveUserDataLocal(ProfileData dataToSave)
    {
        if (dirtyData) { Debug.LogError($"Tried to add dirty data"); return; }

        //Get current time and add to data
        dataToSave.timeOfAdding = DateTimetoISO8601(DateTime.Now);

        if (!IsValidEmail(dataToSave.email)) { Debug.Log($"Data contains invalid email: {dataToSave.email}"); return; }

        string filePath = Path.Combine(Application.persistentDataPath, saveFolderName, $"{ownProfileData.id}.json");

        string folderPath = Path.Combine(Application.persistentDataPath, saveFolderName);
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        File.WriteAllText(filePath, JsonUtility.ToJson(ownProfileData));
        Debug.Log($"Saved positions to JSON at: {filePath}");
    }

    public static long DateTimeToLong(DateTime dateTime)
    {
        return dateTime.ToUniversalTime().Ticks - DateTime.UnixEpoch.Ticks;
    }

    public static DateTime LongToDateTime(long longValue)
    {
        return new DateTime(DateTime.UnixEpoch.Ticks + longValue, DateTimeKind.Utc);
    }

    public static string DateTimetoISO8601(DateTime dateTime)
    {
        string iso8601String = dateTime.ToString("O");

        Debug.Log("ISO 8601 String: " + iso8601String);

        return iso8601String;
    }

    public static DateTime ISO8601toDateTime(string iso8601String)
    {
        DateTime parsedDateTime;
        bool success = DateTime.TryParseExact(iso8601String, "yyyy-MM-dd'T'HH:mm:ss'Z'",
            System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None,
            out parsedDateTime);

        if (success)
        {
            Debug.Log("Parsed DateTime: " + parsedDateTime);
        }
        else
        {
            Debug.Log("Failed to parse DateTime.");
        }
        return parsedDateTime;
    }

    private bool IsValidEmail(string email)
    {
        string pattern = @"^\s*[a-zA-Z0-9._%+-]+\s*@\s*[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}\s*$";
        return Regex.IsMatch(email, pattern);
    }

    public Averages GetAverages()
    {
        Averages av = new();
        string filePath = Path.Combine(Application.persistentDataPath, $"averages.json");

        string jsonString = File.ReadAllText(filePath);
        av = JsonUtility.FromJson<Averages>(jsonString);
        return av;
    }

    public void AddToAverages()
    {
        if(currentAverages == null)
        {
            currentAverages.averageCharacters = ownProfileData.totalCharacters;
            currentAverages.averageClicks = ownProfileData.totalClicks;
            currentAverages.averageScrolled = ownProfileData.pixelsScrolled;
            currentAverages.averageHovered = ownProfileData.secondsHovered;
            currentAverages.averageSelected = ownProfileData.pixelsSelected;
            currentAverages.averageDragged = ownProfileData.pixelsDragged;
            currentAverages.totalDataPoints = 1;
            SaveNewAverages();
            return;
        }

        currentAverages.averageCharacters = ((currentAverages.averageCharacters * currentAverages.totalDataPoints) + ownProfileData.totalCharacters) / (currentAverages.totalDataPoints + 1);
        currentAverages.averageClicks = ((currentAverages.averageClicks * currentAverages.totalDataPoints) + ownProfileData.totalClicks) / (currentAverages.totalDataPoints + 1);
        currentAverages.averageScrolled = ((currentAverages.averageScrolled * currentAverages.totalDataPoints) + ownProfileData.pixelsScrolled) / (currentAverages.totalDataPoints + 1);
        currentAverages.averageHovered = ((currentAverages.averageHovered * currentAverages.totalDataPoints) + ownProfileData.secondsHovered) / (currentAverages.totalDataPoints + 1);
        currentAverages.averageSelected = ((currentAverages.averageSelected * currentAverages.totalDataPoints) + ownProfileData.pixelsSelected) / (currentAverages.totalDataPoints + 1);
        currentAverages.averageDragged = ((currentAverages.averageDragged * currentAverages.totalDataPoints) + ownProfileData.pixelsDragged) / (currentAverages.totalDataPoints + 1);
        currentAverages.totalDataPoints++;
        SaveNewAverages();
    }

    public void SaveNewAverages()
    {
        string filePath = Path.Combine(Application.persistentDataPath, $"averages.json");
        File.WriteAllText(filePath, JsonUtility.ToJson(currentAverages));
        Debug.Log($"Saved averages to JSON at: {filePath}");
    }
}
