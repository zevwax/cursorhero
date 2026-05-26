namespace ZevWaxGames.CursorHero
{
    public class UpgradeHeart : Upgrade
    {
        protected override void Start() {
            tooltipText = "Restore Full HP";
            base.Start();
        }
        public override void ButtonAction()
        {
            MainCharacter.Instance.RestoreFullHP();
            base.ButtonAction();
        }
    }
}