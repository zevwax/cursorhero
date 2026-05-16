using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

namespace ZevWaxGames.CursorHero
{
    public class DJ : MonoBehaviour
    {
        public static DJ Instance { get; private set; }
        private AudioSource fightThemeAS;
        private AudioSource upgradeThemeAS;
        private AudioSource recBinThemeAS;
        private AudioSource diskAS;
        private AudioSource lmbAS;
        private AudioSource completeAS;
        private AudioSource deathAS;
        private Coroutine activeFade;
        private Coroutine activeFade2;
        private Coroutine activeFade3;
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
        private IEnumerator PlayDeath()
        {
            yield return new WaitForSeconds(2f);
            Instance.deathAS.Play();
        }
        #endregion
        private void Awake()
        {
            Instance = this;
            Init();
        }
        private void Init()
        {
            fightThemeAS = SetupAudioSource("My/My/Clips/808 VIP Wizard Club", true);
            upgradeThemeAS = SetupAudioSource("My/My/Clips/Ancient Dream", true);
            recBinThemeAS = SetupAudioSource("My/My/Clips/Was It Real", true);
            diskAS = SetupAudioSource("My/WinXp/Sounds/Windows XP Pop-up Blocked", false);
            lmbAS = SetupAudioSource("My/WinXp/Sounds/Windows XP Menu Command", false);
            completeAS = SetupAudioSource("My/WinXp/Sounds/Windows XP Print complete", false);
            deathAS = SetupAudioSource("My/WinXp/Sounds/Windows XP Critical Stop", false);
            
            PreloadClip(fightThemeAS.clip);
            PreloadClip(upgradeThemeAS.clip);
            PreloadClip(recBinThemeAS.clip);
            PreloadClip(diskAS.clip);
            PreloadClip(lmbAS.clip);
            PreloadClip(completeAS.clip);
            PreloadClip(deathAS.clip);
        }
        private void PreloadClip(AudioClip clip)
        {
            if (clip == null) return;
            
            if (clip.loadState != AudioDataLoadState.Loaded)
            {
                clip.LoadAudioData();
            }
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
            EventHolder.OnPCStarted += HandlePCStarted;
            EventHolder.OnChoosingStarted += HandleChoosingStarted;
            EventHolder.OnChoosingFinished += HandleChoosingFinished;
            EventHolder.OnPCFinished += HandlePCFinished;
            EventHolder.OnRunFinished += HandleRunFinished;
        }
        private void OnDisable()
        {
            EventHolder.OnRunStarted -= HandleRunStarted;
            EventHolder.OnPCStarted -= HandlePCStarted;
            EventHolder.OnChoosingStarted -= HandleChoosingStarted;
            EventHolder.OnChoosingFinished -= HandleChoosingFinished;
            EventHolder.OnPCFinished -= HandlePCFinished;
            EventHolder.OnRunFinished -= HandleRunFinished;
        }
        private void HandleRunStarted()
        {
            if (activeFade != null) StopCoroutine(activeFade);
            Reset(fightThemeAS);
            
            if (activeFade2 != null) StopCoroutine(activeFade2);
            Reset(upgradeThemeAS);
            
            if (activeFade3 != null) StopCoroutine(activeFade3);
            Reset(recBinThemeAS);
        }
        private void HandlePCStarted()
        {
            FadeOut3();
            FadeIn();
        }
        private void HandleChoosingStarted()
        {
            PlayComplete();
            
            FadeOut();
            FadeOut3();
            FadeIn2();
        }
        private void HandleChoosingFinished()
        {
            FadeOut2();
            if (G.Instance.bin.IsEnabled)
                FadeIn3();
            else
                FadeIn();
        }
        private void HandlePCFinished()
        {
            FadeOut();
            FadeIn3();
        }
        private void HandleRunFinished()
        {
            FadeOut();
            FadeOut2();
            FadeOut3();
            StartCoroutine(PlayDeath());
        }
        private void Reset(AudioSource aSource)
        {
            aSource.Play();
            aSource.Pause();
            aSource.volume = 0f;
        }
        private void FadeIn()
        {
            fightThemeAS.UnPause();
            if (activeFade != null) StopCoroutine(activeFade);
            activeFade = StartCoroutine
            (
                FadeVolume
                (
                    1.0f,
                    null
                )
            );
        }
        private void FadeOut()
        {
            if (activeFade != null) StopCoroutine(activeFade);
            activeFade = StartCoroutine
            (
                FadeVolume
                (
                    0.0f,
                    () => fightThemeAS.Pause()
                )
            );
        }
        private void FadeIn2()
        {
            upgradeThemeAS.UnPause();
            if (activeFade2 != null) StopCoroutine(activeFade2);
            activeFade2 = StartCoroutine
            (
                FadeVolume2
                (
                    1.0f,
                    null
                )
            );
        }
        private void FadeOut2()
        {
            if (activeFade2 != null) StopCoroutine(activeFade2);
            activeFade2 = StartCoroutine
            (
                FadeVolume2
                (
                    0.0f,
                    () => upgradeThemeAS.Pause()
                )
            );
        }
        private void FadeIn3()
        {
            recBinThemeAS.UnPause();
            if (activeFade3 != null) StopCoroutine(activeFade3);
            activeFade3 = StartCoroutine
            (
                FadeVolume3
                (
                    1.0f,
                    null
                )
            );
        }
        private void FadeOut3()
        {
            if (activeFade3 != null) StopCoroutine(activeFade3);
            activeFade3 = StartCoroutine
            (
                FadeVolume3
                (
                    0.0f,
                    () => recBinThemeAS.Pause()
                )
            );
        }
        private IEnumerator FadeVolume(float targetVolume, System.Action onComplete)
        {
            var startVolume = fightThemeAS.volume;
            float timer = 0;

            while (timer < 1.0f)
            {
                timer += Time.deltaTime;
                fightThemeAS.volume = Mathf.Lerp(startVolume, targetVolume, timer);
                yield return null;
            }

            fightThemeAS.volume = targetVolume;
            onComplete?.Invoke();
        }
        private IEnumerator FadeVolume2(float targetVolume, System.Action onComplete)
        {
            var startVolume = upgradeThemeAS.volume;
            float timer = 0;

            while (timer < 1.0f)
            {
                timer += Time.deltaTime;
                upgradeThemeAS.volume = Mathf.Lerp(startVolume, targetVolume, timer);
                yield return null;
            }

            upgradeThemeAS.volume = targetVolume;
            onComplete?.Invoke();
        }
        private IEnumerator FadeVolume3(float targetVolume, System.Action onComplete)
        {
            var startVolume = recBinThemeAS.volume;
            float timer = 0;

            while (timer < 1.0f)
            {
                timer += Time.deltaTime;
                recBinThemeAS.volume = Mathf.Lerp(startVolume, targetVolume, timer);
                yield return null;
            }

            recBinThemeAS.volume = targetVolume;
            onComplete?.Invoke();
        }
    }
}