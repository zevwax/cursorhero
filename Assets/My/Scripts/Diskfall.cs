using UnityEngine;
using System.Collections.Generic;

namespace ZevWaxGames.CursorHero
{
    public class Diskfall : MonoBehaviour
    {
        private float spawnRate = 0.01f;
        private float spawnRangeX = 20f;
        private Queue<GameObject> diskPool = new Queue<GameObject>();
        private Transform poolContainer;
        private int poolSize = 500;
        private float spawnTimer;
        private bool isActive;

        private void Start()
        {
            poolContainer = new GameObject("DiskPool").transform;
            poolContainer.transform.position = new Vector3(0f, 0f, 0f);
            poolContainer.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            poolContainer.transform.localScale = new Vector3(1f, 1f, 1f);
            for (int i = 0; i < poolSize; i++)
            {
                var disk = Spawner.NewFallingDisk(Vector2.zero);
                disk.transform.SetParent(poolContainer);
                disk.SetActive(false);
                diskPool.Enqueue(disk);
            }
        }

        private void Update()
        {
            var shouldBeActive = ProgressBar.Instance != null && ProgressBar.Instance.Value >= 1f;

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
                DeactivateAll();
            }
        }

        private void SpawnFallingDisk()
        {
            if (diskPool.Count == 0) return;
            var disk = diskPool.Dequeue();
            disk.GetComponent<FallingDisk>().Refresh();
            diskPool.Enqueue(disk);
        }

        private void DeactivateAll()
        {
            foreach (Transform disk in poolContainer)
                disk.GetComponent<FallingDisk>().Die();
        }
    }
}