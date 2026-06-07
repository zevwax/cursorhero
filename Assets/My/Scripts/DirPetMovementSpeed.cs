namespace ZevWaxGames.CursorHero
{
    public class DirPetMovementSpeed : Dir
    {
        public static bool Unlocked => unlocked;
        private static bool unlocked = false;
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
            base.ButtonAction();
        }
    }
}