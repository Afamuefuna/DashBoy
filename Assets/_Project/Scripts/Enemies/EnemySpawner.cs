using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class SpawnEntry
    {
        public ObjectPoolHandler pool;
    }

    public float spawnRadius = 8f;
    public int EnemyCount = 0;

    [SerializeField] ObjectPoolHandler projectilePool;

    public void SpawnEnemy(SpawnEntry entry, Vector3 spawnPos)
    {
        GameObject go = entry.pool.Get(spawnPos, Quaternion.identity);
        go.transform.position = new Vector3(spawnPos.x, 1, spawnPos.z);
        if (go.GetComponent<ShooterEnemy>() != null)
        {
            go.GetComponent<ShooterEnemy>().projectilePool = projectilePool;
        }
        EnemyCount++;

        EnemyBase enemy = go.GetComponent<EnemyBase>();
        if (enemy != null)
        {
            enemy.OnDie.RemoveAllListeners();
            enemy.OnDie.AddListener(() => { EnemyCount--; });
        }
    }
}