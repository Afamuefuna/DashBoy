using System.Collections;
using UnityEngine;

public class ChaserEnemy : EnemyBase
{
    [Header("Chaser Settings")]
    public bool isDashing = false;
    private float attackDelay;
    private float attackDuration = 0.3f;

    protected override void Start()
    {
        base.Start();
        attackDelay = Random.Range(4f, 6f);
    }
    protected override void Update()
    {
        base.Update();
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
    }

    protected override void Reset()
    {
        base.Reset();
        isDashing = false;
    }

    public override void PerformIDle()
    {
        base.PerformIDle();
        rb.velocity = Vector3.zero;
    }

    public override void PerformPursue()
    {
        base.PerformPursue();
        MoveToPlayer();
    }

    public override void PerformAttack()
    {
        base.PerformAttack();
        AttackPlayer();
    }

    public override void PerformFlee()
    {
        base.PerformFlee();
        rb.velocity = -(player.position - transform.position).normalized * (data != null ? data.moveSpeed : 3f);
    }

    public override void PerformDie()
    {
        base.PerformDie();
    }

    private void AttackPlayer()
    {
        if (!isAttacking)
        {
            StartCoroutine(DashAttackRoutine());
        }
    }

    private IEnumerator DashAttackRoutine()
    {
        isDashing = true;
        isAttacking = true;

        if (player != null)
        {
            Vector3 dashDirection = (player.position - transform.position).normalized;
            float dashForce = 20f;

            rb.velocity = Vector3.zero;
            rb.AddForce(dashDirection * dashForce, ForceMode.Impulse);

            yield return new WaitForSeconds(attackDuration);
            rb.velocity = Vector3.zero;
        }

        isDashing = false;
        yield return new WaitForSeconds(attackDelay);
        isAttacking = false;
    }

    private void MoveToPlayer()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0;

        if (direction.magnitude > 0.1f)
        {
            transform.rotation = Quaternion.LookRotation(direction.normalized);
            Vector3 desired = direction.normalized * data.moveSpeed;
            rb.MovePosition(rb.position + desired * Time.fixedDeltaTime);
        }
    }
}
