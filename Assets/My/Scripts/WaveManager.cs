using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

namespace ZevWaxGames.CursorHero
{
    public class WaveManager : MonoBehaviour
    {
        private List<WaveConfig> waves = new List<WaveConfig>();
        private int currentWaveIndex = 0;
        private float waveStartTime = 0;
        private float loopSpeedMultiplier = 1f;
        private AspectRatioHandler aspectHandler;
        private Coroutine spawnCoroutine;

        private void Awake()
        {
            aspectHandler = Camera.main.GetComponent<AspectRatioHandler>();
            SetupWaves();
        }

        private void OnEnable() => EventHolder.OnRunStarted += StartManager;
        private void OnDisable() => EventHolder.OnRunStarted -= StartManager;

        private void SetupWaves()
        {
            waves.Clear();

            waves.Add(new WaveConfig {
                duration = 20f,
                spawnRate = 2.5f,
                spawnWhiteMobAtStart = true,
                whiteMobSize = 4,
                maxEnemiesPerSpawn = 1,
                yellowLimit = 0,
                cyanLimit = 0,
                backgroundSprite = Resources.Load<Sprite>("My/WinXp/Wallpapers/Bliss")
            });

            waves.Add(new WaveConfig {
                duration = 20f,
                spawnRate = 4f,
                spawnWhiteMobAtStart = false,
                maxEnemiesPerSpawn = 3,
                yellowLimit = 1,
                cyanLimit = 0
            });

            waves.Add(new WaveConfig {
                duration = 20f,
                spawnRate = 5f,
                spawnWhiteMobAtStart = true,
                whiteMobSize = 8,
                maxEnemiesPerSpawn = 4,
                yellowLimit = 5,
                cyanLimit = 0
            });

            waves.Add(new WaveConfig {
                duration = 20f,
                spawnRate = 6f,
                spawnWhiteMobAtStart = false,
                whiteMobSize = 8,
                maxEnemiesPerSpawn = 4,
                yellowLimit = 999,
                cyanLimit = 1,
                backgroundSprite = Resources.Load<Sprite>("My/WinXp/Wallpapers/Autumn")
            });

            waves.Add(new WaveConfig {
                duration = 20f,
                spawnRate = 5f,
                spawnWhiteMobAtStart = false,
                whiteMobSize = 8,
                maxEnemiesPerSpawn = 4,
                yellowLimit = 999,
                cyanLimit = 5
            });

            waves.Add(new WaveConfig {
                duration = 20f,
                spawnRate = 4f,
                spawnWhiteMobAtStart = true,
                whiteMobSize = 8,
                maxEnemiesPerSpawn = 4,
                yellowLimit = 0,
                cyanLimit = 999
            });
        }

        private void StartManager()
        {
            Clock.Instance.Refresh(); //Doesnt matter w/ this row or without: it skips the 1st wave
            currentWaveIndex = 0;
            loopSpeedMultiplier = 1f;
            StartWave(0);
        }

        private void Update()
        {
            if (waves.Count == 0 || Clock.Instance == null) return;

            float currentTime = Clock.Instance.ElapsedTime;
            float currentWaveDuration = waves[currentWaveIndex].duration / loopSpeedMultiplier;

            if (currentTime - waveStartTime >= currentWaveDuration)
            {
                NextWave();
            }
        }

        private void NextWave()
        {
            currentWaveIndex++;
            if (currentWaveIndex >= waves.Count)
            {
                currentWaveIndex = 0;
                loopSpeedMultiplier *= 2f; 
            }
            StartWave(currentWaveIndex);
        }

        private void StartWave(int index)
        {
            waveStartTime = Clock.Instance.ElapsedTime;
            WaveConfig config = waves[index];

            if (config.backgroundSprite != null)
                StartCoroutine(RefreshWallpapers(config.backgroundSprite));
            
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
            float adjustedRate = config.spawnRate / loopSpeedMultiplier;
            while (true)
            {
                yield return new WaitForSeconds(adjustedRate);
                int spawnCount = Random.Range(1, config.maxEnemiesPerSpawn + 1);
                for (int i = 0; i < spawnCount; i++)
                {
                    SpawnRandomEnemy(config);
                }
            }
        }

        private void SpawnRandomEnemy(WaveConfig config)
        {
            int choice = Random.Range(0, 3); 

            if (choice == 2 && GetActiveCount<Cyan>() < config.cyanLimit)
                Spawner.NewCyan(GetRandomPos());
            else if (choice == 1 && GetActiveCount<Yellow>() < config.yellowLimit)
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
        private IEnumerator RefreshWallpapers(Sprite newWallpapers)
        {
            var wallpapersBG = GameObject.Find("Wallpapers BG").GetComponent<Image>();
            var wallpapersFG = GameObject.Find("Wallpapers FG").GetComponent<Image>();
            wallpapersFG.sprite = newWallpapers;
            yield return wallpapersFG.DOFade(1f, 1f).WaitForCompletion();
            wallpapersBG.sprite = newWallpapers;
            wallpapersFG.color = new Color(1, 1, 1, 0);
        }
    }
}