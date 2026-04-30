using UnityEngine;

namespace ZevWaxGames.CursorHero
{
    public class Cyan : Enemy
    {
        protected override void Start()
        {
            base.Start();
            gun = Guns.Library[GunName.Ring];
            speed = 1f;
        }
    }
}