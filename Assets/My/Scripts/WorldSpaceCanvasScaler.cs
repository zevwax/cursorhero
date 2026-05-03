using UnityEngine;

namespace ZevWaxGames.CursorHero
{
    [RequireComponent(typeof(Canvas))]
    public class WorldSpaceCanvasScaler : MonoBehaviour
    {
        [SerializeField] private AspectRatioHandler ratioHandler;
        private RectTransform rectTransform;

        void Start()
        {
            rectTransform = GetComponent<RectTransform>();
            
            if (ratioHandler == null)
            {
                ratioHandler = Camera.main.GetComponent<AspectRatioHandler>();
            }
        }

        void Update()
        {
            if (ratioHandler == null || rectTransform == null) return;
            
            float worldHeight = ratioHandler.Height * 2f;
            float worldWidth = ratioHandler.Width * 2f;
            
            float scaleX = worldWidth / rectTransform.rect.width;
            float scaleY = worldHeight / rectTransform.rect.height;
            
            rectTransform.localScale = new Vector3(scaleX, scaleY, 1f);
        }
    }
}