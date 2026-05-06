using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

namespace ZevWaxGames.CursorHero
{
    public class DJ : MonoBehaviour
    {
        public static DJ Instance { get; private set; }
        private AudioSource musicAS;
        private AudioSource diskAS;
        private AudioSource lmbAS;
        private AudioSource completeAS;
        private AudioSource deathAS;
        private bool weNeedToIncrease;
        private bool weNeedToDecrease;
        private Coroutine activeFade;
        #region Public Static Play Methods
        public static void PlayDisk()
        {
            if (Instance != null && !Instance.diskAS.isPlaying)
                Instance.diskAS.Play();
        }
        public static void PlayLMB()
        {
            if (Instance != null && !Instance.lmbAS.isPlaying)
                Instance.lmbAS.Play();
        }
        public static void PlayComplete()
        {
            if (Instance != null && !Instance.completeAS.isPlaying)
                Instance.completeAS.Play();
        }
        public static void PlayDeath()
        {
            if (Instance != null && !Instance.deathAS.isPlaying)
                Instance.deathAS.Play();
        }
        #endregion
        private void Awake()
        {
            Instance = this;
            musicAS = SetupAudioSource("My/My/Clips/The_Computer_After_School", true);
            diskAS = SetupAudioSource("My/WinXp/Sounds/Windows XP Pop-up Blocked", false);
            lmbAS = SetupAudioSource("My/WinXp/Sounds/Windows XP Menu Command", false);
            completeAS = SetupAudioSource("My/WinXp/Sounds/Windows XP Print complete", false);
            deathAS = SetupAudioSource("My/WinXp/Sounds/Windows XP Critical Stop", false);
            weNeedToDecrease = false;
        }
        private void Update()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
                PlayLMB();
        }
        private AudioSource SetupAudioSource(string path, bool loop)
        {
            var source = gameObject.AddComponent<AudioSource>();
            source.clip = Resources.Load<AudioClip>(path);
            source.loop = loop;
            return source;
        }
        private void OnEnable()
        {
            EventHolder.OnRunStarted += HandleRunStarted;
            EventHolder.OnChoosingStarted += HandleChoosingStarted;
            EventHolder.OnChoosingFinished += HandleChoosingFinished;
            EventHolder.OnPlayerDie += HandlePlayerDie;
        }
        private void OnDisable()
        {
            EventHolder.OnRunStarted -= HandleRunStarted;
            EventHolder.OnChoosingStarted -= HandleChoosingStarted;
            EventHolder.OnChoosingFinished -= HandleChoosingFinished;
            EventHolder.OnPlayerDie -= HandlePlayerDie;
        }
        private void HandleRunStarted()
        {
            weNeedToIncrease = true;
            
            if (!weNeedToDecrease)
            {
                musicAS.Play();
                if (activeFade != null) StopCoroutine(activeFade);
                activeFade = StartCoroutine(FadeVolume(1.0f, () => weNeedToIncrease = false));
            }
        }
        private void HandleChoosingStarted()
        {
            PlayComplete();
            if (activeFade != null) StopCoroutine(activeFade);
            activeFade = StartCoroutine(FadeVolume(0.0f, () => 
            {
                musicAS.Pause();
            }));
        }
        private void HandleChoosingFinished()
        {
            musicAS.UnPause();
            if (activeFade != null) StopCoroutine(activeFade);
            activeFade = StartCoroutine(FadeVolume(1.0f, null));
        }
        private void HandlePlayerDie()
        {
            PlayDeath();
            
            weNeedToDecrease = true;

            if (activeFade != null) StopCoroutine(activeFade);
            activeFade = StartCoroutine(FadeVolume(0.0f, () =>
            {
                weNeedToDecrease = false;
                musicAS.Stop();
            }));
        }
        private IEnumerator FadeVolume(float targetVolume, System.Action onComplete)
        {
            var startVolume = musicAS.volume;
            float timer = 0;

            while (timer < 1.0f)
            {
                timer += Time.deltaTime;
                musicAS.volume = Mathf.Lerp(startVolume, targetVolume, timer);
                yield return null;
            }

            musicAS.volume = targetVolume;
            onComplete?.Invoke();
        }
    }
}