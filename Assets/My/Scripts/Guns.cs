using System.Collections.Generic;

namespace ZevWaxGames.CursorHero
{
    public enum GunName { Red, Yellow, Ring }

    public static class Guns
    {
        public static readonly Dictionary<GunName, Gun> Library = new Dictionary<GunName, Gun>
        {
            { GunName.Yellow, new Gun { ProjectileSpawner = Spawner.NewProjectileBlue, Cooldown = 2f, Weight = 1f, Size = 1f } },
            { GunName.Red, new Gun { ProjectileSpawner = Spawner.NewProjectileRedRegular, Cooldown = 2f, Weight = 1f, Size = 1f } },
            { GunName.Ring, new Gun { ProjectileSpawner = Spawner.NewProjectileRedLarge, Cooldown = 1f, Weight = 1f, Size = 2f } }
        };
    }
}