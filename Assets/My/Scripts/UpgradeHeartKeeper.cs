namespace ZevWaxGames.CursorHero
{
    public class UpgradeHeartKeeper : Upgrade
    {
        protected override void Start() {
            tooltipText = "Add an empty Heart Keeper";
            base.Start();
        }
        public override void ButtonAction()
        {
            MainCharacter.Instance.MaxHP += 5;
            MainCharacter.Instance.AddHeartKeeper();
            base.ButtonAction();
        }
    }
}