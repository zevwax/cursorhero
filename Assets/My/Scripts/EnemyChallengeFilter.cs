using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZevWaxGames.CursorHero
{
    public class EnemyChallengeFilter : MonoBehaviour
    {
        private Vector2 bottomLeft = new Vector2(-(8f+0.5f), -(4.5f+0.5f));
        private Vector2 topLeft = new Vector2(-(8f+0.5f), (4.5f+0.5f));
        private Vector2 bottomRight = new Vector2((8f+0.5f), -(4.5f+0.5f));
        private Vector2 topRight = new Vector2((8f+0.5f), (4.5f+0.5f));

        private List<GameObject> spawnedProjectiles = new List<GameObject>();
        private GameObject ind;
        private int currentSide;
        private int projectileCount;
        private float speed;
        private Glyph _glyph;
        
        private bool isReady;

        public void Init()
        {
            currentSide = Random.Range(0, 4);
            projectileCount = Random.Range(1, 5);
            speed = Mathf.Clamp(WaveManager.Instance.CurrentWaveIndex + 1f, 1f, 6f) + Random.Range(0, 1f);
            _glyph = GenerateRandomGlyph();

            StartCoroutine(StartWithDelay());
        }
        private IEnumerator StartWithDelay()
        {
            var startCorner = Vector2.zero;
            var endCorner = Vector2.zero;
            var direction = Vector2.zero;
            
            switch (currentSide)
            {
                case 0:
                    startCorner = topLeft;
                    endCorner = topRight;
                    direction = Vector2.down;
                    break;
                case 1:
                    startCorner = topRight;
                    endCorner = bottomRight;
                    direction = Vector2.left;
                    break;
                case 2:
                    startCorner = bottomRight;
                    endCorner = bottomLeft;
                    direction = Vector2.up;
                    break;
                case 3:
                    startCorner = bottomLeft;
                    endCorner = topLeft;
                    direction = Vector2.right;
                    break;
            }
            
            ind = Spawner.NewChallengeIndication(Vector2.Lerp(startCorner, endCorner, 0.5f));
            
            yield return new WaitForSeconds(2f);
            
            ind.GetComponent<ChallengeIndication>().Remove();
            
            var totalPoints = projectileCount + 2;
            for (var i = 1; i < totalPoints - 1; i++)
            {
                var t = (float)i / (totalPoints - 1);
                var spawnPosition = Vector2.Lerp(startCorner, endCorner, t);
                
                var projectile = Spawner.NewProjectile(spawnPosition, direction, _glyph);
                if (projectile != null)
                {
                    spawnedProjectiles.Add(projectile);
                }
            }
            
            isReady = true;
        }
        private void Update()
        {
            if (!isReady) return;
            
            for (var i = spawnedProjectiles.Count - 1; i >= 0; i--)
            {
                var projectile = spawnedProjectiles[i];

                if (projectile == null)
                {
                    spawnedProjectiles.RemoveAt(i);
                    continue;
                }

                if (IsPastOppositeSide(projectile.transform.position))
                {
                    Destroy(projectile);
                    spawnedProjectiles.RemoveAt(i);
                }
            }

            if (spawnedProjectiles.Count == 0)
            {
                Destroy(gameObject);
            }
        }

        private bool IsPastOppositeSide(Vector2 position)
        {
            switch (currentSide)
            {
                case 0: return position.y <= bottomLeft.y;
                case 1: return position.x <= bottomLeft.x;
                case 2: return position.y >= topLeft.y;
                case 3: return position.x >= bottomRight.x;
                default: return false;
            }
        }

        private Glyph GenerateRandomGlyph()
        {
            var isAlly = false;
            var weight = 1f;
            var size = 1f;
            if (Random.value <= 0.25f)
            {
                weight = Random.Range(1f, 3f);
                size = Random.Range(2f, 3f);
            }
            var speed = this.speed;

            var isBouncy = false;
            var isPiercing = false;

            if (Random.value <= 0.1f)
            {
                if (Random.value < 0.5f)
                    isBouncy = true;
                else
                    isPiercing = true;
            }

            return new Glyph(isAlly, weight, size, isBouncy, isPiercing, speed);
        }
    }
}