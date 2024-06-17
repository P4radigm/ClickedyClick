using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    public static ColorManager instance;

    public List<ImageColorExecutor> images = new List<ImageColorExecutor>();
    public List<RawImageColorExecutor> rawImages = new List<RawImageColorExecutor>();
    public List<ShapesColorExecutor> shapes = new List<ShapesColorExecutor>();
    public List<TmpColorExecutor> text = new List<TmpColorExecutor>();
    public List<InputFieldColorExecutor> inputFields = new List<InputFieldColorExecutor>();
    public List<ParticleColorExecutor> particles = new List<ParticleColorExecutor>();
    public List<LineRendererColorExecutor> lines = new List<LineRendererColorExecutor>();

    [SerializeField] private int startColorScheme;

    public ColorScheme[] colorSchemes;
    public Color currentBackgroundColor;
    public Color currentMidOneColor;
    public Color currentMidTwoColor;
    public Color currentHighlightColor;
    public Color currentBwColor;

    [SerializeField] private AnimationCurve fadeCurve;
    [SerializeField] private float fadeTime;
    public float FadeTime { get { return fadeTime; } set { fadeTime = value; } }
    private Coroutine fadeRoutine;

    [System.Serializable]
    public class ColorScheme
    {
        public string identifier;
        public Color[] colors; //0 = background, 1 = midone, 2 = midtwo, 3 = highlight, 4 = BW
    }

    [System.Serializable]
    public enum ColorType
    {
        Background,
        MidOne,
        MidTwo,
        Highlight,
        BW
    }

    [ContextMenu("Populate Color Object Lists")]
    private void PopulateColoredObjectLists()
    {
        images = new List<ImageColorExecutor>();
        rawImages = new List<RawImageColorExecutor>();
        shapes = new List<ShapesColorExecutor>();
        text = new List<TmpColorExecutor>();
        inputFields = new List<InputFieldColorExecutor>();
        particles = new List<ParticleColorExecutor>();
        lines = new List<LineRendererColorExecutor>();

        ImageColorExecutor[] imageComponents = Resources.FindObjectsOfTypeAll<ImageColorExecutor>();
        foreach (ImageColorExecutor imageComponent in imageComponents)
        {
            images.Add(imageComponent);
        }

        RawImageColorExecutor[] rawImageComponents = Resources.FindObjectsOfTypeAll<RawImageColorExecutor>();
        foreach (RawImageColorExecutor rawImageComponent in rawImageComponents)
        {
            rawImages.Add(rawImageComponent);
        }

        ShapesColorExecutor[] shapeComponents = Resources.FindObjectsOfTypeAll<ShapesColorExecutor>();
        foreach (ShapesColorExecutor shapeComponent in shapeComponents)
        {
            shapes.Add(shapeComponent);
        }

        TmpColorExecutor[] textComponents = Resources.FindObjectsOfTypeAll<TmpColorExecutor>();
        foreach (TmpColorExecutor textComponent in textComponents)
        {
            text.Add(textComponent);
        }

        InputFieldColorExecutor[] inputComponents = Resources.FindObjectsOfTypeAll<InputFieldColorExecutor>();
        foreach (InputFieldColorExecutor inputComponent in inputComponents)
        {
            inputFields.Add(inputComponent);
        }

        ParticleColorExecutor[] particleComponents = Resources.FindObjectsOfTypeAll<ParticleColorExecutor>();
        foreach (ParticleColorExecutor particleComponent in particleComponents)
        {
            particles.Add(particleComponent);
        }

        LineRendererColorExecutor[] lineRendererComponents = Resources.FindObjectsOfTypeAll<LineRendererColorExecutor>();
        foreach (LineRendererColorExecutor lineRendererComponent in lineRendererComponents)
        {
            lines.Add(lineRendererComponent);
        }
    }

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
        currentBackgroundColor = colorSchemes[startColorScheme].colors[0];
        currentMidOneColor = colorSchemes[startColorScheme].colors[1];
        currentMidTwoColor = colorSchemes[startColorScheme].colors[2];
        currentHighlightColor = colorSchemes[startColorScheme].colors[3];
        currentBwColor = colorSchemes[startColorScheme].colors[4];

        StartCoroutine(ExecuteAfterStart());
        
    }

    private IEnumerator ExecuteAfterStart()
    {
        yield return new WaitForEndOfFrame();
        CutToColorScheme(startColorScheme);
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

    public void StartFadeToColorScheme(int schemeIndex)
    {
        if(fadeRoutine != null) { StopCoroutine(fadeRoutine); }
        fadeRoutine = StartCoroutine(FadeToColorSchemeIE(schemeIndex));
    }

    private IEnumerator FadeToColorSchemeIE(int schemeIndex)
    {
        float timeValue = 0;
        Color startBackgroundColor = currentBackgroundColor;
        Color startMidOneColor = currentMidOneColor;
        Color startMidTwoColor = currentMidTwoColor;
        Color startHighlightColor = currentHighlightColor;
        Color startBwColor = currentBwColor;

        while (timeValue < 1)
        {
            timeValue += Time.deltaTime / fadeTime;
            float evaluatedTimeValue = fadeCurve.Evaluate(timeValue);
            Color newBackgroundColor = Color.Lerp(startBackgroundColor, colorSchemes[schemeIndex].colors[0], evaluatedTimeValue);
            Color newMidOneColor = Color.Lerp(startMidOneColor, colorSchemes[schemeIndex].colors[1], evaluatedTimeValue);
            Color newMidTwoColor = Color.Lerp(startMidTwoColor, colorSchemes[schemeIndex].colors[2], evaluatedTimeValue);
            Color newHighlightColor = Color.Lerp(startHighlightColor, colorSchemes[schemeIndex].colors[3], evaluatedTimeValue);
            Color newBwColor = Color.Lerp(startBwColor, colorSchemes[schemeIndex].colors[4], evaluatedTimeValue);

            Color[] newColorArray = new Color[5];
            newColorArray[0] = newBackgroundColor;
            newColorArray[1] = newMidOneColor;
            newColorArray[2] = newMidTwoColor;
            newColorArray[3] = newHighlightColor;
            newColorArray[4] = newBwColor;

            for (int i = 0; i < images.Count; i++)
            {
                images[i].UpdateColor(newColorArray);
            }

            for (int i = 0; i < rawImages.Count; i++)
            {
                rawImages[i].UpdateColor(newColorArray);
            }

            for (int i = 0; i < shapes.Count; i++)
            {
                shapes[i].UpdateColor(newColorArray);
            }

            for (int i = 0; i < text.Count; i++)
            {
                text[i].UpdateColor(newColorArray);
            }

            for (int i = 0; i < inputFields.Count; i++)
            {
                inputFields[i].UpdateColor(newColorArray);
            }

            for (int i = 0; i < particles.Count; i++)
            {
                particles[i].UpdateColor(newColorArray);
            }

            for (int i = 0; i < lines.Count; i++)
            {
                lines[i].UpdateColor(newColorArray);
            }

            currentBackgroundColor = newBackgroundColor;
            currentMidOneColor = newMidOneColor;
            currentMidTwoColor = newMidTwoColor;
            currentHighlightColor = newHighlightColor;
            currentBwColor = newBwColor;

            yield return null;
        }
        
        fadeRoutine = null;
        
        yield return null;
    }
}
