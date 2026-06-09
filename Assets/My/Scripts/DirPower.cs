using UnityEngine;

namespace ZevWaxGames.CursorHero
{
    public class DirPower : Dir
    {
        public static bool Unlocked => unlocked;
        private static bool unlocked = true;
        private static int price = 3;
        public static void Unlock()
        {
            unlocked = true;
            UIManager.Instance.HideSkillTreeTab();
            UIManager.Instance.ShowSkillTreeTab();
        }
        protected override void Start()
        {
            tooltipText = "Increase Power";
            base.Start();
        }
        public override void ButtonAction()
        {
            if (MainCharacter.Instance.MMs >= price)
            {
                DirMainCharDamage.Unlock();
                DirPetQuantity.Unlock();
                
                MainCharacter.Instance.DeductMMs(price);
                price = (int)System.Math.Round(price*1.25f);

                Power.Instance.IncreaseMaxDuration(25f);
            }
            else
            {
                Spawner.NewPopUpText(transform.position, "Can't afford it\nNeed more RAM sticks", new Color(1, 0, 0, 1), 2);
            }
        }
    }
}