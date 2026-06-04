namespace ZevWaxGames.CursorHero
{
    public class ButtonPlay : Button
    {
        protected override void Start()
        {
            tooltipText = "Play";
            base.Start();
        }
        public override void ButtonAction()
        {
            base.ButtonAction();
            EventHolder.OnRunStarted?.Invoke();
            if (BlueFace.Instance.theAnimIsShown)
                EventHolder.OnPCStarted?.Invoke();
            else
            {
                Zipporah.Instance.SwitchActionTo(1);
                UIManager.Instance.HideStartGameNTryAgainWindows();
            }
        }
    }
}