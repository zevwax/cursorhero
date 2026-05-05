namespace ZevWaxGames.CursorHero
{
    public class ProjectileSpeed : Upgrade
    {
        protected override void Start() {
            iconPath = "My/My/Sprites/btn_upgrade_spd";
            tooltipText = "Increase Speed";
            base.Start();
        }

        public override void ButtonAction()
        {
            MainCharacter.Instance.ProjectileSpeed += 3f;
            base.ButtonAction();
        }
    }
}