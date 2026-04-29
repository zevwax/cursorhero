using UnityEngine;
using System.Collections;

namespace ZevWaxGames.CursorHero
{
    public abstract class Enemy : Cursor
    {
        private GameObject mainCharacter;
        [SerializeField] protected float speed = 3f;
        [SerializeField] protected float shootInterval = 2f;

        protected override void Start()
        {
            mainCharacter = GameObject.Find("MainCharacter");
            
            gameObject.layer = LayerMask.NameToLayer("Enemy");
            
            rb = GetComponent<Rigidbody2D>();
            rb.gravityScale = 0;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
            
            StartCoroutine(ShootingRoutine());
        }

        protected override void FixedUpdate()
        {
            if (MainCharacter.Instance != null)
            {
                Vector2 currentPos = rb.position;
                Vector2 targetPos = MainCharacter.Instance.transform.position;
                Vector2 newPos = Vector2.MoveTowards(currentPos, targetPos, speed * Time.fixedDeltaTime);
                rb.MovePosition(newPos);
            }
        }
        private IEnumerator ShootingRoutine()
        {
            while (true)
            {
                if (MainCharacter.Instance != null)
                {
                    Vector2 direction = (mainCharacter.transform.position - transform.position).normalized;
                    PrefabCreator.NewRedP(new Vector2(transform.position.x, transform.position.y), direction);
                }
                yield return new WaitForSeconds(shootInterval);
            }
        }
        protected override void OnCollisionStay2D(Collision2D collision)
        {
            // Base collision logic (if any)
        }
    }
}