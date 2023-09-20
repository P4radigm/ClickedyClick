using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Video;
using UnityEngine.UI;

public class VideoControls : MonoBehaviour
{

    public VideoPlayer player;
    [SerializeField] private Slider progressSlider;
    [SerializeField] private Slider interactableSlider;
    [SerializeField] private float skipSeconds;

    void Update()
    {
        if (player.frameCount > 0)
        {
            progressSlider.value = (float)player.frame / (float)player.frameCount;
        }

        Mathf.Clamp(progressSlider.value, 0, 1);
    }

    public void HandleInteractableSliderValueChange()
    {
        SkipToPercent(interactableSlider.value);
    }

    public void SkipToPercent(float pct)
    {
        var frame = player.frameCount * pct;
        player.frame = (long)frame;
    }

    public void VideoPlayerPause()
    {
        if (player != null)
            player.Pause();
    }
    public void VideoPlayerPlay()
    {
        if (player != null)
            player.Play();
    }

    public void SkipForwards()
    {
        if (player != null)
        {
            int framesToBeSkipped = (int)Mathf.Clamp((int)player.frame + Mathf.RoundToInt(skipSeconds * player.frameRate), 0, player.frameCount);
            player.frame = framesToBeSkipped;
        }
    }

    public void SkipBackwards()
    {
        if (player != null)
        {
            int framesToBeSkipped = (int)Mathf.Clamp((int)player.frame - Mathf.RoundToInt(skipSeconds * player.frameRate), 0, player.frameCount);
            player.frame = framesToBeSkipped;
        }
    }
}

