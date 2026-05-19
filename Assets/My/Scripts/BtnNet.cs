using UnityEngine;
using UnityEngine.UI;

namespace ZevWaxGames.CursorHero
{
    public class BtnNet : Button
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
            transform.GetChild(1).GetComponent<Image>().color = new Color(0, 0, 0, 0);
            boxPath = "My/My/Sprites/btn_net";
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
        }
    }
}