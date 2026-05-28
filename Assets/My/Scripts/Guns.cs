using System.Collections.Generic;

namespace ZevWaxGames.CursorHero
{
    public enum GunName { Red, Yellow, Ring }

    public static class Guns
    {
        public static readonly Dictionary<GunName, Gun> Library = new Dictionary<GunName, Gun>
        {
            { GunName.Yellow, new Gun
            {
                BurstSize = 1,
                BurstInterval = 2f,
                ShotInterval = 0.2f,
                Glyph = new Glyph(true, 1, 1, false, false, 6)
            } },
            { GunName.Red, new Gun
            {
                BurstSize = 1,
                BurstInterval = 2f,
                ShotInterval = 0.2f,
                Glyph = new Glyph(false, 1, 1, false, false, 6)
            } },
            { GunName.Ring, new Gun
            {
                BurstSize = 3,
                BurstInterval = 6f,
                ShotInterval = 0.15f,
                Glyph = new Glyph(false, 1, 1.5f, false, false, 1.5f)
            } }
        };
    }
}