using UnityEngine;

public class EnemyIdleState : MonoBehaviour, IEnemyState
{
    public void Handle(EnemyBase enemy)
    {
        enemy.PerformIDle();
    }
}
