using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class APImanager : MonoBehaviour
{
    [System.Serializable]
    public class ProfileData
    {
        public string id;
        public string timeOfAdding;
        public int categoryId;
        public int pixelsScrolled;
        public int secondsHovered;
        public int totalClicks;
        public int totalCharacters;
        public Vector2 screenResolution;
    }
    
    [System.Serializable]
    public class ProfilePlaybackData
    {
        public string id;
        public string timeOfAdding;
        public int categoryId;
        public int pixelsScrolled;
        public float secondsHovered;
        public int totalClicks;
        public int totalCharacters;
        public Vector2 screenResolution;
        public string email;
        public string platform;
        public int sampleRate;
        public float performanceDuration;
        public List<Vector2> cursorPositions = new();
        public List<int> scrollPositions = new();
        public List<string> inputFieldStates = new();
        public List<GenericState> genericStates = new();
    }

    [System.Serializable]
    public class GenericState
    {
        public string id;
        public List<int> state = new();
    }

    [System.Serializable]
    public enum TypeCategory
    {
        WildClicker,
        TenderTyper,
        SteadyScroller,
        LazyHoverer
    }

    public static APImanager instance;

    public ProfileData testProfileData;
    public ProfilePlaybackData testProfilePlaybackData;
    public int standardTimeoutValue;
    public string serverUrl;
    [TextArea] public string jsonStringDump;

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

    private void Start()
    {
        //StartCoroutine(RequestProfileData(DebugProfileDataGet));
        //StartCoroutine(RequestProfilePlaybackData("0", DebugProfilePlaybackDataGet));
    }

    public IEnumerator RequestProfileData(Action<ProfileData> callback)
    {
        string url = $"{serverUrl}/recordings/";

        UnityWebRequest req = UnityWebRequest.Get(url);
        req.timeout = standardTimeoutValue;
        var operation = req.SendWebRequest();

        while (!operation.isDone)
        { 
            yield return null;
        }

        Debug.Log($"APIm: RequestProfileData Webrequest has finished");
        ProfileData returnData = new();

        if (req.result != UnityWebRequest.Result.Success && req.result != UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log($"APIm: Got error with request, result = {req.result}, returnText = {req.downloadHandler.text}");
            req.Dispose();
        }
        else
        {
            Debug.Log($"APIm: Retrieved data succesfully, result = {req.result}, returnText = {req.downloadHandler.text}");
            returnData = (ProfileData)JsonUtility.FromJson(req.downloadHandler.text, typeof(ProfileData));
            req.Dispose();
        }

        callback.Invoke(returnData);
    }

    public IEnumerator RequestProfilePlaybackData(string id, Action<ProfilePlaybackData> callback)
    {
        string url = $"{serverUrl}/recordings/";

        UnityWebRequest req = UnityWebRequest.Get($"{url}{id}");
        req.timeout = standardTimeoutValue;
        var operation = req.SendWebRequest();

        while (!operation.isDone)
        {
            yield return null;
        }

        Debug.Log($"APIm: RequestProfilePlaybackData Webrequest for id:{id} has finished");
        ProfilePlaybackData returnData = new();

        if (req.result != UnityWebRequest.Result.Success && req.result != UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log($"APIm: Got error with request, result = {req.result}, returnText = {req.downloadHandler.text}");
            req.Dispose();
        }
        else
        {
            Debug.Log($"APIm: Retrieved data succesfully, result = {req.result}, returnText = {req.downloadHandler.text}");
            returnData = (ProfilePlaybackData)JsonUtility.FromJson(req.downloadHandler.text, typeof(ProfilePlaybackData));
            //jsonStringDump = req.downloadHandler.text;
            req.Dispose();
        }

        callback.Invoke(returnData);
    }

    public IEnumerator TryUploadProfilePlaybackData(ProfilePlaybackData ownData, Action<UnityWebRequest.Result> callback)
    {
        string url = $"{serverUrl}/recordings/";

        string ownDataJson = JsonUtility.ToJson(ownData);

        UnityWebRequest req = UnityWebRequest.Put($"{url}", ownDataJson);
        req.timeout = standardTimeoutValue;
        var operation = req.SendWebRequest();

        while (!operation.isDone)
        {
            yield return null;
        }

        Debug.Log($"APIm: TryUploadProfilePlaybackData Webrequest for id:{ownData.id} has finished");

        if (req.result != UnityWebRequest.Result.Success && req.result != UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log($"APIm: Got error with upload, result = {req.result}");
            req.Dispose();
        }
        else
        {
            Debug.Log($"APIm: Uploaded data succesfully, result = {req.result}");
            req.Dispose();
        }

        callback.Invoke(req.result);
    }

    private void DebugProfileDataGet(ProfileData dataToPrint)
    {
        testProfileData = dataToPrint;
        Debug.Log($"Got Profile Data: {dataToPrint} and updated testProfileData to match");
    }

    private void DebugProfilePlaybackDataGet(ProfilePlaybackData dataToPrint)
    {
        testProfilePlaybackData = dataToPrint;
        Debug.Log($"Got Profile Playback Data: {dataToPrint} and updated testProfilePlaybackData to match");
    }

    /*
    private void TestSerializeToJson()
    {
        ProfilePlaybackData testToSerialize = new();

        Color randomColor = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
        string hexCode = ColorUtility.ToHtmlStringRGB(randomColor);
        testToSerialize.id = hexCode;
        testToSerialize.timeOfAdding = DateTime.Now.ToUniversalTime().Ticks - DateTime.UnixEpoch.Ticks;
        testToSerialize.categoryId = UnityEngine.Random.Range(0, 4);
        testToSerialize.pixelsScrolled = UnityEngine.Random.Range(60, 200);
        testToSerialize.secondsHovered = UnityEngine.Random.Range(0f, 20f);
        testToSerialize.totalClicks = UnityEngine.Random.Range(0, 200);
        testToSerialize.totalCharacters = UnityEngine.Random.Range(0, 30);
        testToSerialize.email = "Test@Data.com";
        testToSerialize.platform = "Windows";
        testToSerialize.sampleRate = 10;
        testToSerialize.performanceDuration = 45f;
        for (int i = 0; i < (testToSerialize.sampleRate * testToSerialize.performanceDuration); i++)
        {
            testToSerialize.cursorPositions.Add(new Vector2(UnityEngine.Random.Range(0, 1080), UnityEngine.Random.Range(0, 1920)));
            testToSerialize.scrollPositions.Add(UnityEngine.Random.Range(0, 1080));
            testToSerialize.inputFieldStates.Add("Test");
        }
        for (int j = 0; j < 27; j++)
        {
            GenericState randomGenericState = new();
            randomGenericState.id = $"GS-{j}";
            for (int i = 0; i < (testToSerialize.sampleRate * testToSerialize.performanceDuration); i++)
            {
                randomGenericState.state.Add(UnityEngine.Random.Range(0, 2));
            }
            testToSerialize.genericStates.Add(randomGenericState);
        }

        string filePath = Path.Combine(System.IO.Path.GetTempPath(), "Test.json");
        File.WriteAllText(filePath, JsonUtility.ToJson(testToSerialize));
        Debug.Log($"Generated test JSON at: {filePath}");
    }
    */
}
