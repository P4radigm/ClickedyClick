using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static ColorManager;

public class PlaybackColorManager : MonoBehaviour
{
    private ColorManager colorManager;
    //private PlaybackController controller;

    public List<PlaybackImageColorExecutor> images = new List<PlaybackImageColorExecutor>();
    public List<PlaybackRawImageColorExecutor> rawImages = new List<PlaybackRawImageColorExecutor>();
    public List<PlaybackShapesColorExecutor> shapes = new List<PlaybackShapesColorExecutor>();
    public List<PlaybackTmpColorExecutor> text = new List<PlaybackTmpColorExecutor>();
    public List<PlaybackInputFieldColorExecutor> inputFields = new List<PlaybackInputFieldColorExecutor>();
    public List<PlaybackParticleColorExecutor> particles = new List<PlaybackParticleColorExecutor>();
    public List<PlaybackLineRendererColorExecutor> lines = new List<PlaybackLineRendererColorExecutor>();

    [SerializeField] private int lightColorScheme;
    [SerializeField] private int darkColorScheme;
    [SerializeField] private TogglePlayback toggleToFollow;
    private bool lastToggleStatus = false;

    private ColorScheme[] colorSchemes;

    public Color currentBackgroundColor;
    public Color currentMidOneColor;
    public Color currentMidTwoColor;
    public Color currentHighlightColor;
    public Color currentBwColor;


    void Start()
    {
        colorManager = ColorManager.instance;
        colorSchemes = colorManager.colorSchemes;
        //controller = GetComponentInParent<PlaybackController>();

        currentBackgroundColor = colorSchemes[lightColorScheme].colors[0];
        currentMidOneColor = colorSchemes[lightColorScheme].colors[1];
        currentMidTwoColor = colorSchemes[lightColorScheme].colors[2];
        currentHighlightColor = colorSchemes[lightColorScheme].colors[3];
        currentBwColor = colorSchemes[lightColorScheme].colors[4];

        StartCoroutine(ExecuteAfterStart());
    }

    [ContextMenu("Populate Color Object Lists")]
    private void PopulateColoredObjectLists()
    {
        images = new List<PlaybackImageColorExecutor>();
        rawImages = new List<PlaybackRawImageColorExecutor>();
        shapes = new List<PlaybackShapesColorExecutor>();
        text = new List<PlaybackTmpColorExecutor>();
        inputFields = new List<PlaybackInputFieldColorExecutor>();
        particles = new List<PlaybackParticleColorExecutor>();
        lines = new List<PlaybackLineRendererColorExecutor>();

        PlaybackImageColorExecutor[] imageComponents = GetComponentsInChildren<PlaybackImageColorExecutor>();
        foreach (PlaybackImageColorExecutor imageComponent in imageComponents)
        {
            images.Add(imageComponent);
        }

        PlaybackRawImageColorExecutor[] rawImageComponents = GetComponentsInChildren<PlaybackRawImageColorExecutor>();
        foreach (PlaybackRawImageColorExecutor rawImageComponent in rawImageComponents)
        {
            rawImages.Add(rawImageComponent);
        }

        PlaybackShapesColorExecutor[] shapeComponents = GetComponentsInChildren<PlaybackShapesColorExecutor>();
        foreach (PlaybackShapesColorExecutor shapeComponent in shapeComponents)
        {
            shapes.Add(shapeComponent);
        }

        PlaybackTmpColorExecutor[] textComponents = GetComponentsInChildren<PlaybackTmpColorExecutor>();
        foreach (PlaybackTmpColorExecutor textComponent in textComponents)
        {
            text.Add(textComponent);
        }

        PlaybackInputFieldColorExecutor[] inputComponents = GetComponentsInChildren<PlaybackInputFieldColorExecutor>();
        foreach (PlaybackInputFieldColorExecutor inputComponent in inputComponents)
        {
            inputFields.Add(inputComponent);
        }

        PlaybackParticleColorExecutor[] particleComponents = GetComponentsInChildren<PlaybackParticleColorExecutor>();
        foreach (PlaybackParticleColorExecutor particleComponent in particleComponents)
        {
            particles.Add(particleComponent);
        }

        PlaybackLineRendererColorExecutor[] lineRendererComponents = GetComponentsInChildren<PlaybackLineRendererColorExecutor>();
        foreach (PlaybackLineRendererColorExecutor lineRendererComponent in lineRendererComponents)
        {
            lines.Add(lineRendererComponent);
        }
    }

    private IEnumerator ExecuteAfterStart()
    {
        yield return new WaitForEndOfFrame();
        CutToColorScheme(lightColorScheme);
    }

    public void CutToColorScheme(int schemeIndex)
    {
        for (int i = 0; i < images.Count; i++)
        {
            images[i].UpdateColor(colorSchemes[schemeIndex].colors);
        }

        for (int i = 0; i < rawImages.Count; i++)
        {
            rawImages[i].UpdateColor(colorSchemes[schemeIndex].colors);
        }

        for (int i = 0; i < shapes.Count; i++)
        {
            shapes[i].UpdateColor(colorSchemes[schemeIndex].colors);
        }

        for (int i = 0; i < text.Count; i++)
        {
            text[i].UpdateColor(colorSchemes[schemeIndex].colors);
        }

        for (int i = 0; i < inputFields.Count; i++)
        {
            inputFields[i].UpdateColor(colorSchemes[schemeIndex].colors);
        }

        for (int i = 0; i < particles.Count; i++)
        {
            particles[i].UpdateColor(colorSchemes[schemeIndex].colors);
        }

        for (int i = 0; i < lines.Count; i++)
        {
            lines[i].UpdateColor(colorSchemes[schemeIndex].colors);
        }

        currentBackgroundColor = colorSchemes[schemeIndex].colors[0];
        currentMidOneColor = colorSchemes[schemeIndex].colors[1];
        currentMidTwoColor = colorSchemes[schemeIndex].colors[2];
        currentHighlightColor = colorSchemes[schemeIndex].colors[3];
        currentBwColor = colorSchemes[schemeIndex].colors[4];
    }

    private void Update()
    {
        if(toggleToFollow != null && lastToggleStatus != toggleToFollow.isOn)
        {
            CutToColorScheme(toggleToFollow.isOn ? lightColorScheme : darkColorScheme);
        }

        lastToggleStatus = toggleToFollow.isOn;
    }
}
