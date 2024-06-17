using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PlaybackController : MonoBehaviour
{
    public Dictionary<string, List<int>> intDataPoints;
    public Dictionary<string, List<string>> stringDataPoints;
    public Dictionary<string, List<Vector2>> vectorDataPoints;
    public Vector2 PlaybackResolution;

    public int currentFrame;
    public bool paused = true;
    private int maxFrame;

    private float playingTimer = 0;
    private float frameDuration = 0.05f;

    [SerializeField] private MatchController matchController;

    [SerializeField] private Slider progressSlider;
    [SerializeField] private Slider interactiveSlider;

    [SerializeField] private TextMeshProUGUI playbackTimerText;

    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject forwardButton;
    [SerializeField] private GameObject backwardButton;
    
    public void UpdateDataLists(NewDataManager.ProfileData profileData)
    {
        intDataPoints = new();
        stringDataPoints = new();
        vectorDataPoints = new();

        foreach (NewDataManager.IntData data in profileData.intDataLists)
        {
            intDataPoints.Add(data.uniqueID, data.ints);
            Debug.Log($"Added {data.uniqueID} to intDataPoints");
        }
        foreach (NewDataManager.StringData data in profileData.stringDataLists)
        {
            stringDataPoints.Add(data.uniqueID, data.strings);
            Debug.Log($"Added {data.uniqueID} to stringDataPoints");
        }
        foreach (NewDataManager.VectorTwoData data in profileData.vectorTwoDataLists)
        {
            vectorDataPoints.Add(data.uniqueID, data.vectorTwos);
            Debug.Log($"Added {data.uniqueID} to vectorTwoDataPoints");
        }


        KeyValuePair<string, List<int>> firstPair = intDataPoints.First();

        maxFrame = firstPair.Value.Count - 1;
        frameDuration = (1f / profileData.sampleRate);

        PlaybackResolution = profileData.screenResolution;

        paused = true;
        currentFrame = 0;
    }

    private void Update()
    {
        pauseButton.SetActive(paused ? false : true);
        playButton.SetActive(paused ? true : false);

        progressSlider.value = Mathf.Clamp((float)currentFrame / (float)maxFrame, 0f, 1f);

        Playing();

        // Convert timer to TimeSpan
        TimeSpan timeSpan = TimeSpan.FromSeconds((maxFrame * frameDuration) - currentFrame * frameDuration);

        // Format the time as m:ss
        string formattedTime = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);

        playbackTimerText.text = formattedTime;
    }

    public void SkipFrames(int frameAmount)
    {
        if(intDataPoints == null || stringDataPoints == null || vectorDataPoints == null) { return; }

        currentFrame = Mathf.Clamp(currentFrame + frameAmount, 0, maxFrame);
    }

    public void HandleSliderInput(Slider slider)
    {
        currentFrame = Mathf.FloorToInt(Mathf.Clamp01(slider.value) * maxFrame);
    }

    public void StartPlaying()
    {
        paused = false;
    }

    public void StopPlaying()
    {
        paused = true;
    }

    public void ActivateControls()
    {
        pauseButton.GetComponent<Button>().interactable = true;
        playButton.GetComponent<Button>().interactable = true;
        forwardButton.GetComponent<Button>().interactable = true;
        backwardButton.GetComponent<Button>().interactable = true;
        interactiveSlider.GetComponent<Slider>().interactable = true;

        pauseButton.GetComponent<ChangeCursorOnHover>().enabled = true;
        playButton.GetComponent<ChangeCursorOnHover>().enabled = true;
        forwardButton.GetComponent<ChangeCursorOnHover>().enabled = true;
        backwardButton.GetComponent<ChangeCursorOnHover>().enabled = true;
        interactiveSlider.GetComponent<ChangeCursorOnHover>().enabled = true;
    }

    public void DeactivateControls()
    {
        pauseButton.GetComponent<Button>().interactable = false;
        playButton.GetComponent<Button>().interactable = false;
        forwardButton.GetComponent<Button>().interactable = false;
        backwardButton.GetComponent<Button>().interactable = false;
        interactiveSlider.GetComponent<Slider>().interactable = false;

        pauseButton.GetComponent<ChangeCursorOnHover>().enabled = false;
        playButton.GetComponent<ChangeCursorOnHover>().enabled = false;
        forwardButton.GetComponent<ChangeCursorOnHover>().enabled = false;
        backwardButton.GetComponent<ChangeCursorOnHover>().enabled = false;
        interactiveSlider.GetComponent<ChangeCursorOnHover>().enabled = false;
    }

    private void Playing()
    {
        if (paused) { return; }

        if(playingTimer < 0)
        {
            currentFrame = Mathf.Clamp(currentFrame + 1, 0, maxFrame);
            playingTimer = frameDuration;
        }
        else
        {
            playingTimer -= Time.deltaTime;
        }
    }
}
