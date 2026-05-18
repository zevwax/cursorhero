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
        private float VoiceVolume = 0.1f;
        private float MusicVolume = 0.1f;
        public static DJ Instance { get; private set; }
        private AudioSource blueFaceVoiceAS;
        private AudioSource shootAS;
        private AudioSource diskAS;
        private AudioSource lmbAS;
        private AudioSource completeAS;
        private AudioSource deathAS;
        private AudioSource clubRGAS;
        private AudioSource clubBCAS;
        private AudioSource dreamAS;
        private AudioSource realRGAS;
        private AudioSource realBCAS;
        private Coroutine clubRGFadeCoroutine;
        private Coroutine clubBCFadeCoroutine;
        private Coroutine dreamFadeCoroutine;
        private Coroutine realRGFadeCoroutine;
        private Coroutine realBCFadeCoroutine;
        #region Public Static Play Methods
        public static void PlayShoot()
        {
            Instance.shootAS.Play();
        }
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
            shootAS = SetupAudioSource("My/My/Clips/shoot", false);
            diskAS = SetupAudioSource("My/WinXp/Sounds/Windows XP Pop-up Blocked", false);
            lmbAS = SetupAudioSource("My/WinXp/Sounds/Windows XP Menu Command", false);
            completeAS = SetupAudioSource("My/WinXp/Sounds/Windows XP Print complete", false);
            deathAS = SetupAudioSource("My/WinXp/Sounds/Windows XP Critical Stop", false);
            clubRGAS = SetupAudioSource("My/My/Clips/808_VIP_Wizard_Club_RG", true);
            clubBCAS = SetupAudioSource("My/My/Clips/808_VIP_Wizard_Club_BC", true);
            dreamAS = SetupAudioSource("My/My/Clips/Ancient_Dream", true);
            realRGAS = SetupAudioSource("My/My/Clips/Was_It_Real_RG", true);
            realBCAS = SetupAudioSource("My/My/Clips/Was_It_Real_BC", true);
            
            PreloadClip(blueFaceVoiceAS.clip);
            PreloadClip(shootAS.clip);
            PreloadClip(diskAS.clip);
            PreloadClip(lmbAS.clip);
            PreloadClip(completeAS.clip);
            PreloadClip(deathAS.clip);
            PreloadClip(clubRGAS.clip);
            PreloadClip(clubBCAS.clip);
            PreloadClip(dreamAS.clip);
            PreloadClip(realRGAS.clip);
            PreloadClip(realBCAS.clip);
            
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
            if (clubRGFadeCoroutine != null) StopCoroutine(clubRGFadeCoroutine);
            Reset(clubRGAS);
            
            if (clubBCFadeCoroutine != null) StopCoroutine(clubBCFadeCoroutine);
            Reset(clubBCAS);
            
            if (dreamFadeCoroutine != null) StopCoroutine(dreamFadeCoroutine);
            Reset(dreamAS);
            
            if (realRGFadeCoroutine != null) StopCoroutine(realRGFadeCoroutine);
            Reset(realRGAS);
            
            if (realBCFadeCoroutine != null) StopCoroutine(realBCFadeCoroutine);
            Reset(realBCAS);
        }
        private void HandlePCStarted()
        {
            RealRGFadeOut();
            ClubRGFadeIn();
        }
        private void HandleChoosingStarted()
        {
            PlayComplete();
            
            ClubRGFadeOut();
            RealRGFadeOut();
            DreamFadeIn();
        }
        private void HandleChoosingFinished()
        {
            DreamFadeOut();
            if (ConditionManager.Instance.prevCondition == ConditionManager.Condition.AfterFightChilling)
                RealRGFadeIn();
            else if (ConditionManager.Instance.prevCondition == ConditionManager.Condition.Fighting)
                ClubRGFadeIn();
        }
        private void HandlePCFinished()
        {
            ClubRGFadeOut();
            RealRGFadeIn();
        }
        private void HandleRunFinished()
        {
            ClubRGFadeOut();
            DreamFadeOut();
            RealRGFadeOut();
            StartCoroutine(PlayDeath());
        }

        public void HandleGettingDamage()
        {
            if (ConditionManager.Instance.prevCondition == ConditionManager.Condition.AfterFightChilling)
                StartCoroutine(CHandleGettingDuringALimbo());
            else if (ConditionManager.Instance.prevCondition == ConditionManager.Condition.Fighting)
                StartCoroutine(CHandleGettingDuringAFight());
        }
        public IEnumerator CHandleGettingDuringAFight()
        {
            ClubRGInstantFadeOut();
            ClubBCInstantFadeIn();
            yield return new WaitForSeconds(0.75f);
            ClubBCInstantFadeOut();
            ClubRGInstantFadeIn();
        }
        public IEnumerator CHandleGettingDuringALimbo()
        {
            RealRGInstantFadeOut();
            RealBCInstantFadeIn();
            yield return new WaitForSeconds(0.75f);
            RealBCInstantFadeOut();
            RealRGInstantFadeIn();
        }
        private void Reset(AudioSource aSource)
        {
            aSource.Play();
            aSource.DORestart();
            aSource.volume = 0f;
        }
        private void ClubRGFadeIn() => ExecuteFade(ref clubRGFadeCoroutine, clubRGAS, MusicVolume, true);
        private void ClubRGInstantFadeIn() => ExecuteFade(ref clubRGFadeCoroutine, clubRGAS, MusicVolume, true, 0);
        private void ClubRGFadeOut() => ExecuteFade(ref clubRGFadeCoroutine, clubRGAS, 0.0f, false);
        private void ClubRGInstantFadeOut() => ExecuteFade(ref clubRGFadeCoroutine, clubRGAS, 0.0f, false, 0);
        private void ClubBCInstantFadeIn() => ExecuteFade(ref clubBCFadeCoroutine, clubBCAS, MusicVolume, true, 0);
        private void ClubBCInstantFadeOut() => ExecuteFade(ref clubBCFadeCoroutine, clubBCAS, 0.0f, false, 0);
        private void DreamFadeIn() => ExecuteFade(ref dreamFadeCoroutine, dreamAS, MusicVolume, true);
        private void DreamInstantFadeIn() => ExecuteFade(ref dreamFadeCoroutine, dreamAS, MusicVolume, true, 0);
        private void DreamFadeOut() => ExecuteFade(ref dreamFadeCoroutine, dreamAS, 0.0f, false);
        private void DreamInstantFadeOut() => ExecuteFade(ref dreamFadeCoroutine, dreamAS, 0.0f, false, 0);
        private void RealRGFadeIn() => ExecuteFade(ref realRGFadeCoroutine, realRGAS, MusicVolume, true);
        private void RealRGInstantFadeIn() => ExecuteFade(ref realRGFadeCoroutine, realRGAS, MusicVolume, true, 0);
        private void RealRGFadeOut() => ExecuteFade(ref realRGFadeCoroutine, realRGAS, 0.0f, false);
        private void RealRGInstantFadeOut() => ExecuteFade(ref realRGFadeCoroutine, realRGAS, 0.0f, false, 0);
        private void RealBCInstantFadeIn() => ExecuteFade(ref realBCFadeCoroutine, realBCAS, MusicVolume, true, 0);
        private void RealBCInstantFadeOut() => ExecuteFade(ref realBCFadeCoroutine, realBCAS, 0.0f, false, 0);
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