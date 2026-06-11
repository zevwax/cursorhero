using UnityEngine;
using System.Collections;
using Unity.Mathematics;
using DG.Tweening;
using Random = UnityEngine.Random;

namespace ZevWaxGames.CursorHero
{
    public class EnemyBlueFace : Enemy
    {
        protected override void Start()
        {
            HP = 6f;
            dropRange = new int2(2, 3);
            base.Start();

            var averageSp = 1f;
            var sp = Random.Range(0.75f, 1.25f)*averageSp;
            var minSpeed = sp;
            var maxSpeed = sp;
            var duration = (float)default;

            speed = minSpeed;

            /*DOTween.To(() => speed, x => speed = x, maxSpeed, duration)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo)
                .SetLink(gameObject);*/
        }
        protected override void Shoot() { }
        private float frequency = 2f;
        private float amplitude = 0f;
        private float timeCounter = 0f;
        private Vector2 currentVirtualTarget;
        
        private float rotationSpread = 7f;
        protected override void FixedUpdate()
        {
            GetComponent<Canvas>().sortingOrder = Mathf.RoundToInt(transform.position.y * -100f);
            if (mainCharacter.is_trackable)
            {
                Vector2 currentPos = rb.position;
                Vector2 actualTargetPos = mainCharacter.transform.position;

                Vector2 direction = (actualTargetPos - currentPos).normalized;
                Vector2 perpendicular = new Vector2(-direction.y, direction.x);

                timeCounter += Time.fixedDeltaTime;
                var wave = Mathf.Sin(timeCounter * frequency) * amplitude;
                
                currentVirtualTarget = actualTargetPos + (perpendicular * wave);
                Vector2 newPos = Vector2.MoveTowards(currentPos, currentVirtualTarget, speed * Time.fixedDeltaTime);
                rb.MovePosition(newPos);
                
                float zRotation = Mathf.Sin(timeCounter * frequency) * rotationSpread;
                rb.MoveRotation(zRotation);
            }
        }
        protected override void RotateTowardsTarget() { }
        protected void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.GetComponent<MainCharacter>() != null)
                MainCharacter.Instance.GetDamage(1);
        }
    }
}