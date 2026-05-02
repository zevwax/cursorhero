namespace ZevWaxGames.CursorHero
{
    public class ProjectileDamage : Upgrade
    {
        protected override void Start() {
            iconPath = "My/My/Sprites/btn_upgrade_dmg";
            base.Start();
        }
        public override void ButtonAction() => MainCharacter.Instance.ProjectileDamage += 1f;
    }
}