using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Volume))]
public class HueShifter : MonoBehaviour
{
    public static HueShifter Instance { get; private set; }
    [NonSerialized] public float shiftSpeed = 4.5f;

    private Volume globalVolume;
    private ColorAdjustments colorAdjustments;
    private Coroutine shiftCoroutine;
    private bool isShifting = false;

    private void Awake()
    {
        Instance = this;
        globalVolume = GetComponent<Volume>();
        if (globalVolume.profile != null)
        {
            globalVolume.profile.TryGet(out colorAdjustments);
        }
    }

    public void StartShifting()
    {
        if (isShifting || colorAdjustments == null) return;

        isShifting = true;
        shiftCoroutine = StartCoroutine(ShiftHueRoutine());
    }

    public void StopShifting()
    {
        if (!isShifting) return;

        isShifting = false;
        if (shiftCoroutine != null)
        {
            StopCoroutine(shiftCoroutine);
        }
        
        colorAdjustments.hueShift.value = 0;
    }

    private IEnumerator ShiftHueRoutine()
    {
        colorAdjustments.hueShift.overrideState = true;

        while (isShifting)
        {
            float currentHue = colorAdjustments.hueShift.value;
            currentHue += shiftSpeed * Time.deltaTime;

            if (currentHue > 180f)
            {
                currentHue -= 360f; 
            }
            else if (currentHue < -180f)
            {
                currentHue += 360f;
            }

            colorAdjustments.hueShift.value = currentHue;
            yield return null;
        }
    }
}