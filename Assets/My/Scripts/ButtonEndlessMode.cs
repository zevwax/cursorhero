using UnityEngine;
using UnityEngine.UI;

namespace ZevWaxGames.CursorHero
{
    public class ButtonEndlessMode : Button
    {
        protected override void Start()
        {
            tooltipText = "Endless Mode";
            base.Start();
        }
        public override void ButtonAction()
        {
            base.ButtonAction();
            EventHolder.OnYouWinFinished?.Invoke();
            FloppyDisk.Normalize();
        }
    }
}