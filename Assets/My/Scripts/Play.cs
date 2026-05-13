namespace ZevWaxGames.CursorHero
{
    public class Play : Button
    {
        protected override void Start() {
            tooltipText = "Play";
            base.Start();
        }
        public override void ButtonAction()
        {
            if (BlueFace.Instance.theAnimIsShown)
                EventHolder.OnRunStarted?.Invoke();
            else
            {
                BlueFace.Instance.StartAnim();
                UIManager.Instance.HideStartGameNTryAgainWindows();
            }
        }
    }
}