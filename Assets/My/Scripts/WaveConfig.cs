using System;
using UnityEngine;
public class WaveConfig
{
    public int maxNumOfEnemiesOnScreen;
    public float spawnRate;
    public bool spawnWhiteMobAtStart;
    public int whiteMobSize;
    public int maxEnemiesPerSpawn;
    public float pointerSpawnProbability;
    public float fistSpawnProbability;
    public float blueFaceSpawnProbability;
    public float redFaceSpawnProbability;
    public float greenFaceSpawnProbability;
    public Action<Vector2> firstAppearance;
}