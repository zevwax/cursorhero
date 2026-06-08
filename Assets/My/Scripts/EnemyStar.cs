using System;
using UnityEngine;
using System.Collections;
using Unity.Mathematics;
using DG.Tweening;
namespace ZevWaxGames.CursorHero
{
    public class EnemyStar : Enemy
    {
        private float frequency = 2f;
        protected override void Start()
        {
            HP = 3f;
            dropRange = new int2(2, 3);
            base.Start();

            StartCoroutine(Wait());
        }
        private IEnumerator Wait()
        {
            var duration = frequency/4f;
            var minSpeed = 4f;
            var maxSpeed = 4f;
            speed = minSpeed;
            yield return new WaitForSeconds(duration);
            DOTween.To(() => speed, x => speed = x, maxSpeed, duration)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo)
                .SetLink(gameObject);
        }
        protected override void Shoot() { }
        private float amplitude = 40f;
        private float timeCounter = 0f;
        private Vector2 currentVirtualTarget;
        
        private float rotationSpread = 15f;
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