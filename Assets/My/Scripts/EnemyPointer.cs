using UnityEngine;
using System.Collections;
using Unity.Mathematics;
using DG.Tweening;
using Random = UnityEngine.Random;
namespace ZevWaxGames.CursorHero
{
    public class EnemyPointer : Enemy
    {
        protected override void Start()
        {
            HP = 2f;
            dropRange = new int2(1, 2);
            base.Start();
            
            var averageSp = 8f;
            var sp = Random.Range(0.75f, 1.25f)*averageSp;
            var minSpeed = 0;
            var maxSpeed = sp;
            var duration = 7f;

            speed = minSpeed;

            DOTween.To(() => speed, x => speed = x, maxSpeed, duration)
                .SetEase(Ease.Linear)
                .SetLink(gameObject);
        }
        protected override void Shoot() { }
        protected void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.GetComponent<MainCharacter>() != null)
                MainCharacter.Instance.GetDamage(1);
        }
    }
}