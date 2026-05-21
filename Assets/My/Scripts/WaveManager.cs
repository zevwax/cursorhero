using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

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
                maxNumOfEnemiesOnScreen = 2,
                spawnRate = 1.2f*2f,
                spawnWhiteMobAtStart = true,
                whiteMobSize = 4,
                maxEnemiesPerSpawn = 1,
                yellowLimit = 0,
                cyanLimit = 0
            });

            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 3,
                spawnRate = 1.1f,
                spawnWhiteMobAtStart = false,
                
                maxEnemiesPerSpawn = 2,
                yellowLimit = 1,
                cyanLimit = 0
            });

            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 1,
                spawnRate = 1f*2f,
                spawnWhiteMobAtStart = true,
                whiteMobSize = 4,
                maxEnemiesPerSpawn = 3,
                yellowLimit = 5,
                cyanLimit = 0
            });

            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 3,
                spawnRate = 1.1f*1.5f,
                spawnWhiteMobAtStart = false,
                
                maxEnemiesPerSpawn = 4,
                yellowLimit = 0,
                cyanLimit = 1
            });

            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 4,
                spawnRate = 1f,
                spawnWhiteMobAtStart = false,
                
                maxEnemiesPerSpawn = 5,
                yellowLimit = 5,
                cyanLimit = 5
            });

            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 2,
                spawnRate = 0.9f*2f,
                spawnWhiteMobAtStart = true,
                whiteMobSize = 8,
                maxEnemiesPerSpawn = 6,
                yellowLimit = 999,
                cyanLimit = 999
            });
            
            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 4,
                spawnRate = 1f*1.5f,
                spawnWhiteMobAtStart = false,
                
                maxEnemiesPerSpawn = 7,
                yellowLimit = 999,
                cyanLimit = 999
            });
            
            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 5,
                spawnRate = 0.9f,
                spawnWhiteMobAtStart = false,
                
                maxEnemiesPerSpawn = 8,
                yellowLimit = 999,
                cyanLimit = 999
            });

            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 3,
                spawnRate = 0.8f*2f,
                spawnWhiteMobAtStart = true,
                whiteMobSize = 12,
                maxEnemiesPerSpawn = 9,
                yellowLimit = 999,
                cyanLimit = 999
            });

            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 5,
                spawnRate = 0.9f*1.5f,
                spawnWhiteMobAtStart = false,
                
                maxEnemiesPerSpawn = 10,
                yellowLimit = 999,
                cyanLimit = 999
            });

            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 6,
                spawnRate = 0.8f,
                spawnWhiteMobAtStart = false,
                
                maxEnemiesPerSpawn = 11,
                yellowLimit = 999,
                cyanLimit = 999
            });

            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 4,
                spawnRate = 0.7f*2f,
                spawnWhiteMobAtStart = true,
                whiteMobSize = 16,
                maxEnemiesPerSpawn = 12,
                yellowLimit = 999,
                cyanLimit = 999
            });

            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 6,
                spawnRate = 0.8f*1.5f,
                spawnWhiteMobAtStart = false,
                
                maxEnemiesPerSpawn = 13,
                yellowLimit = 999,
                cyanLimit = 999
            });

            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 7,
                spawnRate = 0.7f,
                spawnWhiteMobAtStart = false,
                
                maxEnemiesPerSpawn = 14,
                yellowLimit = 999,
                cyanLimit = 999
            });

            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 5,
                spawnRate = 0.6f,
                spawnWhiteMobAtStart = true,
                whiteMobSize = 20,
                maxEnemiesPerSpawn = 15,
                yellowLimit = 999,
                cyanLimit = 999
            });
            
            //=================

            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 2*4,
                spawnRate = 1.2f*2f,
                spawnWhiteMobAtStart = true,
                whiteMobSize = 4,
                maxEnemiesPerSpawn = 1,
                yellowLimit = 0,
                cyanLimit = 0
            });

            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 3*4,
                spawnRate = 1.1f,
                spawnWhiteMobAtStart = false,
                
                maxEnemiesPerSpawn = 2,
                yellowLimit = 1,
                cyanLimit = 0
            });

            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 1*4,
                spawnRate = 1f*2f,
                spawnWhiteMobAtStart = true,
                whiteMobSize = 4,
                maxEnemiesPerSpawn = 3,
                yellowLimit = 5,
                cyanLimit = 0
            });

            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 3*4,
                spawnRate = 1.1f*1.5f,
                spawnWhiteMobAtStart = false,
                
                maxEnemiesPerSpawn = 4,
                yellowLimit = 0,
                cyanLimit = 1
            });

            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 4*4,
                spawnRate = 1f,
                spawnWhiteMobAtStart = false,
                
                maxEnemiesPerSpawn = 5,
                yellowLimit = 5,
                cyanLimit = 5
            });

            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 2*4,
                spawnRate = 0.9f*2f,
                spawnWhiteMobAtStart = true,
                whiteMobSize = 8,
                maxEnemiesPerSpawn = 6,
                yellowLimit = 999,
                cyanLimit = 999
            });
            
            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 4*4,
                spawnRate = 1f*1.5f,
                spawnWhiteMobAtStart = false,
                
                maxEnemiesPerSpawn = 7,
                yellowLimit = 999,
                cyanLimit = 999
            });
            
            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 5*4,
                spawnRate = 0.9f,
                spawnWhiteMobAtStart = false,
                
                maxEnemiesPerSpawn = 8,
                yellowLimit = 999,
                cyanLimit = 999
            });

            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 3*4,
                spawnRate = 0.8f*2f,
                spawnWhiteMobAtStart = true,
                whiteMobSize = 12,
                maxEnemiesPerSpawn = 9,
                yellowLimit = 999,
                cyanLimit = 999
            });

            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 5*4,
                spawnRate = 0.9f*1.5f,
                spawnWhiteMobAtStart = false,
                
                maxEnemiesPerSpawn = 10,
                yellowLimit = 999,
                cyanLimit = 999
            });

            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 6*4,
                spawnRate = 0.8f,
                spawnWhiteMobAtStart = false,
                
                maxEnemiesPerSpawn = 11,
                yellowLimit = 999,
                cyanLimit = 999
            });

            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 4*4,
                spawnRate = 0.7f*2f,
                spawnWhiteMobAtStart = true,
                whiteMobSize = 16,
                maxEnemiesPerSpawn = 12,
                yellowLimit = 999,
                cyanLimit = 999
            });

            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 6*4,
                spawnRate = 0.8f*1.5f,
                spawnWhiteMobAtStart = false,
                
                maxEnemiesPerSpawn = 13,
                yellowLimit = 999,
                cyanLimit = 999
            });

            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 7*4,
                spawnRate = 0.7f,
                spawnWhiteMobAtStart = false,
                
                maxEnemiesPerSpawn = 14,
                yellowLimit = 999,
                cyanLimit = 999
            });

            waves.Add(new WaveConfig {
                maxNumOfEnemiesOnScreen = 5*4,
                spawnRate = 0.6f,
                spawnWhiteMobAtStart = true,
                whiteMobSize = 20,
                maxEnemiesPerSpawn = 15,
                yellowLimit = 999,
                cyanLimit = 999
            });
        }

        private void StartManager()
        {
            Clock.Instance.Refresh();
            currentWaveIndex = 0;
        }

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
            waveStartTime = Clock.Instance.ElapsedTime;
            WaveConfig config = waves[index];
            
            if (spawnCoroutine != null) StopCoroutine(spawnCoroutine);
            spawnCoroutine = StartCoroutine(SpawnRoutine(config));

            if (config.spawnWhiteMobAtStart)
            {
                for (int i = 0; i < config.whiteMobSize; i++)
                    Spawner.NewWhite(GetRandomPos());
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
            
            int choice = Random.Range(0, 3); 

            if (choice == 2 && GetActiveCount<EnemyGoat>() < config.cyanLimit)
                Spawner.NewCyan(GetRandomPos());
            else if (choice == 1 && GetActiveCount<EnemyBlackGlove>() < config.yellowLimit)
                Spawner.NewYellow(GetRandomPos());
            else
                Spawner.NewWhite(GetRandomPos());
        }
        private int GetActiveCount<T>() where T : MonoBehaviour
        {
            return Object.FindObjectsByType<T>(FindObjectsSortMode.None).Length;
        }
        public Vector2 GetRandomPos()
        {
            if (aspectHandler == null) return Vector2.zero;
            int side = Random.Range(0, 4);
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