using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightDarkBehaviour : MonoBehaviour
{
    [SerializeField] private ColorManager colorManager;
    [SerializeField] private int colorSchemeLight;
    [SerializeField] private int colorSchemeDark;

    private void Start()
    {
        colorManager = ColorManager.instance;
    }

    public void ToggleLight()
    {
        colorManager.StartFadeToColorScheme(colorSchemeLight);
    }

    public void ToggleDark()
    {
        colorManager.StartFadeToColorScheme(colorSchemeDark);
    }
}
