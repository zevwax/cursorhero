using UnityEngine;
using System;
using System.Collections;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine.InputSystem;

namespace ZevWaxGames.CursorHero
{
    public class DJ : MonoBehaviour
    {
        private float VoiceVolume = 0.25f;
        private float MusicVolume = 0.25f;
        public static DJ Instance { get; private set; }
        private AudioSource blueFaceVoiceAS;
        private AudioSource fightThemeAS;
        private AudioSource upgradeThemeAS;
        private AudioSource glitchedThemeAS;
        private AudioSource recBinThemeAS;
        private AudioSource diskAS;
        private AudioSource lmbAS;
        private AudioSource completeAS;
        private AudioSource deathAS;
        private Coroutine activeFade;
        private Coroutine activeFade2;
        private Coroutine activeFade3;
        private Coroutine activeFade4;
        #region Public Static Play Methods
        public static void PlayDisk()
        {
            Instance.diskAS.Play();
        }
        public static void PlayVoice()
        {
            Instance.blueFaceVoiceAS.Play();
        }
        public static void StopVoice()
        {
            Instance.blueFaceVoiceAS.Stop();
        }
        public static void PlayLMB()
        {
            Instance.lmbAS.Play();
        }
        public static void PlayComplete()
        {
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
            blueFaceVoiceAS = SetupAudioSource("My/My/Clips/BlueFaceVoice", false);
            fightThemeAS = SetupAudioSource("My/My/Clips/Sewerslvt - Pretty Cvnt", true);
            upgradeThemeAS = SetupAudioSource("My/My/Clips/Sewerslvt - Pretty Cvnt FK10", true);
            glitchedThemeAS = SetupAudioSource("My/My/Clips/Sewerslvt - Pretty Cvnt BC75", true);
            recBinThemeAS = SetupAudioSource("My/My/Clips/Was It Real", true);
            diskAS = SetupAudioSource("My/WinXp/Sounds/Windows XP Pop-up Blocked", false);
            lmbAS = SetupAudioSource("My/WinXp/Sounds/Windows XP Menu Command", false);
            completeAS = SetupAudioSource("My/WinXp/Sounds/Windows XP Print complete", false);
            deathAS = SetupAudioSource("My/WinXp/Sounds/Windows XP Critical Stop", false);
            
            PreloadClip(blueFaceVoiceAS.clip);
            PreloadClip(fightThemeAS.clip);
            PreloadClip(upgradeThemeAS.clip);
            PreloadClip(glitchedThemeAS.clip);
            PreloadClip(recBinThemeAS.clip);
            PreloadClip(diskAS.clip);
            PreloadClip(lmbAS.clip);
            PreloadClip(completeAS.clip);
            PreloadClip(deathAS.clip);
            
            blueFaceVoiceAS.volume = VoiceVolume;
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
            
            if (activeFade4 != null) StopCoroutine(activeFade4);
            Reset(glitchedThemeAS);
        }
        private void HandlePCStarted()
        {
            FadeOut2();
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
            FadeIn2();
        }
        private void HandleRunFinished()
        {
            FadeOut();
            FadeOut2();
            FadeOut3();
            StartCoroutine(PlayDeath());
        }
        public void HandleGettingDamage() => StartCoroutine(CHandleGettingDamage());
        public IEnumerator CHandleGettingDamage()
        {
            if (G.Instance.bin.IsEnabled)
                InstantFadeOut2();
            else
                InstantFadeOut();
            InstantFadeIn4();
            yield return new WaitForSeconds(0.75f);
            InstantFadeOut4();
            if (G.Instance.bin.IsEnabled)
                InstantFadeIn2();
            else
                InstantFadeIn();
        }
        private void Reset(AudioSource aSource)
        {
            aSource.Play();
            aSource.DORestart();
            aSource.volume = 0f;
        }
        private void FadeIn() => ExecuteFade(ref activeFade, fightThemeAS, MusicVolume, true);
        private void InstantFadeIn() => ExecuteFade(ref activeFade, fightThemeAS, MusicVolume, true, 0);
        private void FadeOut() => ExecuteFade(ref activeFade, fightThemeAS, 0.0f, false);
        private void InstantFadeOut() => ExecuteFade(ref activeFade, fightThemeAS, 0.0f, false, 0);
        private void FadeIn2() => ExecuteFade(ref activeFade2, upgradeThemeAS, MusicVolume, true);
        private void InstantFadeIn2() => ExecuteFade(ref activeFade2, upgradeThemeAS, MusicVolume, true, 0);
        private void FadeOut2() => ExecuteFade(ref activeFade2, upgradeThemeAS, 0.0f, false);
        private void InstantFadeOut2() => ExecuteFade(ref activeFade2, upgradeThemeAS, 0.0f, false, 0);
        private void FadeIn3() => ExecuteFade(ref activeFade3, recBinThemeAS, MusicVolume, true);
        private void FadeOut3() => ExecuteFade(ref activeFade3, recBinThemeAS, 0.0f, false);
        private void InstantFadeIn4() => ExecuteFade(ref activeFade4, glitchedThemeAS, MusicVolume, true, 0);
        private void InstantFadeOut4() => ExecuteFade(ref activeFade4, glitchedThemeAS, 0.0f, false, 0);
        private void ExecuteFade(ref Coroutine activeCor, AudioSource source, float targetVol, bool unpause, float duration = 1.0f)
        {
            //if (unpause) source.UnPause();
            if (activeCor != null) StopCoroutine(activeCor);
            
            //Action onComplete = !unpause ? () => source.Pause() : null;
            activeCor = StartCoroutine(MasterFade(source, targetVol, duration/*, onComplete*/));
        }
        private IEnumerator MasterFade(AudioSource source, float targetVolume, float duration/*, Action onComplete*/)
        {
            float startVolume = source.volume;
            float timer = 0;

            while (timer < duration)
            {
                timer += Time.deltaTime;
                var normalizedTime = duration > 0 ? timer / duration : 1.0f;
                source.volume = Mathf.Lerp(startVolume, targetVolume, normalizedTime);
                yield return null;
            }

            source.volume = targetVolume;
            /*onComplete?.Invoke();*/
        }
    }
}