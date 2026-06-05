using UnityEngine;

namespace ZevWaxGames.CursorHero
{
    public abstract class Upgrade : Button
    {
        public override void ButtonAction()
        {
            base.ButtonAction();
            UIManager.Instance.HideYouWinNChooseAnUpgradeWindows();
            if (MainCharacter.Instance.Drivers > 0)
                UIManager.Instance.ShowChooseAnUpgradeWindow();
        }
    }
}