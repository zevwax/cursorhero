namespace ZevWaxGames.CursorHero
{
    public class Firerate : Upgrade
    {
        protected override void Start() {
            tooltipText = "Increase Fire Rate";
            base.Start();
        }

        public override void ButtonAction()
        {
            Guns.Library[GunName.Yellow].Cooldown *= 0.666f;
            base.ButtonAction();
        }
    }
}