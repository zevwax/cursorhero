using UnityEngine;
using UnityEngine.UI;

namespace ZevWaxGames.CursorHero
{
    [RequireComponent(typeof(Canvas))]
    public class WorldSpaceCanvasRealtimeScaler : MonoBehaviour
    {
        private float ppu = 30f;
        public float mult = 1f;
        [SerializeField] private AspectRatioHandler ratioHandler;
        private RectTransform rectTransform;

        private void Start()
        {
            try
            {
                ppu = transform.GetChild(0).GetComponent<Image>().sprite.pixelsPerUnit;
            }
            catch { }
            
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
            
            float scaleX = worldWidth / (ppu * 16);
            float scaleY = worldHeight / (ppu * 9);
            
            rectTransform.localScale = new Vector3(scaleX * mult, scaleY * mult, 1f);
        }
    }
}