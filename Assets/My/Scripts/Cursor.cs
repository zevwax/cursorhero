using UnityEngine;
using System.Collections;

namespace ZevWaxGames.CursorHero
{
    public class Cursor : MonoBehaviour
    {
        protected GameObject targetObj;
        protected Gun gun;
        protected Vector2 virtualMousePixels; 
        protected Rigidbody2D rb;
        protected Vector2 lastMousePos;
        protected Vector2 virtualPos;
        protected static MainCharacter mainCharacter;
        protected Coroutine shootingRoutine;

        protected void Start()
        {
            mainCharacter = MainCharacter.Instance;
            StartShooting();
        }
        protected virtual void Update()
        {
            RotateTowardsTarget();
        }
        protected virtual void RotateTowardsTarget()
        {
            if (targetObj != null)
            {
                Vector2 direction = targetObj.transform.position - transform.position;
                float angle = Vector2.SignedAngle(Vector2.up, direction);
                transform.rotation = Quaternion.Euler(0, 0, angle);
            }
        }
        protected virtual void OnCollisionStay2D(Collision2D collision)
        {
            virtualPos = rb.position;
        }
        protected IEnumerator ShootingRoutine()
        {
            while (mainCharacter.is_trackable)
            {
                if (targetObj != null && gun != null)
                {
                    for (var i = 0; i < gun.BurstSize; i++)
                    {
                        Shoot();
                        yield return new WaitForSeconds(gun.ShotInterval);
                    }
                    yield return new WaitForSeconds(gun.BurstInterval);
                }
                else
                    yield return new WaitForSeconds(0.1f);
            }
        }
        protected virtual void Shoot()
        {
            if (targetObj != null)
            {
                var direction = (targetObj.transform.position - transform.position).normalized;
                gun.Shoot(new Vector2(transform.position.x, transform.position.y), direction);
            }
        }
        protected virtual void Die()
        {
            throw new System.NotImplementedException();
        }
        protected void StartShooting()
        {
            if (shootingRoutine != null)
                StopCoroutine(shootingRoutine);
            shootingRoutine = StartCoroutine(ShootingRoutine());
        }
        public virtual void GetDamage(float damage) { }
    }
}