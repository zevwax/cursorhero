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
        protected MainCharacter mainCharacter;

        protected void Start()
        {
            mainCharacter = MainCharacter.Instance;
            StartCoroutine(ShootingRoutine());
        }
        protected virtual void Update()
        {
            if (mainCharacter.is_trackable && HP <= 0)
                Die();
        }
        protected virtual void OnCollisionStay2D(Collision2D collision)
        {
            virtualPos = rb.position;
        }
        private IEnumerator ShootingRoutine()
        {
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
    }
}