namespace ZevWaxGames.CursorHero
{
    public class UpgradeSensitivity : Upgrade
    {
        protected override void Start() {
            tooltipText = string.Format("<b>ZIPPY MOUSE</b>\n-~<=#O#=>~-\nIncrease Sensitivity");
            base.Start();
        }
        public override void ButtonAction()
        {
            MainCharacter.Instance.Sensitivity += 0.1f;
            base.ButtonAction();
        }
    }
}