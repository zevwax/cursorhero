using UnityEngine;

namespace ZevWaxGames.CursorHero
{
    public class Wall : MonoBehaviour
    {
        public enum WallType { Left, Right, Top, Bottom }
        public WallType type;
        
        private BoxCollider2D col;
        private AspectRatioHandler aspectHandler;

        void Awake()
        {
            col = GetComponent<BoxCollider2D>();
            aspectHandler = Camera.main.GetComponent<AspectRatioHandler>();
        }

        void Update()
        {
            if (aspectHandler == null) return;

            float h = aspectHandler.Height * 2;
            float w = aspectHandler.Width * 2;

            switch (type)
            {
                case WallType.Left:
                    transform.position = new Vector2(-aspectHandler.Width - (col.size.x / 2f), 0);
                    col.size = new Vector2(col.size.x, h);
                    break;
                case WallType.Right:
                    transform.position = new Vector2(aspectHandler.Width + (col.size.x / 2f), 0);
                    col.size = new Vector2(col.size.x, h);
                    break;
                case WallType.Top:
                    transform.position = new Vector2(0, aspectHandler.Height + (col.size.y / 2f));
                    col.size = new Vector2(w, col.size.y);
                    break;
                case WallType.Bottom:
                    transform.position = new Vector2(0, -aspectHandler.Height - (col.size.y / 2f));
                    col.size = new Vector2(w, col.size.y);
                    break;
            }
        }
    }
}