using UnityEngine;
using System.Collections.Generic;

namespace ZevWaxGames.CursorHero
{
    public class Discfall : MonoBehaviour
    {
        public static Discfall Instance { get; private set; }
        private float spawnRate = 0.01f;
        private float spawnRangeX = 20f;
        private Queue<GameObject> diskPool = new Queue<GameObject>();
        private Transform poolContainer;
        private int poolSize = 500;
        private float spawnTimer;
        private bool isActive;
        private bool shouldBeActive;

        private void Start()
        {
            Instance = this;
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
        public void SetActive(bool active) => shouldBeActive = active;
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