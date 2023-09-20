using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleColorExecutor : MonoBehaviour
{
    public ColorManager.ColorType colorType;
    private ParticleSystem.MainModule ownParticles;
    bool isSet;

    public void UpdateColor(Color[] newColors)
    {
        if (!isSet) { ownParticles = GetComponent<ParticleSystem>().main; isSet = true; }

        Color currentColor = ownParticles.startColor.color;
        ownParticles.startColor = new Color(newColors[(int)colorType].r, newColors[(int)colorType].g, newColors[(int)colorType].b, currentColor.a);
    }
}
