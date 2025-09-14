using UnityEngine;

public class EnemyAttackState : MonoBehaviour, IEnemyState
{
    public void Handle(EnemyBase enemy)
    {
        enemy.PerformAttack();
    }
}
