namespace ZevWaxGames.CursorHero
{
    public class ProjectileSpeed : Upgrade
    {
        protected override void Start() {
            tooltipText = "Increase Glyph Speed";
            base.Start();
        }

        public override void ButtonAction()
        {
            MainCharacter.Instance.ProjectileSpeed += 3f;
            base.ButtonAction();
        }
    }
}