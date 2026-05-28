using UnityEngine;
using System.Collections;
using Unity.Mathematics;
namespace ZevWaxGames.CursorHero
{
    public class EnemyGrabber : Enemy
    {
        protected override void Start()
        {
            HP = 1f;
            dropRange = new int2(0, 1);
            base.Start();
            speed = 1.25f;
        }
        protected override void Shoot() { }
        private float frequency = 2f;
        private float amplitude = 5f;
        private float timeCounter = 0f;
        private Vector2 currentVirtualTarget;
        protected override void FixedUpdate()
        {
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
            }
        }
        protected override void RotateTowardsTarget()
        {
            if (targetObj != null)
            {
                Vector2 direction = currentVirtualTarget - (Vector2)transform.position;
                float angle = Vector2.SignedAngle(Vector2.up, direction);
                transform.rotation = Quaternion.Euler(0, 0, angle);
            }
        }
        protected void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.GetComponent<MainCharacter>() != null)
            {
                MainCharacter.Instance.GetDamage(1);
            }
        }
    }
}