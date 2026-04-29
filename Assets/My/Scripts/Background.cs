using UnityEngine;

namespace ZevWaxGames.CursorHero
{
    public class Background : MonoBehaviour
    {
        private SpriteRenderer sr;
        private AspectRatioHandler aspectHandler;

        void Start()
        {
            sr = GetComponent<SpriteRenderer>();
            aspectHandler = Camera.main.GetComponent<AspectRatioHandler>();
        }

        void Update()
        {
            if (aspectHandler == null || sr == null || sr.sprite == null) return;
            
            float spriteWidth = sr.sprite.bounds.size.x;
            float spriteHeight = sr.sprite.bounds.size.y;
            
            float worldWidth = aspectHandler.Width * 2;
            float worldHeight = aspectHandler.Height * 2;
            
            transform.localScale = new Vector3(
                worldWidth / spriteWidth,
                worldHeight / spriteHeight,
                1
            );
        }
    }
}