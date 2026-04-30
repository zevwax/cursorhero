using UnityEngine;

namespace ZevWaxGames.CursorHero
{
    public class Yellow : Enemy
    {
        protected override void Start()
        {
            HP = 2f;
            base.Start();
            gun = Guns.Library[GunName.Red];
            speed = 3f;
        }
    }
}