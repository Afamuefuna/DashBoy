using UnityEngine;

public class EnemyPursueState : MonoBehaviour, IEnemyState
{
    public void Handle(EnemyBase enemy)
    {
        enemy.PerformPursue();
    }
}
