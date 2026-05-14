using UnityEngine;
using UnityEngine.UI;

namespace ZevWaxGames.CursorHero
{
    public class BtnNet : Button
    {
        private void OnEnable()
        {
            EventHolder.OnPCFinished += EnableButton;
        }
        private void OnDisable()
        {
            EventHolder.OnPCFinished -= EnableButton;
        }
        protected override void Start()
        {
            transform.GetChild(1).GetComponent<Image>().color = new Color(0, 0, 0, 0);
            boxPath = "My/My/Sprites/btn_net";
            tooltipText = "Switch PC";
            GetComponent<Canvas>().sortingLayerName = "ButtonsBG";
            base.Start();
        }

        public override void ButtonAction() => UIManager.Instance.SwitchPC();
    }
}