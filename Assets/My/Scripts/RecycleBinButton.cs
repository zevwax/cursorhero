namespace ZevWaxGames.CursorHero
{
    public class RecycleBinButton : Button
    {
        protected override void Start() {
            boxPath = "My/My/Sprites/bin";
            iconPath = "My/My/Sprites/bin";
            tooltipText = "Recycle Bin";
            base.Start();
        }
        public override void ButtonAction()
        {
            UIManager.Instance.ShowRecycleBinWindow();
        }
    }
}