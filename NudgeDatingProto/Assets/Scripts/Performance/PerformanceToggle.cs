using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PerformanceToggle : MonoBehaviour
{
    [SerializeField] private bool startState = true;
    [SerializeField] private bool activateOnStart = false;
    [HideInInspector] public bool isOn = true;
    public UnityEvent turnOnEvent;
    public UnityEvent turnOffEvent;

    private void Start()
    {
        if (activateOnStart)
        {
            isOn = !startState;
            Toggle();
        }
        else
        {
            isOn = startState;
        }
    }

    public void Toggle()
    {
        isOn = !isOn;

        if (isOn) { turnOnEvent.Invoke(); }
        else { turnOffEvent.Invoke(); }
    }
}
