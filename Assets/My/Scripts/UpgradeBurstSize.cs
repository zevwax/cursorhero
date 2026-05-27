namespace ZevWaxGames.CursorHero
{
    public class UpgradeBurstSize : Upgrade
    {
        protected override void Start() {
            tooltipText = "Xtra Shot";
            base.Start();
        }

        public override void ButtonAction()
        {
            Guns.Library[GunName.Yellow].BurstSize++;
            Guns.Library[GunName.Yellow].ShotInterval *= 0.95f;
            Guns.Library[GunName.Yellow].BurstInterval += 0.2f;
            base.ButtonAction();
        }
    }
}