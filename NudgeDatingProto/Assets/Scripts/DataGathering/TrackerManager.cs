using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TrackerManager : MonoBehaviour
{
    public static TrackerManager Instance;

    [SerializeField] private NewDataManager newDataManager;
    [SerializeField] private int sampleRate;

    public Dictionary<string, List<Vector2>> trackedVectorTwos = new Dictionary<string, List<Vector2>>();
    public Dictionary<string, List<string>> trackedStrings = new Dictionary<string, List<string>>();
    public Dictionary<string, List<int>> trackedInts = new Dictionary<string, List<int>>();

    public UnityEvent sampleEvent;
    private float sampleTimer = 0;

    public float totalDragDistance; //Y
    public int totalScrollDistance; //Y
    public int totalSelectedArea; //Y
    public int totalCharactersTyped; //Y
    public int totalClicks; //Y
    public float totalSecondsHovered; //Y

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        if (newDataManager == null) { newDataManager = NewDataManager.instance; }
    }

    private void Update()
    {
        if(sampleTimer > 0) { sampleTimer -= Time.deltaTime; }
        else
        {
            sampleEvent.Invoke();
            sampleTimer = 1f / sampleRate;
        }

        //Debug
        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    SendDataToDataManager();
        //}
    }

    public void AddTrackedVectorTwo(string uniqueID)
    {
        if (!trackedVectorTwos.ContainsKey(uniqueID))
            trackedVectorTwos.Add(uniqueID, new List<Vector2>());
    }

    public void AddTrackedString(string uniqueID)
    {
        if (!trackedStrings.ContainsKey(uniqueID))
            trackedStrings.Add(uniqueID, new List<string>());
    }

    public void AddTrackedInt(string uniqueID)
    {
        if (!trackedInts.ContainsKey(uniqueID))
            trackedInts.Add(uniqueID, new List<int>());
    }

    public void TrackVectorTwo(string uniqueID, Vector2 value)
    {
        if (trackedVectorTwos.ContainsKey(uniqueID))
            trackedVectorTwos[uniqueID].Add(value);
    }

    public void TrackString(string uniqueID, string value)
    {
        if (trackedStrings.ContainsKey(uniqueID))
            trackedStrings[uniqueID].Add(value);
    }

    public void TrackInt(string uniqueID, int value)
    {
        if (trackedInts.ContainsKey(uniqueID))
            trackedInts[uniqueID].Add(value);
    }

    public void SendDataToDataManager()
    {
        //Tracked objects
        List<string> uniqueIntIDs = new List<string>(trackedInts.Keys);
        for (int i = 0; i < uniqueIntIDs.Count; i++)
        {
            NewDataManager.IntData newIntData = new();
            newIntData.uniqueID = uniqueIntIDs[i];
            newIntData.ints = trackedInts[uniqueIntIDs[i]];
            newDataManager.ownProfileData.intDataLists.Add(newIntData);
        }

        List<string> uniqueStringIDs = new List<string>(trackedStrings.Keys);
        for (int i = 0; i < uniqueStringIDs.Count; i++)
        {
            NewDataManager.StringData newStringData = new();
            newStringData.uniqueID = uniqueStringIDs[i];
            newStringData.strings = trackedStrings[uniqueStringIDs[i]];
            newDataManager.ownProfileData.stringDataLists.Add(newStringData);
        }

        List<string> uniqueVectorTwoIDs = new List<string>(trackedVectorTwos.Keys);
        for (int i = 0; i < uniqueVectorTwoIDs.Count; i++)
        {
            NewDataManager.VectorTwoData newVectorTwoData = new();
            newVectorTwoData.uniqueID = uniqueVectorTwoIDs[i];
            newVectorTwoData.vectorTwos = trackedVectorTwos[uniqueVectorTwoIDs[i]];
            newDataManager.ownProfileData.vectorTwoDataLists.Add(newVectorTwoData);
        }

        //Others
        newDataManager.ownProfileData.sampleRate = sampleRate;
        newDataManager.ownProfileData.totalCharacters = totalCharactersTyped;
        newDataManager.ownProfileData.totalClicks = totalClicks;
        newDataManager.ownProfileData.pixelsScrolled = totalScrollDistance;
        newDataManager.ownProfileData.secondsHovered = totalSecondsHovered;
        newDataManager.ownProfileData.pixelsDragged = Mathf.RoundToInt(totalDragDistance);
        newDataManager.ownProfileData.pixelsSelected = totalSelectedArea;
    }
}
