namespace ZevWaxGames.CursorHero
{
    public class Heart : Upgrade
    {
        protected override void Start() {
            iconPath = "My/My/Sprites/btn_upgrade_hrt";
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