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
            
            base.Start();
        }
        protected override void Update()
        {
            base.Update();
            if (mainCharacter.is_trackable)
                targetObj = mainCharacter.gameObject;
            else
                targetObj = gameObject;
        }
        private void FixedUpdate()
        {
            if (mainCharacter.is_trackable)
            {
                Vector2 currentPos = rb.position;
                Vector2 targetPos = mainCharacter.transform.position;
                Vector2 newPos = Vector2.MoveTowards(currentPos, targetPos, speed * Time.fixedDeltaTime);
                rb.MovePosition(newPos);
            }
        }
        protected override void OnCollisionStay2D(Collision2D collision)
        {
            // Base collision logic (if any)
        }
        protected override void Die()
        {
            Destroy(gameObject);
        }
    }
}