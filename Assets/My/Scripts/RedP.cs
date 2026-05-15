namespace ZevWaxGames.CursorHero
{
    using UnityEngine;

    public class RedP : Projectile
    {
        protected override void Setup()
        {
            weight = 1f;
            speed = 6f;
            gameObject.layer = LayerMask.NameToLayer("EnemyProjectile");
        }
    }
}