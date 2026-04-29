using UnityEngine;

namespace ZevWaxGames.CursorHero
{
    public class Cyan : Enemy
    {
        protected override void Start()
        {
            base.Start();
            speed = 8f; // High speed
        }
    }
}