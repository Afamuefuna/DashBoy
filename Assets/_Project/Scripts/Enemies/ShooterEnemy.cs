using UnityEngine;

public class ShooterEnemy : EnemyBase
{
    [Header("Shooter Settings")]
    public ProjectileData projectileData;
    [SerializeField] private float fireRate;
    [SerializeField] private float desiredDistance = 6f;
    [SerializeField] private float fireTimer = 0f;

    [Header("Dependencies")]
    public ObjectPoolHandler projectilePool;

    protected override void Update()
    {
        base.Update();

        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);

        if (_currentState == _pursueState || _currentState == _attackState)
        {
            Vector3 dir = (player.position - transform.position).normalized;
            if (dist < desiredDistance * 0.9f)
                rb.velocity = -dir * (data != null ? data.moveSpeed : 2f);
            else if (dist > desiredDistance * 1.1f)
                rb.velocity = dir * (data != null ? data.moveSpeed : 2f);
            else
                rb.velocity = Vector3.zero;

            fireTimer -= Time.deltaTime;
            if (fireTimer <= 0f)
            {
                FireAtPlayer();
                fireTimer = fireRate;
            }
        }
        else
        {
            rb.velocity = Vector3.zero;
        }

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
    }

    private void FireAtPlayer()
    {
        if (projectilePool == null || projectileData == null || player == null) return;
        Vector3 spawnPos = transform.position + (player.position - transform.position).normalized * 1.0f + Vector3.up * 0.2f;
        GameObject go = projectilePool.Get(spawnPos, Quaternion.LookRotation((player.position - transform.position).normalized));
        var proj = go.GetComponent<Projectile>();
        if (proj != null)
        {
            proj.Init(projectileData);
        }
    }
}
