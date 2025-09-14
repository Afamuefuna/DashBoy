using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 5;
    private int current;
    [SerializeField] private bool canTakeDamage = true;
    Coroutine recoverFromDamageCoroutine;

    private void Awake()
    {
        current = maxHealth;
    }

    IEnumerator RecoverFromDamageCoroutine()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        TrailRenderer trailRenderer = GetComponentInChildren<TrailRenderer>();

        canTakeDamage = false;

        for (int i = 0; i < 5; i++)
        {
            trailRenderer.enabled = false;
            meshRenderer.enabled = false;
            yield return new WaitForSeconds(0.15f);
            meshRenderer.enabled = true;
            trailRenderer.enabled = true;
            yield return new WaitForSeconds(0.15f);
        }

        canTakeDamage = true;
    }

    public void TakeDamage(int dmg)
    {
        if (!canTakeDamage) return;

        current -= dmg;
        GameEvents.OnPlayerDamaged?.Invoke(current);
        if (current <= 0)
        {
            StopCoroutine(recoverFromDamageCoroutine);
            Die();
        }
        else
        {
            recoverFromDamageCoroutine = StartCoroutine(RecoverFromDamageCoroutine());
        }
    }

    private void Die()
    {
        GameEvents.OnPlayerDied?.Invoke();
        gameObject.SetActive(false);
    }

    public int GetHealth() => current;
}
