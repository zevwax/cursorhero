namespace ZevWaxGames.CursorHero
{
    using UnityEngine;

    public class YellowP : Projectile
    {
        protected override void Setup()
        {
            damage = 1f;
            speed = 6f;
            gameObject.layer = LayerMask.NameToLayer("MainCharacterProjectile");
        }
    }
}