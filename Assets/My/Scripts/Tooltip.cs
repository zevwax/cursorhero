using UnityEngine;
using TMPro;

namespace ZevWaxGames.CursorHero
{
    public class Tooltip : MonoBehaviour
    {
        public TextMeshProUGUI textComponent;
        public RectTransform rectTransform;
        public float gap = 10f;

        private void Update()
        {
            if (MainCharacter.Instance != null)
                UpdateUIPosition();
        }

        public void UpdateUIPosition()
        {
            var mainCharPos = MainCharacter.Instance.transform.position;
            
            Vector2 screenPos = Camera.main.WorldToScreenPoint(mainCharPos);
            
            Canvas canvas = GetComponentInParent<Canvas>();
            RectTransform canvasRect = canvas.transform as RectTransform;
            
            Camera uiCam = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    canvasRect,
                    screenPos,
                    uiCam,
                    out Vector2 localPoint))
            {
                float offsetX = mainCharPos.x < 0 ? gap : -gap; 
                float offsetY = mainCharPos.y < 0 ? gap : -gap;
                rectTransform.anchoredPosition = localPoint + new Vector2(offsetX, offsetY);
            }
        }
    }
}