using UnityEngine;

namespace ZevWaxGames.CursorHero
{
    [RequireComponent(typeof(Canvas))]
    public class WorldSpaceCanvasOnInitScaler : MonoBehaviour
    {
        [SerializeField] private AspectRatioHandler ratioHandler;
        private RectTransform rectTransform;

        private void Start()
        {
            rectTransform = GetComponent<RectTransform>();
            if (ratioHandler == null)
                ratioHandler = Camera.main.GetComponent<AspectRatioHandler>();
        }

        public void Scale()
        {
            if (ratioHandler == null || rectTransform == null) return;
            float worldHeight = ratioHandler.Height * 2f;
            float worldWidth = ratioHandler.Width * 2f;
            var ppu = 30f;
            float scaleX = worldWidth / (ppu * 16);
            float scaleY = worldHeight / (ppu * 9);
            rectTransform.localScale = new Vector3(scaleX, scaleY, 1f);
        }
    }
}