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
            float scaleX = worldWidth / 960;
            float scaleY = worldHeight / 540;
            rectTransform.localScale = new Vector3(scaleX, scaleY, 1f);
        }
    }
}