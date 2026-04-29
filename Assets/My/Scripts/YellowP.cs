namespace ZevWaxGames.CursorHero
{
    using UnityEngine;

    public class YellowP : Projectile
    {
        protected override void SetupLayer()
        {
            speed = 6f;
            gameObject.layer = LayerMask.NameToLayer("MainCharacterProjectile");
        }
    }
}