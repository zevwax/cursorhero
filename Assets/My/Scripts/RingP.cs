namespace ZevWaxGames.CursorHero
{
    using UnityEngine;

    public class RingP : Projectile
    {
        protected override void SetupLayer()
        {
            speed = 1f;
            gameObject.layer = LayerMask.NameToLayer("EnemyProjectile");
        }
    }
}