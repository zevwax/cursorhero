using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace ZevWaxGames.CursorHero
{
    public class ProgressBar : MonoBehaviour
    {
        public static ProgressBar Instance { get; private set; }
        
        [Header("Settings")]
        [Range(0, 1)] public float value = 0f;
        public Color blockColor = new Color(0.2f, 0.8f, 0.2f); // XP Green
        
        [Header("Spacing")]
        public float blockWidth = 12f;
        public float blockHeightOffset = 4f;
        public float gap = 2f;

        private RectTransform rectTransform;
        private List<Image> blocks = new List<Image>();

        private void Awake()
        {
            Instance = this;
            rectTransform = GetComponent<RectTransform>();
        }

        private void Start()
        {
            CreateBlocks();
        }

        private void Update()
        {
            UpdateProgress();
        }

        private void CreateBlocks()
        {
            float totalWidth = rectTransform.rect.width;
            float totalHeight = rectTransform.rect.height;
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
            }
        }

        public void SetValue(float newValue)
        {
            value = Mathf.Clamp01(newValue);
        }
    }
}