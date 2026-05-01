using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

namespace ZevWaxGames.CursorHero
{
    public class ProgressBar : MonoBehaviour
    {
        public static ProgressBar Instance { get; private set; }
        
        [Header("Settings")]
        [Range(0, 1)] public float value = 0f;
        public Color blockColor = new Color(0.2f, 0.8f, 0.2f);
        
        [Header("Rainbow Animation")]
        public float cycleDuration = 0.75f;
        public float transitionSpeed = 0.5f;
        public Color deepBlue = new Color(0f, 0f, 0.5f);
        public Color cyan = Color.cyan;
        public Color lightRed = new Color(1f, 0.4f, 0.4f);

        [Header("Spacing")]
        public float blockWidth = 12f;
        public float blockHeightOffset = 4f;
        public float gap = 2f;

        private RectTransform rectTransform;
        private List<Image> blocks = new List<Image>();
        private Sequence rainbowSequence;
        private Tween transitionTween;
        
        private Color rainbowColor;
        private Color displayColor;

        private void Awake()
        {
            Instance = this;
            rectTransform = GetComponent<RectTransform>();
            displayColor = blockColor;
        }

        private void Start()
        {
            CreateBlocks();
        }

        private void Update()
        {
            UpdateProgress();
            HandleRainbowEffect();
        }

        private void CreateBlocks()
        {
            float totalWidth = rectTransform.rect.width;
            float currentX = gap;

            while (currentX + blockWidth + gap <= totalWidth)
            {
                GameObject go = new GameObject("Block", typeof(Image));
                go.transform.SetParent(transform, false);
                
                Image img = go.GetComponent<Image>();
                img.color = blockColor;

                RectTransform rt = go.GetComponent<RectTransform>();
                rt.anchorMin = new Vector2(0, 0);
                rt.anchorMax = new Vector2(0, 1);
                rt.pivot = new Vector2(0, 0.5f);
                
                rt.sizeDelta = new Vector2(blockWidth, -blockHeightOffset);
                rt.anchoredPosition = new Vector2(currentX, 0);

                blocks.Add(img);
                currentX += blockWidth + gap;
            }
        }

        private void UpdateProgress()
        {
            int blocksToShow = Mathf.FloorToInt(value * blocks.Count);
            for (int i = 0; i < blocks.Count; i++)
            {
                blocks[i].enabled = i < blocksToShow;
                blocks[i].color = displayColor;
            }
        }

        private void HandleRainbowEffect()
        {
            if (value >= 1f)
            {
                if (rainbowSequence == null)
                {
                    rainbowColor = deepBlue;
                    rainbowSequence = DOTween.Sequence();
                    rainbowSequence.Append(DOTween.To(() => rainbowColor, x => rainbowColor = x, cyan, cycleDuration / 2).SetEase(Ease.Linear));
                    rainbowSequence.Append(DOTween.To(() => rainbowColor, x => rainbowColor = x, lightRed, cycleDuration / 2).SetEase(Ease.Linear));
                    rainbowSequence.Append(DOTween.To(() => rainbowColor, x => rainbowColor = x, deepBlue, cycleDuration / 2).SetEase(Ease.Linear));
                    rainbowSequence.SetLoops(-1, LoopType.Restart);

                    transitionTween?.Kill();
                    transitionTween = DOTween.To(() => displayColor, x => displayColor = x, rainbowColor, transitionSpeed)
                        .OnUpdate(() => { if(value >= 1f) displayColor = Color.Lerp(displayColor, rainbowColor, Time.deltaTime * 5f); });
                }
                
                // Keep displayColor in sync with the cycling rainbowColor once fully transitioned
                displayColor = Color.Lerp(displayColor, rainbowColor, Time.deltaTime * 10f);
            }
            else
            {
                if (rainbowSequence != null)
                {
                    rainbowSequence.Kill();
                    rainbowSequence = null;

                    transitionTween?.Kill();
                    transitionTween = DOTween.To(() => displayColor, x => displayColor = x, blockColor, transitionSpeed);
                }
            }
        }

        public void SetValue(float newValue)
        {
            value = Mathf.Clamp01(newValue);
        }
    }
}