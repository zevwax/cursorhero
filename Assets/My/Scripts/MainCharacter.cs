using UnityEngine;
using UnityEngine.InputSystem;

namespace ZevWaxGames.CursorHero
{
    public class MainCharacter : Cursor
    {
        public bool is_trackable = true;
        public float ProjectileDamage = 1f;
        public float ProjectileSpeed = 6f;
        public float Sensitivity = 8f;
        public float Firerate = 8f;
        public static MainCharacter Instance { get; private set; }
        private void OnEnable()
        {
            EventHolder.OnRunStarted += Born;
        }
        private void OnDisable()
        {
            EventHolder.OnRunStarted -= Born;
        }
        private void Start()
        {
            HP = 10f;
            
            gun = Guns.Library[GunName.Yellow];
            gameObject.layer = LayerMask.NameToLayer("MainCharacter");
            
            virtualMousePixels = Mouse.current.position.ReadValue();
            rb = GetComponent<Rigidbody2D>();
            rb.gravityScale = 0;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
            
            lastMousePos = GetMousePos();
            virtualPos = rb.position;
            
            Instance = this;
            
            base.Start();
        }
        protected override void Update()
        {
            base.Update();
            var closest = GetClosestEnemy();
            if (closest != null)
                targetObj = closest;
        }
        private void FixedUpdate()
        {
            Vector2 currentMousePos = GetMousePos();
            Vector2 delta = currentMousePos - lastMousePos;
            lastMousePos = currentMousePos;

            virtualPos += delta;
            rb.MovePosition(virtualPos);
        }
        private Vector2 GetMousePos()
        {
            for(int i = 0; i < 12; i++) 
            {
                virtualMousePixels += Mouse.current.delta.ReadValue();
            }
            return Camera.main.ScreenToWorldPoint((Vector3)virtualMousePixels + Vector3.forward * Sensitivity);
        }
        public GameObject GetClosestEnemy()
        {
            Enemy[] enemies = Object.FindObjectsByType<Enemy>(FindObjectsSortMode.None);
            if (enemies.Length > 0)
            {
                Enemy closest = null;
                float distance = Mathf.Infinity;
                Vector3 position = transform.position;

                foreach (Enemy enemy in enemies)
                {
                    Vector3 diff = enemy.transform.position - position;
                    float curDistance = diff.sqrMagnitude;

                    if (curDistance < distance)
                    {
                        closest = enemy;
                        distance = curDistance;
                    }
                }
                return closest.gameObject;
            }
            return null;
        }
        public void Born()
        {
            HP = 10;
            GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("My/My/Sprites/default_arrow");
            is_trackable = true;
            
            StartCoroutine(ShootingRoutine());
        }
        private void StartChoosing()
        {
            GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("My/My/Sprites/default_link");
            is_trackable = false;
        }
        private void StopChoosing()
        {
            GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("My/My/Sprites/default_arrow");
            is_trackable = true;
        }
        protected override void Die()
        {
            if (is_trackable)
            {
                GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("My/My/Sprites/default_wait");
                is_trackable = false;
                Spawner.NewSoul(transform.position);
                EventHolder.OnPlayerDie?.Invoke();
            }
        }
    }
}