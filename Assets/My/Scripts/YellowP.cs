namespace ZevWaxGames.CursorHero
{
    using UnityEngine;

    public class YellowP : Projectile
    {
        protected override void Setup()
        {
            speed = MainCharacter.Instance.ProjectileSpeed;
            gameObject.layer = LayerMask.NameToLayer("MainCharacterProjectile");
        }
    }
}