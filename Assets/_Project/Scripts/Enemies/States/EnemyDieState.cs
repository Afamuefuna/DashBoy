using UnityEngine;

public class EnemyDieState : MonoBehaviour, IEnemyState
{
    public void Handle(EnemyBase enemy)
    {
        enemy.PerformDie();
    }
}
