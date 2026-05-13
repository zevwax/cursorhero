namespace ZevWaxGames.CursorHero
{
    public class ProjectileDamage : Upgrade
    {
        protected override void Start() {
            tooltipText = "Increase Bullet Damage";
            base.Start();
        }

        public override void ButtonAction()
        {
            MainCharacter.Instance.ProjectileDamage += 0.5f;
            base.ButtonAction();
        }
    }
}