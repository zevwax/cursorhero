namespace ZevWaxGames.CursorHero
{
    using UnityEngine;

    public class RingP : Projectile
    {
        protected override void Setup()
        {
            weight = 1f;
            speed = 1f;
            gameObject.layer = LayerMask.NameToLayer("EnemyProjectile");
        }
    }
}