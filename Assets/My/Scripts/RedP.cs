namespace ZevWaxGames.CursorHero
{
    using UnityEngine;

    public class RedP : Projectile
    {
        protected override void SetupLayer()
        {
            speed = 6f;
            gameObject.layer = LayerMask.NameToLayer("EnemyProjectile");
        }
    }
}