using UnityEngine;

namespace ZevWaxGames.CursorHero
{
    public class DirPetMovementSpeed : Dir
    {
        public static bool Unlocked => unlocked;
        private static bool unlocked = false;
        private static int price = 20;
        public static void Unlock()
        {
            unlocked = true;
            UIManager.Instance.HideSkillTreeTab();
            UIManager.Instance.ShowSkillTreeTab();
        }
        protected override void Start()
        {
            tooltipText = "Pet Movement Speed";
            base.Start();
        }
        public override void ButtonAction()
        {
            if (MainCharacter.Instance.MMs >= price)
            {
                MainCharacter.Instance.DeductMMs(price);
                price = (int)System.Math.Round(price*1.75f);

                Rover.speed += 1f;
            }
            else
            {
                Spawner.NewPopUpText(transform.position, "Can't afford it\nNeed more RAM sticks", new Color(1, 0, 0, 1), 2);
            }
        }
    }
}