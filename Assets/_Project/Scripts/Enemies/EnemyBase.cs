using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class EnemyBase : MonoBehaviour
{
    [Header("Enemy Data")]
    public EnemyData data;
    public int currentHealth = 1;

    [Header("References")]
    protected Transform player;
    protected Rigidbody rb;

    [Header("Status")]
    public bool isAttacking = false;

    [Header("States")]
    protected IEnemyState _idleState;
    protected IEnemyState _pursueState;
    protected IEnemyState _attackState;
    protected IEnemyState _fleeState;
    protected IEnemyState _dieState;
    protected IEnemyState _currentState;
    private EnemyStateContext _enemyStateContext;

    [Header("Events")]
    public UnityEvent OnDie = new UnityEvent();


    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    protected virtual void Start()
    {
        _enemyStateContext = new EnemyStateContext(this);
        _idleState = gameObject.AddComponent<EnemyIdleState>();
        _pursueState = gameObject.AddComponent<EnemyPursueState>();
        _attackState = gameObject.AddComponent<EnemyAttackState>();
        _fleeState = gameObject.AddComponent<EnemyFleeState>();
        _dieState = gameObject.AddComponent<EnemyDieState>();

        SwitchEnemyState(_idleState);

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void OnEnable()
    {
        Reset();
    }

    public virtual void PerformIDle() { }
    public virtual void PerformPursue() { }
    public virtual void PerformAttack() { }
    public virtual void PerformFlee() { }
    public virtual void PerformDie() { }

    public void SwitchEnemyState(IEnemyState newState)
    {
        if (newState == null) return;
        _currentState = newState;
        _enemyStateContext.Transition(newState);
    }

    protected virtual void Reset()
    {
        currentHealth = data != null ? data.health : 1;
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        isAttacking = false;
        SwitchEnemyState(_idleState);
    }

    protected virtual void Update()
    {
        if (_currentState == _dieState) return;

        if (isAttacking) return;

        if (player == null) return;
        float dist = Vector3.Distance(transform.position, player.position);

        if (dist <= (data != null ? data.attackRange : 1.2f))
        {
            SwitchEnemyState(_attackState);
        }
        else if (dist <= (data != null ? data.detectionRange : 10f))
        {
            SwitchEnemyState(_pursueState);
        }
        else
        {
            SwitchEnemyState(_idleState);
        }
    }

    public virtual void TakeDamage(int dmg, Vector3 knockbackDirection)
    {
        if (_currentState == _dieState) return;

        currentHealth -= dmg;
        if (currentHealth <= 0) Die();
    }

    protected virtual void Die()
    {
        OnDie.Invoke();

        SwitchEnemyState(_dieState);

        ReturnToPool returnToPool = GetComponent<ReturnToPool>();
        if (returnToPool != null)
        {
            returnToPool.Release();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
