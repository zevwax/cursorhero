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

            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 10,
                spawnRate = 1f/(1*0.75f),
                spawnWhiteMobAtStart = true,
                whiteMobSize = 3,
                maxEnemiesPerSpawn = 2,
                pointerSpawnProbability = 0,
                gloveSpawnProbability = 0,
                goatSpawnProbability = 0
            });

            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 15,
                spawnRate = 1f/2,
                spawnWhiteMobAtStart = false,
                
                maxEnemiesPerSpawn = 4,
                pointerSpawnProbability = 0.15f,
                gloveSpawnProbability = 0,
                goatSpawnProbability = 0
            });

            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 20,
                spawnRate = 1f/3,
                spawnWhiteMobAtStart = true,
                whiteMobSize = 10,
                maxEnemiesPerSpawn = 6,
                pointerSpawnProbability = 0,
                gloveSpawnProbability = 0.15f,
                goatSpawnProbability = 0
            });

            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 15,
                spawnRate = 1f/(3*0.75f),
                spawnWhiteMobAtStart = false,
                
                maxEnemiesPerSpawn = 6,
                pointerSpawnProbability = 0,
                gloveSpawnProbability = 0,
                goatSpawnProbability = 0.15f
            });

            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 20/4,
                spawnRate = 1f/4,
                spawnWhiteMobAtStart = false,
                
                maxEnemiesPerSpawn = 8,
                pointerSpawnProbability = 0.25f,
                gloveSpawnProbability = 0.25f,
                goatSpawnProbability = 0.25f
            });

            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 25,
                spawnRate = 1f/5,
                spawnWhiteMobAtStart = true,
                whiteMobSize = 15,
                maxEnemiesPerSpawn = 10,
                pointerSpawnProbability = 0,
                gloveSpawnProbability = 0,
                goatSpawnProbability = 0
            });
            
            //====
            
            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 12,
                spawnRate = 1f/(5*0.5f),
                spawnWhiteMobAtStart = false,
                
                maxEnemiesPerSpawn = 10,
                pointerSpawnProbability = 0,
                gloveSpawnProbability = 0,
                goatSpawnProbability = 1
            });
            
            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 25,
                spawnRate = 1f/6,
                spawnWhiteMobAtStart = false,
                
                maxEnemiesPerSpawn = 12,
                pointerSpawnProbability = 0,
                gloveSpawnProbability = 1,
                goatSpawnProbability = 0
            });

            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 30,
                spawnRate = 1f/7,
                spawnWhiteMobAtStart = true,
                whiteMobSize = 20,
                maxEnemiesPerSpawn = 14,
                pointerSpawnProbability = 0,
                gloveSpawnProbability = 0,
                goatSpawnProbability = 0
            });
            
            //====
            
            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 15,
                spawnRate = 1f/(7*0.5f),
                spawnWhiteMobAtStart = false,
                
                maxEnemiesPerSpawn = 14,
                pointerSpawnProbability = 0.08f,
                gloveSpawnProbability = 0.08f,
                goatSpawnProbability = 0
            });

            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 30,
                spawnRate = 1f/8,
                spawnWhiteMobAtStart = false,
                
                maxEnemiesPerSpawn = 16,
                pointerSpawnProbability = 0.08f,
                gloveSpawnProbability = 0.08f,
                goatSpawnProbability = 0
            });

            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 35,
                spawnRate = 1f/9,
                spawnWhiteMobAtStart = true,
                whiteMobSize = 25,
                maxEnemiesPerSpawn = 18,
                pointerSpawnProbability = 0.08f,
                gloveSpawnProbability = 0.08f,
                goatSpawnProbability = 0
            });
            
            //====
            
            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 30,
                spawnRate = 1f/(9*0.75f),
                spawnWhiteMobAtStart = false,
                
                maxEnemiesPerSpawn = 18,
                pointerSpawnProbability = 0.08f,
                gloveSpawnProbability = 0.08f,
                goatSpawnProbability = 0
            });

            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 35,
                spawnRate = 1f/10,
                spawnWhiteMobAtStart = false,
                
                maxEnemiesPerSpawn = 20,
                pointerSpawnProbability = 0.08f,
                gloveSpawnProbability = 0.08f,
                goatSpawnProbability = 0
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
                maxNumOfEnemiesOnScreen = 2*4,
                spawnRate = 1f*2f/2f,
                spawnWhiteMobAtStart = true,
                whiteMobSize = 4,
                maxEnemiesPerSpawn = 1,
                gloveSpawnProbability = 0.5f,
                goatSpawnProbability = 0.5f
            });

            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 3*4,
                spawnRate = .9f/2f,
                spawnWhiteMobAtStart = false,
                
                maxEnemiesPerSpawn = 2,
                gloveSpawnProbability = 0.5f,
                goatSpawnProbability = 0.5f
            });

            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 1*4,
                spawnRate = .8f*2f/2f,
                spawnWhiteMobAtStart = true,
                whiteMobSize = 4,
                maxEnemiesPerSpawn = 3,
                gloveSpawnProbability = 0.5f,
                goatSpawnProbability = 0.5f
            });

            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 3*4,
                spawnRate = .9f*1.5f/2f,
                spawnWhiteMobAtStart = false,
                
                maxEnemiesPerSpawn = 4,
                gloveSpawnProbability = 0.5f,
                goatSpawnProbability = 0.5f
            });

            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 4*4,
                spawnRate = .8f/2f,
                spawnWhiteMobAtStart = false,
                
                maxEnemiesPerSpawn = 5,
                gloveSpawnProbability = 0.5f,
                goatSpawnProbability = 0.5f
            });

            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 2*4,
                spawnRate = .7f*2f/2f,
                spawnWhiteMobAtStart = true,
                whiteMobSize = 8,
                maxEnemiesPerSpawn = 6,
                gloveSpawnProbability = 0.5f,
                goatSpawnProbability = 0.5f
            });
            
            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 4*4,
                spawnRate = .8f*1.5f/2f,
                spawnWhiteMobAtStart = false,
                
                maxEnemiesPerSpawn = 7,
                gloveSpawnProbability = 0.5f,
                goatSpawnProbability = 0.5f
            });
            
            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 5*4,
                spawnRate = .7f/2f,
                spawnWhiteMobAtStart = false,
                
                maxEnemiesPerSpawn = 8,
                gloveSpawnProbability = 0.5f,
                goatSpawnProbability = 0.5f
            });

            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 3*4,
                spawnRate = .6f*2f/2f,
                spawnWhiteMobAtStart = true,
                whiteMobSize = 12,
                maxEnemiesPerSpawn = 9,
                gloveSpawnProbability = 0.5f,
                goatSpawnProbability = 0.5f
            });

            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 5*4,
                spawnRate = .7f*1.5f/2f,
                spawnWhiteMobAtStart = false,
                
                maxEnemiesPerSpawn = 10,
                gloveSpawnProbability = 0.5f,
                goatSpawnProbability = 0.5f
            });

            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 6*4,
                spawnRate = .6f/2f,
                spawnWhiteMobAtStart = false,
                
                maxEnemiesPerSpawn = 11,
                gloveSpawnProbability = 0.5f,
                goatSpawnProbability = 0.5f
            });

            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 4*4,
                spawnRate = .5f*2f/2f,
                spawnWhiteMobAtStart = true,
                whiteMobSize = 16,
                maxEnemiesPerSpawn = 12,
                gloveSpawnProbability = 0.5f,
                goatSpawnProbability = 0.5f
            });

            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 6*4,
                spawnRate = .6f*1.5f/2f,
                spawnWhiteMobAtStart = false,
                
                maxEnemiesPerSpawn = 13,
                gloveSpawnProbability = 0.5f,
                goatSpawnProbability = 0.5f
            });

            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 7*4,
                spawnRate = .5f/2f,
                spawnWhiteMobAtStart = false,
                
                maxEnemiesPerSpawn = 14,
                gloveSpawnProbability = 0.5f,
                goatSpawnProbability = 0.5f
            });

            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 5*4,
                spawnRate = .4f/2f,
                spawnWhiteMobAtStart = true,
                whiteMobSize = 20,
                maxEnemiesPerSpawn = 15,
                gloveSpawnProbability = 0.5f,
                goatSpawnProbability = 0.5f
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
                Spawner.NewEnemyFuck(new Vector2(-11f, 0));//-10 мало
            }
            waveStartTime = Clock.Instance.ElapsedTime;
            var config = waves[index];
            
            if (spawnCoroutine != null) StopCoroutine(spawnCoroutine);
            spawnCoroutine = StartCoroutine(SpawnRoutine(config));

            if (config.spawnWhiteMobAtStart)
            {
                for (int i = 0; i < config.whiteMobSize; i++)
                    Spawner.NewEnemyGrabber(GetRandomPos());
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
                Spawner.NewEnemyGoat(GetRandomPos());
            else if (choice == config.gloveSpawnProbability)
                Spawner.NewEnemyGlove(GetRandomPos());
            else if (choice == config.pointerSpawnProbability)
                Spawner.NewEnemyPointer(GetRandomPos());
            else
                Spawner.NewEnemyGrabber(GetRandomPos());
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
            float buffer = 1f;
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