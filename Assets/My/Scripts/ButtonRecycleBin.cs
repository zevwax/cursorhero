using UnityEngine;
using UnityEngine.UI;

namespace ZevWaxGames.CursorHero
{
    public class ButtonRecycleBin : Button
    {
        private void OnEnable()
        {
            EventHolder.OnPCFinished += EnableButton;
            EventHolder.OnBIOSStarted += DisableButton;
        }
        private void OnDisable()
        {
            EventHolder.OnPCFinished -= EnableButton;
            EventHolder.OnBIOSStarted -= DisableButton;
        }
        protected override void Start()
        {
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