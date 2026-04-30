using UnityEngine;

namespace ZevWaxGames.CursorHero
{
    public class Background : MonoBehaviour
    {
        private AspectRatioHandler aspectHandler;

        void Start()
        {
            aspectHandler = Camera.main.GetComponent<AspectRatioHandler>();
        }

        void Update()
        {
            if (aspectHandler == null) return;

            transform.localScale = new Vector3(aspectHandler.Width * 2, aspectHandler.Height * 2, 1);
        }
    }
}