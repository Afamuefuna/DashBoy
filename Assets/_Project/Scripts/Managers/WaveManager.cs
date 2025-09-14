using System.Collections;
using UnityEngine;

[System.Serializable]
public class Wave
{
    [Header("Enemies")]
    public int enemyTypeA;
    public int enemyTypeB;

    [Header("PowerUps")]
    public int speedBoosts;
    public int doubleDashes;

    [Header("Settings")]
    public float spawnDelay = 0.5f;
    public float waveDelay = 3f;
}

public class WaveManager : MonoBehaviour
{
    [Header("Prefabs")]
    public EnemySpawner.SpawnEntry enemyTypeASpawnEntry;
    public EnemySpawner.SpawnEntry enemyTypeBSpawnEntry;
    public GameObject speedBoostPrefab;
    public GameObject doubleDashPrefab;

    [Header("Spawn Points")]
    public Transform[] powerupSpawnPoints;
    public Transform[] enemySpawnPoints;

    [Header("Waves")]
    public Wave[] waves;

    private int currentWave = 0;

    [SerializeField] EnemySpawner enemySpawner;

    void Start()
    {
        StartCoroutine(StartWaveRoutine());
    }

    private IEnumerator StartWaveRoutine()
    {
        while (currentWave < waves.Length)
        {
            Wave wave = waves[currentWave];
            Debug.Log($"Wave {currentWave + 1} starting!");

            for (int i = 0; i < wave.enemyTypeA; i++)
            {
                Transform point = enemySpawnPoints[Random.Range(0, enemySpawnPoints.Length)];
                enemySpawner.SpawnEnemy(enemyTypeASpawnEntry, point.position);
                yield return new WaitForSeconds(wave.spawnDelay);
            }

            for (int i = 0; i < wave.enemyTypeB; i++)
            {
                Transform point = enemySpawnPoints[Random.Range(0, enemySpawnPoints.Length)];
                enemySpawner.SpawnEnemy(enemyTypeBSpawnEntry, point.position);
                yield return new WaitForSeconds(wave.spawnDelay);
            }

            for (int i = 0; i < wave.speedBoosts; i++)
                SpawnPowerUp(speedBoostPrefab);

            for (int i = 0; i < wave.doubleDashes; i++)
                SpawnPowerUp(doubleDashPrefab);

            yield return new WaitUntil(() => enemySpawner.EnemyCount <= 0);

            Debug.Log($"Wave {currentWave + 1} complete!");
            currentWave++;
            GameEvents.OnGameUpdate?.Invoke("Wave Complete");
            yield return new WaitForSeconds(wave.waveDelay);
        }

        Debug.Log("All Waves Complete!");
    }

    private void SpawnPowerUp(GameObject prefab)
    {
        Transform point = powerupSpawnPoints[Random.Range(0, powerupSpawnPoints.Length)];
        Instantiate(prefab, point.position, Quaternion.identity);
    }
}
