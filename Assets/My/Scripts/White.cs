using UnityEngine;

namespace ZevWaxGames.CursorHero
{
    public class White : Enemy
    {
        protected override void Start()
        {
            HP = 1f;
            base.Start();
            gun = Guns.Library[GunName.Red];
            speed = 2f;
        }
    }
}