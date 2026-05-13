namespace ZevWaxGames.CursorHero
{
    public class Sensitivity : Upgrade
    {
        protected override void Start() {
            tooltipText = "Increase Sensitivity";
            base.Start();
        }

        public override void ButtonAction()
        {
            MainCharacter.Instance.Sensitivity += 0.2f;
            base.ButtonAction();
        }
    }
}