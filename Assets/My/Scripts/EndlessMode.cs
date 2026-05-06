namespace ZevWaxGames.CursorHero
{
    public class EndlessMode : Button
    {
        protected override void Start() {
            iconPath = "My/My/Sprites/btn_endless_mode";
            tooltipText = "Endless Mode";
            base.Start();
        }
        public override void ButtonAction()
        {
            EventHolder.OnChoosingFinished?.Invoke();
        }
    }
}