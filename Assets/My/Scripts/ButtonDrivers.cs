using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace ZevWaxGames.CursorHero
{
    public class ButtonDrivers : Button
    {
        protected override void Start()
        {
            tooltipText = "Drivers";
            base.Start();
            DisableButton();
        }
        public override void ButtonAction()
        {
            base.ButtonAction();
            FindFirstObjectByType<ButtonSkillTree>().EnableButton();
            UIManager.Instance.HideSkillTreeTab();
            UIManager.Instance.ShowChooseAnUpgradeWindow();
            GameObject.Find("BIOS Driver Indicator").GetComponent<TextMeshProUGUI>().color = new Color(1, 1, 1, 1);
            GameObject.Find("BIOS MM Indicator").GetComponent<TextMeshProUGUI>().color = new Color(0, 0, 0, 0);
        }
        protected override void EnableAnimation()
        {
            transform.GetChild(0).GetComponent<Image>().sprite = Spawner.GetSprite("bios_btns", "bios_btns_0");
        }
        protected override void DisableAnimation()
        {
            transform.GetChild(0).GetComponent<Image>().sprite = Spawner.GetSprite("bios_btns", "bios_btns_1");
        }
    }
}