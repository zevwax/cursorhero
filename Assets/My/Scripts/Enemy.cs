using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace ZevWaxGames.CursorHero
{
    public abstract class Enemy : Cursor
    {
        [SerializeField] protected float speed = 3f;
        [SerializeField] protected int2 dropRange;
        private Collider2D col;
        private void OnEnable()
        {
            EventHolder.OnRunStarted += CleanUp;
            EventHolder.OnChoosingStarted += Disable;
            EventHolder.OnChoosingFinished += Enable;
            EventHolder.OnPlayerDie += Disable;
        }
        private void OnDisable()
        {
            EventHolder.OnRunStarted -= CleanUp;
            EventHolder.OnChoosingStarted -= Disable;
            EventHolder.OnChoosingFinished -= Enable;
            EventHolder.OnPlayerDie -= Disable;
        }
        protected virtual void Start()
        {
            col = GetComponent<Collider2D>();
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
        private void Enable()
        {
            targetObj = mainCharacter.gameObject;
            col.enabled = true;
            StartShooting();
        }
        private void Disable()
        {
            col.enabled = false;
        }
        protected override void Die()
        {
            var disksToDrop = UnityEngine.Random.Range(dropRange.x, dropRange.y+1);
            for (int i = 0; i < disksToDrop; i++)
                Spawner.NewDisk(transform.position);
            Destroy(gameObject);
        }
        private void CleanUp()
        {
            Destroy(gameObject);
        }
    }
}