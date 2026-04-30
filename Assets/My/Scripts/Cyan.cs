using UnityEngine;

namespace ZevWaxGames.CursorHero
{
    public class Cyan : Enemy
    {
        protected override void Start()
        {
            HP = 3f;
            base.Start();
            gun = Guns.Library[GunName.Ring];
            speed = 1f;
        }
    }
}