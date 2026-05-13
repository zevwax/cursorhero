using UnityEngine;
using UnityEngine.UI;

namespace ZevWaxGames.CursorHero
{
    public class RecycleBinButton : Button
    {
        protected override void Start()
        {
            transform.GetChild(1).GetComponent<Image>().color = new Color(0, 0, 0, 0);
            boxPath = "My/My/Sprites/bin";
            tooltipText = "Recycle Bin";
            base.Start();
        }
        public override void ButtonAction()
        {
            UIManager.Instance.ShowRecycleBinWindow();
        }
    }
}