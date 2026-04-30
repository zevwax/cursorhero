using UnityEngine;

namespace ZevWaxGames.CursorHero
{
    public class White : Enemy
    {
        protected override void Start()
        {
            base.Start();
            gun = Guns.Library[GunName.Red];
            speed = 2f;
        }
    }
}