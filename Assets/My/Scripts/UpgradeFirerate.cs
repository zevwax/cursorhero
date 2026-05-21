namespace ZevWaxGames.CursorHero
{
    public class UpgradeFirerate : Upgrade
    {
        protected override void Start() {
            tooltipText = "Increase Typing Speed";
            base.Start();
        }

        public override void ButtonAction()
        {
            Guns.Library[GunName.Yellow].Cooldown *= 0.666f;
            base.ButtonAction();
        }
    }
}