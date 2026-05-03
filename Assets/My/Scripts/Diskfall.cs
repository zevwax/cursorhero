using UnityEngine;

namespace ZevWaxGames.CursorHero
{
    public class Diskfall : MonoBehaviour
    {
        [SerializeField] private float spawnRate = 0.005f;
        [SerializeField] private float spawnRangeX = 20f;
        private Transform poolContainer;
        private float spawnTimer;
        private bool isActive;

        private void Start()
        {
            poolContainer = new GameObject("DiskPool").transform;
            poolContainer.SetParent(transform);
        }

        private void Update()
        {
            bool shouldBeActive = ProgressBar.Instance != null && ProgressBar.Instance.Value >= 1f;

            if (shouldBeActive)
            {
                isActive = true;
                spawnTimer += Time.deltaTime;
                
                while (spawnTimer >= spawnRate)
                {
                    SpawnFallingDisk();
                    spawnTimer -= spawnRate;
                }
            }
            else if (isActive)
            {
                isActive = false;
                ClearPool();
            }
        }

        private void SpawnFallingDisk()
        {
            float randomX = Random.Range(-spawnRangeX, spawnRangeX);
            float spawnY = 7f; 
            GameObject disk = Spawner.NewFallingDisk(new Vector2(randomX, spawnY));
            disk.transform.SetParent(poolContainer);
        }

        private void ClearPool()
        {
            foreach (Transform child in poolContainer)
            {
                Destroy(child.gameObject);
            }
        }
    }
}