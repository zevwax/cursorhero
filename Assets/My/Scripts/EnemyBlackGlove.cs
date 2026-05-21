using Unity.Mathematics;
namespace ZevWaxGames.CursorHero
{
    public class EnemyBlackGlove : Enemy
    {
        protected override void Start()
        {
            HP = 2f;
            dropRange = new int2(1, 2);
            base.Start();
            gun = Guns.Library[GunName.Red];
            speed = 3f;
        }
    }
}