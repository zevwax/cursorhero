using UnityEngine;
using System.Collections;
using Unity.Mathematics;
using DG.Tweening;
using Random = UnityEngine.Random;
namespace ZevWaxGames.CursorHero
{
    public class EnemyGoat : Enemy
    {
        protected override void Start()
        {
            HP = 3f;
            dropRange = new int2(0, 1);
            base.Start();
            
            var averageSp = 1f;
            var sp = Random.Range(0.75f, 1.25f)*averageSp;
            var minSpeed = sp;
            var maxSpeed = sp;
            var duration = (float)default;

            speed = minSpeed;
        }
        protected override void Shoot() { }
        protected void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.GetComponent<MainCharacter>() != null)
                MainCharacter.Instance.GetDamage(1);
        }
    }
}