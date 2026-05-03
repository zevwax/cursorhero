namespace ZevWaxGames.CursorHero
{
    public class Firerate : Upgrade
    {
        protected override void Start() {
            iconPath = "My/My/Sprites/btn_upgrade_cdn";
            tooltipText = "Decrease Cooldown";
            base.Start();
        }

        public override void ButtonAction()
        {
            Guns.Library[GunName.Yellow].Cooldown -= 0.1f;
            base.ButtonAction();
        }
    }
}