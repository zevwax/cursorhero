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
            
            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 2,
                spawnRate = 1f/(1*0.75f),
                spawnWhiteMobAtStart = false,
                
                maxEnemiesPerSpawn = 4,
                
                pointerSpawnProbability = 0,
                gloveSpawnProbability = 0,
                goatSpawnProbability = 0
            });

            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 3,
                spawnRate = 1f/2,
                spawnWhiteMobAtStart = false,
                
                maxEnemiesPerSpawn = 4,
                
                firstAppearance = (pos) => Spawner.NewEnemyMajor(pos),
                pointerSpawnProbability = 0,
                gloveSpawnProbability = 0,
                goatSpawnProbability = 0
            });

            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 4,
                spawnRate = 1f/3,
                spawnWhiteMobAtStart = true,
                whiteMobSize = 1,
                maxEnemiesPerSpawn = 4,
                
                firstAppearance = (pos) => Spawner.NewEnemyStar(pos),
                pointerSpawnProbability = 0,
                gloveSpawnProbability = 0,
                goatSpawnProbability = 0
            });

            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 3,
                spawnRate = 1f/(3*0.75f),
                spawnWhiteMobAtStart = false,
                
                maxEnemiesPerSpawn = 4,
                
                pointerSpawnProbability = 1/5f + id1,
                gloveSpawnProbability = 1/5f + id2,
                goatSpawnProbability = 0
            });

            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 4,
                spawnRate = 1f/4,
                spawnWhiteMobAtStart = false,
                
                maxEnemiesPerSpawn = 4,
                
                firstAppearance = (pos) => Spawner.NewEnemyFlesh(pos),
                pointerSpawnProbability = 0,
                gloveSpawnProbability = 0,
                goatSpawnProbability = 0
            });

            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 5,
                spawnRate = 1f/5,
                spawnWhiteMobAtStart = true,
                whiteMobSize = 2,
                maxEnemiesPerSpawn = 4,
                pointerSpawnProbability = 1/5f + id1,
                gloveSpawnProbability = 0,
                goatSpawnProbability = 0
            });
            
            //====
            
            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 4,
                spawnRate = 1f/(5*0.5f),
                spawnWhiteMobAtStart = false,
                
                maxEnemiesPerSpawn = 6,
                pointerSpawnProbability = 0,
                gloveSpawnProbability = 1/5f + id2,
                goatSpawnProbability = 1/5f + id3
            });
            
            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 5,
                spawnRate = 1f/6,
                spawnWhiteMobAtStart = false,
                
                maxEnemiesPerSpawn = 6,
                pointerSpawnProbability = 1,
                gloveSpawnProbability = 0,
                goatSpawnProbability = 0
            });

            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 6,
                spawnRate = 1f/7,
                spawnWhiteMobAtStart = true,
                whiteMobSize = 3,
                maxEnemiesPerSpawn = 6,
                pointerSpawnProbability = 0,
                gloveSpawnProbability = 0,
                goatSpawnProbability = 0
            });
            
            //====
            
            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 5,
                spawnRate = 1f/(7*0.5f),
                spawnWhiteMobAtStart = false,
                
                maxEnemiesPerSpawn = 6,
                pointerSpawnProbability = 1/5f + id1,
                gloveSpawnProbability = 1/5f + id2,
                goatSpawnProbability = 1/5f + id3
            });

            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 6,
                spawnRate = 1f/8,
                spawnWhiteMobAtStart = false,
                
                maxEnemiesPerSpawn = 6,
                pointerSpawnProbability = 0,
                gloveSpawnProbability = 1,
                goatSpawnProbability = 0
            });

            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 7,
                spawnRate = 1f/9,
                spawnWhiteMobAtStart = true,
                whiteMobSize = 4,
                maxEnemiesPerSpawn = 6,
                pointerSpawnProbability = 1/5f + id1,
                gloveSpawnProbability = 1/5f + id2,
                goatSpawnProbability = 1/5f + id3
            });
            
            //====
            
            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 6,
                spawnRate = 1f/(9*0.75f),
                spawnWhiteMobAtStart = false,
                
                maxEnemiesPerSpawn = 8,
                pointerSpawnProbability = 1/5f + id1,
                gloveSpawnProbability = 1/5f + id2,
                goatSpawnProbability = 1/5f + id3
            });

            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 7,
                spawnRate = 1f/10,
                spawnWhiteMobAtStart = false,
                
                maxEnemiesPerSpawn = 8,
                pointerSpawnProbability = 0,
                gloveSpawnProbability = 0,
                goatSpawnProbability = 1
            });

            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 1,
                spawnRate = 999,
                spawnWhiteMobAtStart = false,
                
                maxEnemiesPerSpawn = 0,
                pointerSpawnProbability = 0,
                gloveSpawnProbability = 0,
                goatSpawnProbability = 0
            });
            
            //=================
            
            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 7,
                spawnRate = 1f/10,
                spawnWhiteMobAtStart = false,
                
                maxEnemiesPerSpawn = 10,
                pointerSpawnProbability = 1/5f + id1,
                gloveSpawnProbability = 1/5f + id2,
                goatSpawnProbability = 1/5f + id3
            });
            
            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 7,
                spawnRate = 1f/10,
                spawnWhiteMobAtStart = false,
                
                maxEnemiesPerSpawn = 10,
                pointerSpawnProbability = 1/5f + id1,
                gloveSpawnProbability = 1/5f + id2,
                goatSpawnProbability = 1/5f + id3
            });
            
            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 14,
                spawnRate = 1f/20,
                spawnWhiteMobAtStart = true,
                whiteMobSize = 6,
                maxEnemiesPerSpawn = 10,
                pointerSpawnProbability = 1/5f + id1,
                gloveSpawnProbability = 1/5f + id2,
                goatSpawnProbability = 1/5f + id3
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
                Spawner.NewEnemySkull(new Vector2(-11f, 0));//-10 мало
            }
            waveStartTime = Clock.Instance.ElapsedTime;
            var config = waves[index];
            
            if (spawnCoroutine != null) StopCoroutine(spawnCoroutine);
            spawnCoroutine = StartCoroutine(SpawnRoutine(config));
            
            if (config.firstAppearance != null)
                config.firstAppearance.Invoke(GetRandomPos());
            
            if (config.spawnWhiteMobAtStart)
            {
                for (int i = 0; i < config.whiteMobSize; i++)
                    Spawner.NewEnemyMinor(GetRandomPos());
            }
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
                        config.gloveSpawnProbability,
                        config.goatSpawnProbability
                    }
                );
            
            if (choice == config.goatSpawnProbability)
                Spawner.NewEnemyFlesh(GetRandomPos());
            else if (choice == config.gloveSpawnProbability)
                Spawner.NewEnemyStar(GetRandomPos());
            else if (choice == config.pointerSpawnProbability)
                Spawner.NewEnemyMajor(GetRandomPos());
            else
                Spawner.NewEnemyMinor(GetRandomPos());
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