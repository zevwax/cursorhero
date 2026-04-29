using UnityEngine;

namespace ZevWaxGames.CursorHero
{
    public class White : Enemy
    {
        protected override void Start()
        {
            base.Start();
            speed = 2f; // Low speed
        }
    }
}