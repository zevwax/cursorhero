namespace ZevWaxGames.CursorHero
{
    public class UpgradeHeart : Upgrade
    {
        protected override void Start() {
            tooltipText = "Increase Max Health";
            base.Start();
        }
        public override void ButtonAction()
        {
            MainCharacter.Instance.MaxHP++;
            MainCharacter.Instance.HP = MainCharacter.Instance.MaxHP;
            base.ButtonAction();
        }
    }
}