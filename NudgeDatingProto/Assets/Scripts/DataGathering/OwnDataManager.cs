using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Text.RegularExpressions;
using System.Text;
using UnityEditor;

public class OwnDataManager : MonoBehaviour
{
    public static OwnDataManager instance = null;

    private const string AlphanumericCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    public string[] categoryStrings;
    public string[] platforms;
    public int sampleRate; //amount of times per second we get a datasample out of this to be used during playback
    public float totalPerformanceTime;
    //I should make sure the app runs on a higher framerate than the samplerate otherwise the data is gonna be a bit useless
    public APImanager.ProfilePlaybackData currentUserData;
    public bool dirtyData = false;

    private string ownUniqueID;
    public string displayID;

    private ResultsManager resultsManager;
    private APImanager apimanager;

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
        currentUserData.id = ownUniqueID;
    }

    private void Start()
    {
        apimanager = APImanager.instance;
        resultsManager = ResultsManager.instance;

        currentUserData.platform = platforms[0]; //Should implement at some point that this actually checks the platform
    }

    public void UploadCurrentUserToServer()
    {
        //Don't upload when data is dirty in some way
        if (dirtyData) { Debug.LogError($"Tried to add dirty data"); return; }

        //Get current time and add to data
        //currentUserData.timeOfAdding = DateTimeToLong(DateTime.Now);
        currentUserData.timeOfAdding = DateTimetoISO8601(DateTime.Now);
        currentUserData.sampleRate = sampleRate;
        currentUserData.performanceDuration = totalPerformanceTime;

        //Maybe add a check if data is complete before uploading?
        if (!IsValidEmail(currentUserData.email)) { Debug.Log($"Data contains invalid email: {currentUserData.email}"); return; }

        //Send to APImanager
        //StartCoroutine(apimanager.TryUploadProfilePlaybackData(currentUserData, resultsManager.HandleAfterUpload));
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
}
