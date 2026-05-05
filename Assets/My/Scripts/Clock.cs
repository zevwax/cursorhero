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
        private bool isRunning = false;
        private void Awake()
        {
            Instance = this;
            clockText = GetComponent<TextMeshProUGUI>();
        }
        private void OnEnable()
        {
            /*EventHolder.OnRunStarted += Refresh;*/
            EventHolder.OnChoosingStarted += Stop;
            EventHolder.OnChoosingFinished += Resume;
            EventHolder.OnPlayerDie += Stop;
        }
        private void OnDisable()
        {
            /*EventHolder.OnRunStarted -= Refresh;*/
            EventHolder.OnChoosingStarted -= Stop;
            EventHolder.OnChoosingFinished -= Resume;
            EventHolder.OnPlayerDie -= Stop;
        }
        private void Update()
        {
            if (!isRunning) return;

            elapsedTime += Time.deltaTime;
            UpdateClockDisplay();
        }
        public void Stop()
        {
            isRunning = false;
        }
        public void Resume()
        {
            isRunning = true;
        }
        public void Refresh()
        {
            elapsedTime = 0f;
            isRunning = true;
            UpdateClockDisplay();
        }
        private void UpdateClockDisplay()
        {
            int minutes = Mathf.FloorToInt(elapsedTime / 60f);
            int seconds = Mathf.FloorToInt(elapsedTime % 60f);
            clockText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}