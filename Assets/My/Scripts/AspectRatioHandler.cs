using UnityEngine;

namespace ZevWaxGames.CursorHero
{
    public class AspectRatioHandler : MonoBehaviour
    {
        private Camera cam;
        public float Width { get; private set; }
        public float Height { get; private set; }

        void Awake()
        {
            cam = GetComponent<Camera>();
            UpdateDimensions();
        }

        void Update()
        {
            var targetAspect = 16.0f / 9.0f;
            var windowAspect = Screen.width / (float)Screen.height;
            var scaleHeight = windowAspect / targetAspect;

            if (scaleHeight < 1.0f)
            {
                Rect rect = cam.rect;
                rect.width = 1.0f;
                rect.height = scaleHeight;
                rect.x = 0;
                rect.y = (1.0f - scaleHeight) / 2.0f;
                cam.rect = rect;
            }
            else
            {
                float scaleWidth = 1.0f / scaleHeight;
                Rect rect = cam.rect;
                rect.width = scaleWidth;
                rect.height = 1.0f;
                rect.x = (1.0f - scaleWidth) / 2.0f;
                rect.y = 0;
                cam.rect = rect;
            }

            UpdateDimensions();
        }

        private void UpdateDimensions()
        {
            Height = cam.orthographicSize;
            Width = Height * (16f / 9f);
        }
    }
}