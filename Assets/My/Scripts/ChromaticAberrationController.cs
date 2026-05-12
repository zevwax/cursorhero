using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class ChromaticAberrationController : MonoBehaviour
{
    private Volume volume;
    private ChromaticAberration ca;

    [Header("Settings")]
    private float min = 0.1f;
    private float max = 0.6f;
    private float duration = 0.7f;

    private void Start()
    {
        volume = GetComponent<Volume>();
        
        if (volume.profile.TryGet(out ca))
        {
            StartCoroutine(PulseEffect());
        }
        else
        {
            Debug.LogError("Эффект Chromatic Aberration не найден!");
        }
    }

    private IEnumerator PulseEffect()
    {
        while (true)
        {
            yield return LerpIntensity(min, max-0.1f);
            yield return LerpIntensity(max-0.1f, max-0.2f);
            yield return LerpIntensity(max-0.2f, max);
            yield return LerpIntensity(max, min);
            yield return new WaitForSeconds(15);
        }
    }

    private IEnumerator LerpIntensity(float start, float end)
    {
        float time = 0;
        while (time < duration)
        {
            ca.intensity.value = Mathf.Lerp(start, end, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        ca.intensity.value = end;
    }
}