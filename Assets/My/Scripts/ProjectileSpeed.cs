namespace ZevWaxGames.CursorHero
{
    public class ProjectileSpeed : Upgrade
    {
        protected override void Start() {
            tooltipText = "Increase Bullet Speed";
            base.Start();
        }

        public override void ButtonAction()
        {
            MainCharacter.Instance.ProjectileSpeed += 3f;
            base.ButtonAction();
        }
    }
}