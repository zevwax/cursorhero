namespace ZevWaxGames.CursorHero
{
    public class UpgradeSensitivity : Upgrade
    {
        protected override void Start() {
            tooltipText = "Increase Sensitivity";
            base.Start();
        }
        public override void ButtonAction()
        {
            MainCharacter.Instance.Sensitivity += 0.1f;
            base.ButtonAction();
        }
    }
}