using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float acceleration = 20f;

    [Header("Dash")]
    public float dashSpeed = 15f;
    public float dashDuration = 0.12f;
    public float dashCooldown = 1.0f;
    public bool canDoubleDash = false;
    private bool isDashing = false;
    private float dashCooldownTimer = 0f;
    private int dashesLeft = 1;

    [Header("References")]
    private Rigidbody rb;
    private PlayerHealth health;
    public PlayerPowerUp powerUp;

    [Header("Input")]
    private Vector3 inputDir;
    private PlayerInputActions inputActions;

    [Header("Time Scale Effect")]
    float originalScale = 1;
    float originalFixedDeltaTime = 0.02f;
    float timeScaleEffectDuration = 0.3f;
    float timeScaleEffectSlowScale = 0.2f;
    float timeScaleEffectSmoothTime = 0.1f;

    [Header("Movement Limits")]
    [SerializeField] float zMoveMax = 10f;
    [SerializeField] float zMoveMin = -10f;
    [SerializeField] float xMoveMin = 10f;
    [SerializeField] float xMoveMax = 10f;

    [Header("Debug")]
    [SerializeField] float timeScale;
    [SerializeField] float fixedDeltaTime;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        health = GetComponent<PlayerHealth>();
        powerUp = GetComponent<PlayerPowerUp>();
    }

    private void Start()
    {
        inputActions = new PlayerInputActions();
        inputActions.Player.Enable();
    }

    private void Update()
    {
        float h = 0f;
        float v = 0f;

        Vector2 move = inputActions.Player.Move.ReadValue<Vector2>();
        h = move.x;
        v = move.y;

        inputDir = new Vector3(h, 0f, v).normalized;

        if (inputActions.Player.Dash.triggered && dashesLeft > 0)
        {
            StartCoroutine(DoDash());
        }

        if (dashCooldownTimer > 0f)
        {
            dashCooldownTimer -= Time.deltaTime;
            GameEvents.OnDashUsed?.Invoke(Mathf.Max(0, dashCooldownTimer));
        }
        else
        {
            dashesLeft = canDoubleDash ? 2 : 1;
        }

        timeScale = originalScale;
        fixedDeltaTime = originalFixedDeltaTime;
    }

    private void FixedUpdate()
    {
        Vector3 pos = transform.position;

        pos.x = Mathf.Clamp(pos.x, xMoveMin, xMoveMax);
        pos.z = Mathf.Clamp(pos.z, zMoveMin, zMoveMax);

        transform.position = pos;

        if (isDashing) return;

        Vector3 target = inputDir * moveSpeed;
        rb.velocity = Vector3.Lerp(rb.velocity, target, acceleration * Time.fixedDeltaTime);
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
    }

    private IEnumerator DoDash()
    {
        isDashing = true;
        dashesLeft--;
        dashCooldownTimer = dashCooldown;

        GameEvents.OnDashUsed?.Invoke(dashCooldownTimer);

        Vector3 dashDir = inputDir;
        if (dashDir.sqrMagnitude < 0.01f)
        {
            dashDir = transform.forward;
        }
        float timer = 0f;

        AudioManager.Instance.Play("Dash");

        while (timer < dashDuration)
        {
            rb.velocity = dashDir * dashSpeed;
            timer += Time.deltaTime;
            yield return null;
        }

        isDashing = false;
    }

    void OnCollisionStay(Collision collision)
    {
        if (isDashing && collision.gameObject.CompareTag("Enemy"))
        {
            var enemy = collision.gameObject.GetComponent<EnemyBase>();
            Vector3 hitDirection = (transform.position - collision.transform.position).normalized;

            if (enemy != null)
            {
                enemy.TakeDamage(999, hitDirection);
                GameEvents.OnScoreChanged?.Invoke(1);
                AudioManager.Instance.Play("Hit");
                StartCoroutine(HitStop(timeScaleEffectDuration, timeScaleEffectSlowScale, timeScaleEffectSmoothTime));
            }
        }
    }

    private IEnumerator HitStop(float duration, float slowScale, float smoothTime)
    {
        float t = 0f;
        while (t < smoothTime)
        {
            t += Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Lerp(originalScale, slowScale, t / smoothTime);
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            yield return null;
        }

        Time.timeScale = slowScale;
        yield return new WaitForSecondsRealtime(duration);

        t = 0f;
        while (t < smoothTime)
        {
            t += Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Lerp(slowScale, originalScale, t / smoothTime);
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            yield return null;
        }

        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02f;
    }

    public void TakeDamage(int damage)
    {
        if (isDashing) return;
        health.TakeDamage(damage);
    }

    public void TakeProjectileHit(int damage)
    {
        health.TakeDamage(damage);
    }
}
