using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// A component added to a pooled object that allows it to return itself to its pool.
/// </summary>
public class ReturnToPool : MonoBehaviour
{
    public IObjectPool<GameObject> Pool { get; set; }

    // Call this method to return the object to the pool
    public void Release()
    {
        Pool.Release(gameObject);
    }
}