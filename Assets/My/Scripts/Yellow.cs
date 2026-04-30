using UnityEngine;

namespace ZevWaxGames.CursorHero
{
    public class Yellow : Enemy
    {
        protected override void Start()
        {
            base.Start();
            gun = Guns.Library[GunName.Red];
            speed = 3f;
        }
    }
}