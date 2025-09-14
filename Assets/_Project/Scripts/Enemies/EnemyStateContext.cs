using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateContext
{
    public IEnemyState CurrentState { get; set; }

    private readonly EnemyBase _enemyBase;
    public EnemyStateContext(EnemyBase enemyBase)
    {
        this._enemyBase = enemyBase;
    }

    public void Transition()
    {
        CurrentState.Handle(_enemyBase);
    }

    public void Transition(IEnemyState state)
    {
        CurrentState = state;
        CurrentState.Handle(_enemyBase);
    }
}
