using UnityEngine;
using System;
using System.Collections;

namespace ZevWaxGames.CursorHero
{
    public class Cursor : MonoBehaviour
    {
        public float HP;
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
            if (mainCharacter.is_trackable && HP <= 0)
                Die();
            
            RotateTowardsTarget();
        }
        protected void RotateTowardsTarget()
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
            yield return new WaitForSeconds(1);
            while (mainCharacter.is_trackable)
            {
                if (targetObj != null && gun != null)
                {
                    Vector2 direction = (targetObj.transform.position - transform.position).normalized;
                    gun.ProjectileSpawner.Invoke(new Vector2(transform.position.x, transform.position.y), direction);
                    yield return new WaitForSeconds(gun.Cooldown);
                }
                else
                    yield return new WaitForSeconds(0.1f);
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
    }
}