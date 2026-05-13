namespace ZevWaxGames.CursorHero
{
    public class EndlessMode : Button
    {
        protected override void Start() {
            tooltipText = "Endless Mode";
            base.Start();
        }
        public override void ButtonAction()
        {
            EventHolder.OnChoosingFinished?.Invoke();
        }
    }
}