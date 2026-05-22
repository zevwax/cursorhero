using UnityEngine;
using UnityEngine.UI;

namespace ZevWaxGames.CursorHero
{
    public class ButtonPlay : Button
    {
        protected override void Start()
        {
            boxPath = "My/My/Sprites/btn_play";
            transform.GetChild(1).GetComponent<Image>().color = new Color(0, 0, 0, 0);
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
                Tabby.Instance.SwitchActionTo(1);
                UIManager.Instance.HideStartGameNTryAgainWindows();
            }
        }
    }
}