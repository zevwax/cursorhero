using Unity.Mathematics;
namespace ZevWaxGames.CursorHero
{
    public class EnemyGoatLegacy : Enemy
    {
        protected override void Start()
        {
            HP = 3f;
            dropRange = new int2(2, 3);
            base.Start();
            gun = Guns.Library[GunName.Ring];
            speed = 1f;
        }
    }
}