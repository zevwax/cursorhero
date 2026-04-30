using System;
using UnityEngine;

namespace ZevWaxGames.CursorHero
{
    public class Gun
    {
        public Action<Vector2, Vector2> ProjectileSpawner;
        public float Cooldown;
    }
}