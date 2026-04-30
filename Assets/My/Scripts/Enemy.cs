using UnityEngine;

namespace ZevWaxGames.CursorHero
{
    public abstract class Enemy : Cursor
    {
        [SerializeField] protected float speed = 3f;

        protected virtual void Start()
        {
            gameObject.layer = LayerMask.NameToLayer("Enemy");
            
            rb = GetComponent<Rigidbody2D>();
            rb.gravityScale = 0;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
            
            targetObj = GameObject.Find("MainCharacter");
            
            base.Start();
        }

        private void FixedUpdate()
        {
            if (MainCharacter.Instance != null)
            {
                Vector2 currentPos = rb.position;
                Vector2 targetPos = MainCharacter.Instance.transform.position;
                Vector2 newPos = Vector2.MoveTowards(currentPos, targetPos, speed * Time.fixedDeltaTime);
                rb.MovePosition(newPos);
            }
        }
        protected override void OnCollisionStay2D(Collision2D collision)
        {
            // Base collision logic (if any)
        }
    }
}