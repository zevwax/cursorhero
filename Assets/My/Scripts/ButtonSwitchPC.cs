using UnityEngine;
using UnityEngine.UI;

namespace ZevWaxGames.CursorHero
{
    public class ButtonSwitchPC : Button
    {
        private void OnEnable()
        {
            EventHolder.OnPCFinished += EnableSafely;
        }
        private void OnDisable()
        {
            EventHolder.OnPCFinished -= EnableSafely;
        }
        protected override void Start()
        {
            tooltipText = "Switch PC";
            GetComponent<Canvas>().sortingLayerName = "ButtonsBG";
            base.Start();
        }
        private void EnableSafely()
        {
            if (!UIManager.Instance.forThe1stTime)
                EnableButton();
        }

        public override void ButtonAction()
        {
            base.ButtonAction();
            UIManager.Instance.SwitchPC();
            EventHolder.OnFadingOutFromPCStarted?.Invoke();
        }
    }
}