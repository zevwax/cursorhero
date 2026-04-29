namespace ZevWaxGames.CursorHero
{
    using UnityEngine;

    public abstract class Projectile : MonoBehaviour
    {
        public float speed;
        public Vector3 direction;

        protected virtual void Start()
        {
            SetupLayer();
        }

        protected abstract void SetupLayer();

        public void Launch(Vector3 launchDirection)
        {
            direction = launchDirection.normalized;
        }

        protected virtual void Update()
        {
            transform.position += direction * speed * Time.deltaTime;
        }

        protected void OnTriggerEnter2D(Collider2D other)
        {
            Destroy(gameObject);
        }
    }
}