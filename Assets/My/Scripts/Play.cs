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
            EventHolder.OnRunStarted?.Invoke();
            if (BlueFace.Instance.theAnimIsShown)
                EventHolder.OnPCStarted?.Invoke();
            else
            {
                BlueFace.Instance.StartAnim();
                UIManager.Instance.HideStartGameNTryAgainWindows();
            }
        }
    }
}