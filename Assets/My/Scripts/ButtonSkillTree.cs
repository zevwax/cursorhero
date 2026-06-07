using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace ZevWaxGames.CursorHero
{
    public class ButtonSkillTree : Button
    {
        protected override void Start()
        {
            tooltipText = "Skill Tree";
            base.Start();
        }
        public override void ButtonAction()
        {
            base.ButtonAction();
            FindFirstObjectByType<ButtonDrivers>().EnableButton();
            UIManager.Instance.ShowSkillTreeTab();
            UIManager.Instance.HideYouWinNChooseAnUpgradeWindows();
            GameObject.Find("BIOS Driver Indicator").GetComponent<TextMeshProUGUI>().color = new Color(0, 0, 0, 0);
            GameObject.Find("BIOS Chip Indicator").GetComponent<TextMeshProUGUI>().color = new Color(1, 1, 1, 1);
        }
        protected override void EnableAnimation()
        {
            transform.GetChild(0).GetComponent<Image>().sprite = Spawner.GetSprite("bios_btns", "bios_btns_2");
        }
        protected override void DisableAnimation()
        {
            transform.GetChild(0).GetComponent<Image>().sprite = Spawner.GetSprite("bios_btns", "bios_btns_3");
        }
    }
}