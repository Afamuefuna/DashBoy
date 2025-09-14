using UnityEngine;

[CreateAssetMenu(menuName = "DashBoy/EnemyData")]
public class EnemyData : ScriptableObject
{
    public float moveSpeed = 3f;
    public int health = 1;
    public float detectionRange = 10f;
    public float attackRange = 1.2f;
    public float spawnWeight = 1f;
}
