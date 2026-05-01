using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ZevWaxGames.CursorHero
{
    public class WaveManager : MonoBehaviour
    {
        [Header("Wave Settings")]
        public float waveDuration = 30f;
        public float spawnRate = 1f;
        public float initialDifficultyPoints = 1000f;
        
        [Header("Scaling Pattern")]
        public float difficultyMultiplierX = 10f;
        public float[] pattern = { 3f, 8f, 1f };
        
        [Header("Unlock Waves")]
        public int yellowUnlockWave = 2;
        public int cyanUnlockWave = 4;

        private int patternIndex = 0;
        private int currentWave = 1;
        private float currentTotalPoints;
        private float waveBudget;
        private AspectRatioHandler aspectHandler;
        private Coroutine spawnCoroutine;
        private Coroutine waveCoroutine;

        private void Awake()
        {
            aspectHandler = Camera.main.GetComponent<AspectRatioHandler>();
        }

        private void OnEnable()
        {
            EventHolder.OnRunStarted += Refresh;
        }

        private void OnDisable()
        {
            EventHolder.OnRunStarted -= Refresh;
        }

        private void Start()
        {
            Refresh();
        }

        public void Refresh()
        {
            if (spawnCoroutine != null) StopCoroutine(spawnCoroutine);
            if (waveCoroutine != null) StopCoroutine(waveCoroutine);

            patternIndex = 0;
            currentWave = 1;
            currentTotalPoints = initialDifficultyPoints;
            waveBudget = initialDifficultyPoints;

            spawnCoroutine = StartCoroutine(SpawnRoutine());
            waveCoroutine = StartCoroutine(WaveRoutine());
        }

        private IEnumerator SpawnRoutine()
        {
            while (true)
            {
                SpawnEnemies();
                yield return new WaitForSeconds(spawnRate);
            }
        }

        private IEnumerator WaveRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(waveDuration);

                float increase = pattern[patternIndex] * difficultyMultiplierX;
                currentTotalPoints += increase;
                waveBudget = currentTotalPoints;
                
                patternIndex = (patternIndex + 1) % pattern.Length;
                currentWave++;
            }
        }

        private void SpawnEnemies()
        {
            var seed = Random.Range(0, (int)waveDuration);
            var numberOfEnemies = 0;
            if (seed == 0)
                numberOfEnemies = 10;
            else if (seed <= 5)
                numberOfEnemies = 5;
            else if (seed <= 15)
                numberOfEnemies = 1;

            for (var i = 0; i < numberOfEnemies; i++)
            {
                if (waveBudget >= 100)
                {
                    List<int> availableChoices = new List<int> { 0 };
                    if (currentWave >= yellowUnlockWave) availableChoices.Add(1);
                    if (currentWave >= cyanUnlockWave) availableChoices.Add(2);

                    int choice = availableChoices[Random.Range(0, availableChoices.Count)];

                    switch (choice)
                    {
                        case 0:
                            Spawner.NewWhite(GetRandomPos());
                            waveBudget -= 100;
                            break;
                        case 1:
                            Spawner.NewYellow(GetRandomPos());
                            waveBudget -= 120;
                            break;
                        case 2:
                            Spawner.NewCyan(GetRandomPos());
                            waveBudget -= 200;
                            break;
                    }
                }
                else
                    break;
            }
        }

        public Vector2 GetRandomPos()
        {
            if (aspectHandler == null) return Vector2.zero;

            int side = Random.Range(0, 4);
            float buffer = 1f;

            switch (side)
            {
                case 0:
                    return new Vector2(-aspectHandler.Width - buffer, Random.Range(-aspectHandler.Height, aspectHandler.Height));
                case 1:
                    return new Vector2(aspectHandler.Width + buffer, Random.Range(-aspectHandler.Height, aspectHandler.Height));
                case 2:
                    return new Vector2(Random.Range(-aspectHandler.Width, aspectHandler.Width), aspectHandler.Height + buffer);
                case 3:
                default:
                    return new Vector2(Random.Range(-aspectHandler.Width, aspectHandler.Width), -aspectHandler.Height - buffer);
            }
        }
    }
}