using Unity.Mathematics;
using UnityEngine;
using System.Collections;

namespace ZevWaxGames.CursorHero
{
    public abstract class Enemy : Cursor
    {
        public float HP;
        [SerializeField] protected float speed = 3f;
        [SerializeField] protected int2 dropRange;
        private BoxCollider2D col;
        private void OnEnable()
        {
            EventHolder.OnBSODStarted += CleanUp;
            EventHolder.OnYouWinStarted += Disable;
            EventHolder.OnYouWinFinished += Enable;
            EventHolder.OnRunFinished += Disable;
        }
        private void OnDisable()
        {
            EventHolder.OnBSODStarted -= CleanUp;
            EventHolder.OnYouWinStarted -= Disable;
            EventHolder.OnYouWinFinished -= Enable;
            EventHolder.OnRunFinished -= Disable;
        }
        protected virtual void Start()
        {
            col = GetComponent<BoxCollider2D>();
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
        protected virtual void FixedUpdate()
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
            StartCoroutine(CStartShooting());
        }
        private IEnumerator CStartShooting()
        {
            yield return new WaitForSeconds(1.25f);
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
                Spawner.NewFloppyDisk(transform.position);
            Destroy(gameObject);
        }
        private void CleanUp()
        {
            Destroy(gameObject);
        }
        public override void GetDamage(float damage)
        {
            HP -= damage;
            if (HP <= 0)
                Die();
            Spawner.NewDamageNumbers(transform.position, true, damage);
        }
    }
}