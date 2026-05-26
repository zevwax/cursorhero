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
            MainCharacter.Instance.AddHeartKeepers(1);
            base.ButtonAction();
        }
    }
}