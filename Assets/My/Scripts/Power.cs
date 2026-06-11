using UnityEngine;
namespace ZevWaxGames.CursorHero
{
    public class Power : MonoBehaviour
    {
        public static Power Instance { get; private set; }
        private RectTransform rectTransform;
        private float maxDuration = 39f;
        public float CurrentLeftTime => currentLeftTime;
        private float currentLeftTime;
        private bool isRunning;
        public void IncreaseMaxDuration(float amount)
        {
            maxDuration += amount;
            if (isRunning)
                currentLeftTime += amount;
        }
        private void Awake() => Instance = this;
        private void StartTimer()
        {
            currentLeftTime = maxDuration;
            isRunning = true;
            UpdateVisual(1f);
        }
        private void Start()
        {
            rectTransform = GetComponent<RectTransform>();
            EventHolder.OnRunStarted += StartTimer;
        }
        private void OnDestroy()
        {
            EventHolder.OnRunStarted -= StartTimer;
        }
        private void Update()
        {
            if (!isRunning) return;
            if (Object.FindObjectsByType<Enemy>(FindObjectsSortMode.None).Length <= 0) return;
            
            currentLeftTime -= Time.deltaTime;

            if (currentLeftTime <= 0f)
            {
                currentLeftTime = 0f;
                isRunning = false;
                UpdateVisual(0f);
                EventHolder.OnRunFinished?.Invoke();
                return;
            }

            var progress = currentLeftTime / maxDuration;
            UpdateVisual(progress);
        }
        private void UpdateVisual(float progress)
        {
            var offsetMax = rectTransform.offsetMax;
            var parentWidth = ((RectTransform)rectTransform.parent).rect.width;
            
            offsetMax.x = -parentWidth * (1f - progress);
            rectTransform.offsetMax = offsetMax;
        }
    }
}