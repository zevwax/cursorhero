using System;
using UnityEngine;

namespace ZevWaxGames.CursorHero
{
    public class Gun
    {
        public int BurstSize;
        public float BurstInterval;
        public float ShotInterval;
        public Glyph Glyph;
        public void Shoot(Vector2 pos, Vector2 dir)
        {
            Spawner.NewProjectile(pos, dir, Glyph);
        }
    }
}