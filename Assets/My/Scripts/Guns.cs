using System.Collections.Generic;

namespace ZevWaxGames.CursorHero
{
    public enum GunName { Red, Yellow, Ring }

    public static class Guns
    {
        public static readonly Dictionary<GunName, Gun> Library = new Dictionary<GunName, Gun>
        {
            { GunName.Yellow, new Gun { ProjectileSpawner = Spawner.NewYellowP, Cooldown = 2f } },
            { GunName.Red, new Gun { ProjectileSpawner = Spawner.NewRedP, Cooldown = 2f } },
            { GunName.Ring, new Gun { ProjectileSpawner = Spawner.NewRingP, Cooldown = 1f } }
        };
    }
}