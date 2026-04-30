namespace ZevWaxGames.CursorHero
{
    using UnityEngine;

    public class RingP : Projectile
    {
        protected override void Setup()
        {
            damage = 1f;
            speed = 1f;
            gameObject.layer = LayerMask.NameToLayer("EnemyProjectile");
        }
    }
}