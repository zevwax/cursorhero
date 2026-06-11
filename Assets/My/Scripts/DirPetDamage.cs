using UnityEngine;

namespace ZevWaxGames.CursorHero
{
    public class DirPetDamage : Dir
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
            tooltipText = string.Format("<b>SHARP CLAWS</b>\n-~<=#O#=>~-\nIncrease Pet Damage\n[ Price: <sprite=7> {0} ]", price);
            base.Start();
        }
        public override void ButtonAction()
        {
            if (MainCharacter.Instance.MMs >= price)
            {
                DirPetAttackInterval.Unlock();
                
                MainCharacter.Instance.DeductMMs(price);
                price = (int)System.Math.Round(price*1.75f);
                
                Rover.damage += 0.5f;
            }
            else
            {
                Spawner.NewPopUpText(transform.position, "Can't afford it\nNeed more RAM sticks", new Color(1, 0, 0, 1), 2);
            }
        }
    }
}