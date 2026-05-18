using UnityEngine;
using TMPro;
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
        private void Awake()
        {
            Instance = this;
            clockText = GetComponent<TextMeshProUGUI>();
        }
        private void OnEnable()
        {
            /*EventHolder.OnRunStarted += Refresh;*/
            EventHolder.OnPCStarted += Resume;
            EventHolder.OnChoosingStarted += Stop;
            EventHolder.OnChoosingFinished += ResumeIfNeeded;
            EventHolder.OnPCFinished += Stop;
            EventHolder.OnRunFinished += Stop;
        }
        private void OnDisable()
        {
            /*EventHolder.OnRunStarted -= Refresh;*/
            EventHolder.OnPCStarted -= Resume;
            EventHolder.OnChoosingStarted -= Stop;
            EventHolder.OnChoosingFinished -= ResumeIfNeeded;
            EventHolder.OnPCFinished -= Stop;
            EventHolder.OnRunFinished -= Stop;
        }
        private void Update()
        {
            if (elapsedTime > nextStamp)
            {
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

            if (!theEndScreenIsShown && elapsedTime > 300)
            {
                EventHolder.OnChoosingStarted?.Invoke();
                theEndScreenIsShown = true;
            }
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
            UpdateClockDisplay();
        }
        private void UpdateClockDisplay()
        {
            if (elapsedTime < 301f)
            {
                var remainingTime = 300 - elapsedTime;
                int minutes = Mathf.FloorToInt(remainingTime / 60f);
                int seconds = Mathf.FloorToInt(remainingTime % 60f);
                clockText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            }
            else if (301f <= elapsedTime && elapsedTime <= 311f)
            {
                clockText.text = "0:00";
            }
            else
            {
                int minutes = Mathf.FloorToInt(elapsedTime / 60f);
                int seconds = Mathf.FloorToInt(elapsedTime % 60f);
                clockText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            }
        }
    }
}