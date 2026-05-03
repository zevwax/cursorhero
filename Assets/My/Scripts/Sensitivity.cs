namespace ZevWaxGames.CursorHero
{
    public class Sensitivity : Upgrade
    {
        protected override void Start() {
            iconPath = "My/My/Sprites/btn_upgrade_sen";
            tooltipText = "Increase Sensitivity";
            base.Start();
        }

        public override void ButtonAction()
        {
            MainCharacter.Instance.Sensitivity += 0.5f;
            base.ButtonAction();
        }
    }
}