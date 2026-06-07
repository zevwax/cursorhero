namespace ZevWaxGames.CursorHero
{
    public class DirPetQuantity : Dir
    {
        public static bool Unlocked => unlocked;
        private static bool unlocked = true;
        public static void Unlock()
        {
            unlocked = true;
            UIManager.Instance.HideSkillTreeTab();
            UIManager.Instance.ShowSkillTreeTab();
        }
        protected override void Start()
        {
            tooltipText = "Pet Quantity";
            base.Start();
        }
        public override void ButtonAction()
        {
            base.ButtonAction();
            DirPetHealth.Unlock();
            DirPetDamage.Unlock();
        }
    }
}