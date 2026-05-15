using System;
using UnityEngine;

namespace ZevWaxGames.CursorHero
{
    public class Gun
    {
        public Action<Vector2, Vector2, Gun> ProjectileSpawner;
        public float Cooldown;
        public float Weight;
        public float Size;
    }
}