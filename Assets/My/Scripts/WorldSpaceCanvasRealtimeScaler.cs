using UnityEngine;

namespace ZevWaxGames.CursorHero
{
    [RequireComponent(typeof(Canvas))]
    public class WorldSpaceCanvasRealtimeScaler : MonoBehaviour
    {
        public float mult = 1f;
        [SerializeField] private AspectRatioHandler ratioHandler;
        private RectTransform rectTransform;

        private void Start()
        {
            rectTransform = GetComponent<RectTransform>();
            
            if (ratioHandler == null)
            {
                ratioHandler = Camera.main.GetComponent<AspectRatioHandler>();
            }
        }

        private void Update()
        {
            if (ratioHandler == null || rectTransform == null) return;
            
            float worldHeight = ratioHandler.Height * 2f;
            float worldWidth = ratioHandler.Width * 2f;
            
            float scaleX = worldWidth / 480;
            float scaleY = worldHeight / 270;
            
            rectTransform.localScale = new Vector3(scaleX * mult, scaleY * mult, 1f);
        }
    }
}