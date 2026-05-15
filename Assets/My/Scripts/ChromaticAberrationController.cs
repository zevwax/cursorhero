using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

namespace ZevWaxGames.CursorHero
{
    public class ChromaticAberrationController : MonoBehaviour
    {
        public static ChromaticAberrationController Instance { get; private set; }

        private Volume volume;
        private ChromaticAberration ca;

        [Header("Settings")] private float min = 0.1f;
        private float max = 0.6f;
        private float duration = 0.7f;
        private void OnEnable()
        {
            EventHolder.OnPCFinished += DoGlitch;
        }
        private void OnDisable()
        {
            EventHolder.OnPCFinished -= DoGlitch;
        }
        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            volume = GetComponent<Volume>();
            
            if (volume.profile.TryGet(out ca)) { }
            else
            {
                Debug.LogError("Эффект Chromatic Aberration не найден!");
            }
        }
        private void DoGlitch()
        {
            StartCoroutine(PulseEffect());
        }
        private IEnumerator PulseEffect()
        {
            yield return LerpIntensity(min, max - 0.1f);
            yield return LerpIntensity(max - 0.1f, max - 0.2f);
            yield return LerpIntensity(max - 0.2f, max);
            yield return LerpIntensity(max, min);
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
}