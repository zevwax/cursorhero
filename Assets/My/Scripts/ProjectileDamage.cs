namespace ZevWaxGames.CursorHero
{
    public class ProjectileDamage : Upgrade
    {
        protected override void Start() {
            tooltipText = "Increase Glyph Weight";
            base.Start();
        }

        public override void ButtonAction()
        {
            MainCharacter.Instance.IncreaseWeightBuff(0.5f);
            base.ButtonAction();
        }
    }
}