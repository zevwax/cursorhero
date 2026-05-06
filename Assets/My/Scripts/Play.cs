namespace ZevWaxGames.CursorHero
{
    public class Play : Button
    {
        protected override void Start() {
            iconPath = "My/My/Sprites/btn_play";
            tooltipText = "Play";
            base.Start();
        }
        public override void ButtonAction()
        {
            EventHolder.OnRunStarted?.Invoke();
        }
    }
}