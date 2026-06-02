using UnityEngine;
using UnityEngine.UI;

namespace ZevWaxGames.CursorHero
{
    public class ButtonEndlessMode : Button
    {
        protected override void Start()
        {
            boxPath = "My/My/Sprites/btn_endless_mode";
            transform.GetChild(1).GetComponent<Image>().color = new Color(0, 0, 0, 0);
            tooltipText = "Endless Mode";
            base.Start();
        }
        public override void ButtonAction()
        {
            base.ButtonAction();
            EventHolder.OnChoosingFinished?.Invoke();
            FloppyDisk.Normalize();
        }
    }
}