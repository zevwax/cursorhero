namespace ZevWaxGames.CursorHero
{
    using UnityEngine;

    public abstract class Projectile : MonoBehaviour
    {
        protected float damage;
        public float speed;
        public Vector3 direction;
        private void OnEnable()
        {
            EventHolder.OnRunStarted += Die;
        }
        private void OnDisable()
        {
            EventHolder.OnRunStarted -= Die;
        }
        protected virtual void Start()
        {
            Setup();
        }
        protected abstract void Setup();
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
            if (other.gameObject.GetComponent<Cursor>() != null)
                other.gameObject.GetComponent<Cursor>().HP -= damage;
            Die();
        }
        private void Die()
        {
            Destroy(gameObject);
        }
    }
}