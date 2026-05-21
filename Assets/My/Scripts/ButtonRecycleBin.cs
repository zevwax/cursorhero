using UnityEngine;
using UnityEngine.UI;

namespace ZevWaxGames.CursorHero
{
    public class ButtonRecycleBin : Button
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
            boxPath = "My/My/Sprites/bin";
            transform.GetChild(1).GetComponent<Image>().color = new Color(0, 0, 0, 0);
            tooltipText = "Recycle Bin";
            GetComponent<Canvas>().sortingLayerName = "ButtonsBG";
            base.Start();
        }
        public override void ButtonAction()
        {
            base.ButtonAction();
            MainCharacter.Instance.StopBeingSkinSetter(gameObject);
            UIManager.Instance.ShowRecycleBinWindow();
        }
    }
}