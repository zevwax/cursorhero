using UnityEngine;
using TMPro;
using System.Collections;
namespace ZevWaxGames.CursorHero
{
    public class Clock : MonoBehaviour
    {
        public static Clock Instance { get; private set; }
        private TextMeshProUGUI clockText;
        public float ElapsedTime => elapsedTime;
        private float elapsedTime = 0;
        public bool IsRunning => isRunning;
        private bool isRunning = false;
        private bool theEndScreenIsShown = false;
        private float initNextStamp = 60f;
        private float nextStamp = 60f;
        private float gameDuration = 300f;
        private void Awake()
        {
            Instance = this;
            clockText = GetComponent<TextMeshProUGUI>();
        }
        private void OnEnable()
        {
            EventHolder.OnRunStarted += Refresh;
            EventHolder.OnFadingInToPCStarted += HandleOnFadingInToPCStarted;
            EventHolder.OnPCStarted += Resume;
            EventHolder.OnYouWinStarted += Stop;
            EventHolder.OnYouWinFinished += ResumeIfNeeded;
            EventHolder.OnPCFinished += HandleOnPCFinished;
            EventHolder.OnRunFinished += Stop;
        }
        private void OnDisable()
        {
            EventHolder.OnRunStarted -= Refresh;
            EventHolder.OnFadingInToPCStarted -= HandleOnFadingInToPCStarted;
            EventHolder.OnPCStarted -= Resume;
            EventHolder.OnYouWinStarted -= Stop;
            EventHolder.OnYouWinFinished -= ResumeIfNeeded;
            EventHolder.OnPCFinished -= HandleOnPCFinished;
            EventHolder.OnRunFinished -= Stop;
        }
        private void Update()
        {
            if (elapsedTime > nextStamp)
            {
                Stop();
                if (FindObjectsByType<Enemy>(FindObjectsSortMode.None).Length == 0)
                {
                    EventHolder.OnPCFinished?.Invoke();
                    isRunning = false;
                    nextStamp += initNextStamp;
                }
                return;
            }
            
            if (!isRunning) return;

            elapsedTime += Time.deltaTime;
            UpdateClockDisplay();

            if (!theEndScreenIsShown && elapsedTime > gameDuration)
                StartCoroutine(HandleWinWhenReady());
        }
        private IEnumerator HandleWinWhenReady()
        {
            yield return new WaitUntil(() => ConditionManager.Instance.currCondition == ConditionManager.Condition.AfterFightChilling);
            yield return new WaitForSeconds(1f);
            yield return StartCoroutine(MainCharacter.Instance.DoGlitch());
            BlueFace.Instance.InstantFadeOut();
            yield return new WaitForSeconds(2f);
            EventHolder.OnYouWinStarted?.Invoke();
            theEndScreenIsShown = true;
        }
        private void HandleOnPCFinished()
        {
            Stop();
            if (elapsedTime < gameDuration + 0.5f)
                MakeClockBeEmpty();
        }
        private void HandleOnFadingInToPCStarted()
        {
            MakeClockBeFull();
        }
        public void Stop()
        {
            isRunning = false;
        }
        public void Resume()
        {
            isRunning = true;
        }
        public void ResumeIfNeeded()
        {
            if (ConditionManager.Instance.prevCondition == ConditionManager.Condition.Fighting)
                Resume();
        }
        public void Refresh()
        {
            elapsedTime = 0f;
            nextStamp = initNextStamp;
            MakeClockBeFull();
        }
        private void MakeClockBeFull()
        {
            if (gameDuration - 0.5f < elapsedTime && elapsedTime < gameDuration + 0.5f)
                clockText.text = "5:00";
            else if (elapsedTime < gameDuration)
                clockText.text = "1:00";
        }
        private void MakeClockBeEmpty() => clockText.text = "0:00";
        private void UpdateClockDisplay()
        {
            int totalSeconds = Mathf.FloorToInt(elapsedTime);
            
            if (elapsedTime < gameDuration + 0.5f && totalSeconds % 60 == 0) return;

            if (totalSeconds <= gameDuration)
            {
                int secondsInMinute = totalSeconds % 60;
                int displaySeconds = (secondsInMinute == 0) ? 0 : (60 - secondsInMinute);

                clockText.text = string.Format("0:{0:00}", displaySeconds);
            }
            else
            {
                int minutes = totalSeconds / 60;
                int displaySeconds = totalSeconds % 60;

                clockText.text = string.Format("{0}:{1:00}", minutes, displaySeconds);
            }
        }
    }
}