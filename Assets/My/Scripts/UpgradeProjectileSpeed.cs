namespace ZevWaxGames.CursorHero
{
    public class UpgradeProjectileSpeed : Upgrade
    {
        protected override void Start() {
            tooltipText = string.Format("<b>ZIPPY MOUSE</b>\n-~<=#O#=>~-\nIncrease Projectile Speed");
            base.Start();
        }

        public override void ButtonAction()
        {
            Guns.Library[GunName.Yellow].Glyph.Speed += 3f;
            Guns.Library[GunName.Yellow].ShotInterval *= 0.9f;
            base.ButtonAction();
        }
    }
}