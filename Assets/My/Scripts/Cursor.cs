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

        protected void Start()
        {
            StartCoroutine(ShootingRoutine());
        }
        protected virtual void Update()
        {
            if (HP <= 0)
                Destroy(gameObject);
        }
        protected virtual void OnCollisionStay2D(Collision2D collision)
        {
            virtualPos = rb.position;
        }
        private IEnumerator ShootingRoutine()
        {
            while (true)
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
    }
}