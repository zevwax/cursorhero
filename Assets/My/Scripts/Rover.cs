using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using System.Linq;

namespace ZevWaxGames.CursorHero
{
    public class Rover : MonoBehaviour
    {
        public static int quantity = 0;
        public static float health = 1f;
        public static float damage = 1f;
        public static float speed = 2f;
        public static float attackInterval = 1f;
        
        private bool isWaiting = false;
        
        private float minWanderTime = 1.5f;
        private float maxWanderTime = 3.5f;
        private float minIdleTime = 0.5f;
        private float maxIdleTime = 1.5f;
        private float frameRate = 0.15f;

        private Rigidbody2D rb;
        private Image image;
        
        private Vector2 movementDirection;
        private Vector2 targetPosition;
        private bool isMoving;

        private Sprite[] leftSprites = new Sprite[4];
        private Sprite[] rightSprites = new Sprite[4];
        private int currentFrameIndex = 0;

        private Transform targetEnemy;

        public void Init()
        {
            rb = GetComponent<Rigidbody2D>();
            if (rb == null) rb = gameObject.AddComponent<Rigidbody2D>();
            
            rb.gravityScale = 0;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;

            var childZero = transform.GetChild(0);
            image = childZero.GetComponent<Image>();
            if (image == null) image = childZero.gameObject.AddComponent<Image>();

            for (var i = 0; i < 4; i++)
            {
                leftSprites[i] = Spawner.GetSprite("rover", $"rover_l{i}");
                rightSprites[i] = Spawner.GetSprite("rover", $"rover_r{i}");
            }

            StartCoroutine(CMainAI());
            StartCoroutine(CAnimateRover());
        }

        private void FixedUpdate()
        {
            FindNearestEnemy();

            if (targetEnemy != null)
            {
                isMoving = true;
                targetPosition = targetEnemy.position;
                movementDirection = (targetPosition - rb.position).normalized;

                var currentPos = rb.position;
                var newPos = Vector2.MoveTowards(currentPos, targetPosition, speed * Time.fixedDeltaTime);
                rb.MovePosition(newPos);
            }
            else if (isMoving)
            {
                var currentPos = rb.position;
                var newPos = Vector2.MoveTowards(currentPos, targetPosition, speed * Time.fixedDeltaTime);
                rb.MovePosition(newPos);

                if (Vector2.Distance(rb.position, targetPosition) < 0.05f)
                {
                    isMoving = false;
                }
            }
        }

        private void FindNearestEnemy()
        {
            var enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
            if (enemies == null || enemies.Length == 0)
            {
                targetEnemy = null;
                return;
            }

            var closestDistance = Mathf.Infinity;
            Transform closestEnemy = null;

            foreach (var enemy in enemies)
            {
                if (enemy == null) continue;
                var distance = Vector2.Distance(transform.position, enemy.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = enemy.transform;
                }
            }

            targetEnemy = closestEnemy;
        }

        private IEnumerator CMainAI()
        {
            while (true)
            {
                if (targetEnemy != null)
                {
                    yield return null;
                    continue;
                }

                var shouldWalk = Random.value > 0.3f;

                if (shouldWalk)
                {
                    movementDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
                    var wanderDuration = Random.Range(minWanderTime, maxWanderTime);
                    
                    targetPosition = rb.position + (movementDirection * speed * wanderDuration);
                    isMoving = true;

                    var elapsed = 0f;
                    while (elapsed < wanderDuration && targetEnemy == null)
                    {
                        elapsed += Time.deltaTime;
                        yield return null;
                    }
                }
                else
                {
                    isMoving = false;
                    var idleDuration = Random.Range(minIdleTime, maxIdleTime);
                    
                    var elapsed = 0f;
                    while (elapsed < idleDuration && targetEnemy == null)
                    {
                        elapsed += Time.deltaTime;
                        yield return null;
                    }
                }
            }
        }

        private IEnumerator CAnimateRover()
        {
            while (true)
            {
                var currentSheet = (movementDirection.x < 0) ? leftSprites : rightSprites;

                if (currentSheet[currentFrameIndex] != null)
                {
                    image.sprite = currentSheet[currentFrameIndex];
                }

                currentFrameIndex = (currentFrameIndex + 1) % 4;

                yield return new WaitForSeconds(frameRate);
            }
        }
        
        protected void OnCollisionEnter2D(Collision2D collision)
        {
            if (isWaiting) return;
            var enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.GetDamage(damage);
                StartCoroutine(Wait());
            }
        }
        private IEnumerator Wait()
        {
            isWaiting = true;
            yield return new WaitForSeconds(attackInterval);
            isWaiting = false;
        }
        private void OnEnable() => EventHolder.OnBIOSStarted += Die;
        private void OnDisable() => EventHolder.OnBIOSStarted -= Die;
        private void Die() => Destroy(gameObject);
    }
}