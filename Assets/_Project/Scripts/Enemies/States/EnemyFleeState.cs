using UnityEngine;

public class EnemyFleeState : MonoBehaviour, IEnemyState
{
    public void Handle(EnemyBase enemy)
    {
        enemy.PerformFlee();
    }
}
