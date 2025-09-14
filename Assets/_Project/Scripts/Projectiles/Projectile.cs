using UnityEngine;

[RequireComponent(typeof(ReturnToPool))]
public class Projectile : MonoBehaviour
{
    public ProjectileData data;
    private float lifeTimer;

    private ReturnToPool returnToPool;

    private void Awake()
    {
        returnToPool = GetComponent<ReturnToPool>();
    }

    public void Init(ProjectileData d)
    {
        data = d;
        lifeTimer = d.lifeTime;
    }

    private void Update()
    {
        transform.position += transform.forward * data.speed * Time.deltaTime;

        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f)
        {
            ReturnToPool();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var pc = other.GetComponent<PlayerController>();
            if (pc != null)
            {
                pc.TakeProjectileHit(data.damage);
            }
            ReturnToPool();
        }
        else if (other.CompareTag("Enemy"))
        {
            return;
        }
        else
        {
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
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