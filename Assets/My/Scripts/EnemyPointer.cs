using Unity.Mathematics;
namespace ZevWaxGames.CursorHero
{
    public class EnemyPointer : Enemy
    {
        protected override void Start()
        {
            HP = 1f;
            dropRange = new int2(0, 1);
            base.Start();
            gun = Guns.Library[GunName.Red];
            speed = 1.5f;
        }
    }
}