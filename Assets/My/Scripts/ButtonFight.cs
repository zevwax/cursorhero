namespace ZevWaxGames.CursorHero
{
    public class ButtonFight : Button
    {
        protected override void Start()
        {
            tooltipText = "Fight";
            base.Start();
        }
        public override void ButtonAction()
        {
            base.ButtonAction();
            EventHolder.OnBIOSFinished?.Invoke();
        }
    }
}