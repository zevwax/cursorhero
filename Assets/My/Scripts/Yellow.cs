using UnityEngine;

namespace ZevWaxGames.CursorHero
{
    public class Yellow : Enemy
    {
        protected override void Start()
        {
            base.Start();
            speed = 5f; // Medium speed
        }
    }
}