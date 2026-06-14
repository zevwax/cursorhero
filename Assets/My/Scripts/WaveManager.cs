using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ZevWaxGames.CursorHero
{
    public class WaveManager : MonoBehaviour
    {
        public static WaveManager Instance { get; private set; }
        private List<WaveConfig> waves = new List<WaveConfig>();
        public int CurrentWaveIndex => currentWaveIndex;
        private int currentWaveIndex = 0;
        private float waveStartTime = 0;
        private AspectRatioHandler aspectHandler;
        private Coroutine spawnCoroutine;
        private float waveDuration = 20f;

        private void Awake()
        {
            Instance = this;
            aspectHandler = Camera.main.GetComponent<AspectRatioHandler>();
            SetupWaves();
        }

        private void OnEnable() => EventHolder.OnRunStarted += StartManager;
        private void OnDisable() => EventHolder.OnRunStarted -= StartManager;

        private void SetupWaves()
        {
            waves.Clear();

            var id1 = 0.001f;
            var id2 = 0.002f;
            var id3 = 0.003f;
            var id4 = 0.004f;
            var id5 = 0.005f;
            
            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 4,
                spawnRate = 1f/(1*0.75f),
                spawnWhiteMobAtStart = false,
                
                maxEnemiesPerSpawn = 4,
                
                pointerSpawnProbability = 0,
                fistSpawnProbability = 0,
                blueFaceSpawnProbability = 0,
                redFaceSpawnProbability = 0,
                greenFaceSpawnProbability = 0
            });

            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 5,
                spawnRate = 1f/2,
                spawnWhiteMobAtStart = false,
                
                maxEnemiesPerSpawn = 4,
                
                firstAppearance = (pos) => Spawner.NewEnemyPointer(pos),
                pointerSpawnProbability = 0,
                fistSpawnProbability = 0,
                blueFaceSpawnProbability = 0,
                redFaceSpawnProbability = 0,
                greenFaceSpawnProbability = 0
            });

            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 6,
                spawnRate = 1f/3,
                spawnWhiteMobAtStart = true,
                whiteMobSize = 1,
                maxEnemiesPerSpawn = 4,
                
                firstAppearance = (pos) => Spawner.NewEnemyBlueFace(pos),
                pointerSpawnProbability = 0,
                fistSpawnProbability = 0,
                blueFaceSpawnProbability = 0,
                redFaceSpawnProbability = 0,
                greenFaceSpawnProbability = 0
            });
            
            //===

            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 5,
                spawnRate = 1f/(3*0.75f),
                spawnWhiteMobAtStart = false,
                
                maxEnemiesPerSpawn = 4,
                
                firstAppearance = (pos) => Spawner.NewEnemyFist(pos),
                pointerSpawnProbability = 1/10f + id1,
                fistSpawnProbability = 0,
                blueFaceSpawnProbability = 1/10f + id3,
                redFaceSpawnProbability = 0,
                greenFaceSpawnProbability = 0
            });

            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 6,
                spawnRate = 1f/4,
                spawnWhiteMobAtStart = false,
                
                maxEnemiesPerSpawn = 4,
                
                pointerSpawnProbability = 0,
                fistSpawnProbability = 1,
                blueFaceSpawnProbability = 0,
                redFaceSpawnProbability = 0,
                greenFaceSpawnProbability = 0
            });

            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 7,
                spawnRate = 1f/5,
                spawnWhiteMobAtStart = true,
                whiteMobSize = 2,
                
                maxEnemiesPerSpawn = 4,
                
                firstAppearance = (pos) => Spawner.NewEnemyRedFace(pos),
                pointerSpawnProbability = 0,
                fistSpawnProbability = 1/10f + id2,
                blueFaceSpawnProbability = 0,
                redFaceSpawnProbability = 0,
                greenFaceSpawnProbability = 0
            });
            
            //====
            
            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 6,
                spawnRate = 1f/(5*0.5f),
                spawnWhiteMobAtStart = false,
                
                maxEnemiesPerSpawn = 6,
                
                pointerSpawnProbability = 1/10f + id1,
                fistSpawnProbability = 1/10f + id2,
                blueFaceSpawnProbability = 1/10f + id3,
                redFaceSpawnProbability = 1/10f + id4,
                greenFaceSpawnProbability = 0
            });
            
            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 7,
                spawnRate = 1f/6,
                spawnWhiteMobAtStart = false,
                
                firstAppearance = (pos) => Spawner.NewEnemyGreenFace(pos),
                pointerSpawnProbability = 1/10f + id1,
                fistSpawnProbability = 1/10f + id2,
                blueFaceSpawnProbability = 0,
                redFaceSpawnProbability = 0,
                greenFaceSpawnProbability = 0
            });

            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 8,
                spawnRate = 1f/7,
                spawnWhiteMobAtStart = true,
                whiteMobSize = 3,
                
                maxEnemiesPerSpawn = 6,
                
                pointerSpawnProbability = 1/10f + id1,
                fistSpawnProbability = 1/10f + id2,
                blueFaceSpawnProbability = 1/10f + id3,
                redFaceSpawnProbability = 1/10f + id4,
                greenFaceSpawnProbability = 0
            });
            
            //====
            
            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 7,
                spawnRate = 1f/(7*0.5f),
                spawnWhiteMobAtStart = false,
                
                maxEnemiesPerSpawn = 6,
                
                pointerSpawnProbability = 1/10f + id1,
                fistSpawnProbability = 1/10f + id2,
                blueFaceSpawnProbability = 1/10f + id3,
                redFaceSpawnProbability = 1/10f + id4,
                greenFaceSpawnProbability = 1/10f + id5
            });

            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 8,
                spawnRate = 1f/8,
                spawnWhiteMobAtStart = false,
                
                maxEnemiesPerSpawn = 6,
                
                pointerSpawnProbability = 1,
                fistSpawnProbability = 0,
                blueFaceSpawnProbability = 0,
                redFaceSpawnProbability = 0,
                greenFaceSpawnProbability = 0
            });

            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 9,
                spawnRate = 1f/9,
                spawnWhiteMobAtStart = true,
                whiteMobSize = 4,
                
                maxEnemiesPerSpawn = 6,
                
                pointerSpawnProbability = 1/10f + id1,
                fistSpawnProbability = 1/10f + id2,
                blueFaceSpawnProbability = 1/10f + id3,
                redFaceSpawnProbability = 1/10f + id4,
                greenFaceSpawnProbability = 1/10f + id5
            });
            
            //====
            
            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 8,
                spawnRate = 1f/(9*0.75f),
                spawnWhiteMobAtStart = false,
                
                maxEnemiesPerSpawn = 8,
                
                pointerSpawnProbability = 1/10f + id1,
                fistSpawnProbability = 1/10f + id2,
                blueFaceSpawnProbability = 1/10f + id3,
                redFaceSpawnProbability = 1/10f + id4,
                greenFaceSpawnProbability = 1/10f + id5
            });

            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 9,
                spawnRate = 1f/10,
                spawnWhiteMobAtStart = false,
                
                maxEnemiesPerSpawn = 8,
                
                pointerSpawnProbability = 1/10f + id1,
                fistSpawnProbability = 1/10f + id2,
                blueFaceSpawnProbability = 1/10f + id3,
                redFaceSpawnProbability = 1/10f + id4,
                greenFaceSpawnProbability = 1/10f + id5
            });

            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 1,
                spawnRate = 999,
                spawnWhiteMobAtStart = false,
                
                maxEnemiesPerSpawn = 0,
                
                pointerSpawnProbability = 0,
                fistSpawnProbability = 0,
                blueFaceSpawnProbability = 0,
                redFaceSpawnProbability = 0,
                greenFaceSpawnProbability = 0
            });
            
            //=================
            
            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 12,
                spawnRate = 1f/10,
                spawnWhiteMobAtStart = true,
                whiteMobSize = 6,
                
                maxEnemiesPerSpawn = 8,
                
                pointerSpawnProbability = 1/10f + id1,
                fistSpawnProbability = 1/10f + id2,
                blueFaceSpawnProbability = 1/10f + id3,
                redFaceSpawnProbability = 1/10f + id4,
                greenFaceSpawnProbability = 1/10f + id5
            });
        }
        private void StartManager() => currentWaveIndex = 0;
        private void Update()
        {
            if (waves.Count == 0 || Clock.Instance == null) return;
            float currentTime = Clock.Instance.ElapsedTime;
            if (currentTime > 0 && currentWaveIndex == 0)
                NextWave();
            else if (currentTime - waveStartTime >= waveDuration)
                NextWave();
        }
        private void NextWave()
        {
            if (currentWaveIndex >= waves.Count)
                throw new System.Exception("No wave available");
            StartWave(currentWaveIndex);
            currentWaveIndex++;
        }
        private void StartWave(int index)
        {
            if (index == 12)
            {
                BlueFace.Instance.FadeIn();
                Spawner.NewEnemyBoss(new Vector2(-11f, 0));//-10 мало
            }
            waveStartTime = Clock.Instance.ElapsedTime;
            var config = waves[index];
            
            if (spawnCoroutine != null) StopCoroutine(spawnCoroutine);
            spawnCoroutine = StartCoroutine(SpawnRoutine(config));
            
            if (index % 3 != 0)
                Spawner.NewEnemyChallengeFilter();
            if (config.firstAppearance != null)
                config.firstAppearance.Invoke(GetRandomPos());
            if (config.spawnWhiteMobAtStart)
                for (int i = 0; i < config.whiteMobSize; i++)
                    Spawner.NewEnemyGoat(GetRandomPos());
        }
        private IEnumerator SpawnRoutine(WaveConfig config)
        {
            float adjustedRate = config.spawnRate;
            while (true)
            {
                yield return new WaitForSeconds(adjustedRate);
                
                if (!Clock.Instance.IsRunning)
                    while (!Clock.Instance.IsRunning)
                        yield return null;
                
                int spawnCount = Random.Range(1, config.maxEnemiesPerSpawn + 1);
                for (int i = 0; i < spawnCount; i++)
                {
                    SpawnRandomEnemy(config);
                }
            }
        }
        private void SpawnRandomEnemy(WaveConfig config)
        {
            if (GetActiveCount<Enemy>() >= config.maxNumOfEnemiesOnScreen) return;
            
            var choice = GetWeightedRandom(
                new float[]
                    {
                        config.pointerSpawnProbability,
                        config.fistSpawnProbability,
                        config.blueFaceSpawnProbability,
                        config.redFaceSpawnProbability,
                        config.greenFaceSpawnProbability
                    }
                );
            
            if (choice == config.greenFaceSpawnProbability)
                Spawner.NewEnemyGreenFace(GetRandomPos());
            else if (choice == config.redFaceSpawnProbability)
                Spawner.NewEnemyRedFace(GetRandomPos());
            else if (choice == config.blueFaceSpawnProbability)
                Spawner.NewEnemyBlueFace(GetRandomPos());
            else if (choice == config.fistSpawnProbability)
                Spawner.NewEnemyFist(GetRandomPos());
            else if (choice == config.pointerSpawnProbability)
                Spawner.NewEnemyPointer(GetRandomPos());
            else
                Spawner.NewEnemyGoat(GetRandomPos());
        }
        public static float GetWeightedRandom(float[] probabilities)
        {
            var roll = Random.value;
            var accumulated = 0.0f;
            foreach (float p in probabilities)
            {
                accumulated += p;
                if (roll < accumulated)
                {
                    return p;
                }
            }
            return 1.0f - probabilities.Sum();
        }
        private int GetActiveCount<T>() where T : MonoBehaviour
        {
            return Object.FindObjectsByType<T>(FindObjectsSortMode.None).Length;
        }
        public Vector2 GetRandomPos()
        {
            if (aspectHandler == null) return Vector2.zero;
            int side = 0;//Random.Range(0, 4);
            float buffer = 2.5f;
            switch (side)
            {
                case 0: return new Vector2(-aspectHandler.Width - buffer, Random.Range(-aspectHandler.Height, aspectHandler.Height));
                case 1: return new Vector2(aspectHandler.Width + buffer, Random.Range(-aspectHandler.Height, aspectHandler.Height));
                case 2: return new Vector2(Random.Range(-aspectHandler.Width, aspectHandler.Width), aspectHandler.Height + buffer);
                default: return new Vector2(Random.Range(-aspectHandler.Width, aspectHandler.Width), -aspectHandler.Height - buffer);
            }
        }
    }
}