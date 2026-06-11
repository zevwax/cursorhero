namespace ZevWaxGames.CursorHero
{
    public class DriverProjectileDamage : Upgrade
    {
        protected override void Start() {
            tooltipText = string.Format("<b>DEAD WEIGHT</b>\n-=*=-\nIncrease Projectile Damage");
            base.Start();
        }
        public override void ButtonAction()
        {
            MainCharacter.Instance.IncreaseWeightBuff(0.5f);
            base.ButtonAction();
        }
    }
}