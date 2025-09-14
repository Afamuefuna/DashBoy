using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// A generic object pool using Unity's built-in system.
/// Call Get() to fetch an object and Release() on the object's pool to return it.
/// </summary>
public class ObjectPoolHandler : MonoBehaviour
{
    [Tooltip("The prefab to be pooled.")]
    [SerializeField] private GameObject prefab;

    [Tooltip("The default number of items to create when the pool is initialized.")]
    [SerializeField] private int initialSize = 10;

    [Tooltip("The maximum number of items that can be stored in the pool.")]
    [SerializeField] private int maxSize = 20;

    private IObjectPool<GameObject> pool;

    private void Awake()
    {
        pool = new ObjectPool<GameObject>(
            createFunc: CreatePooledItem,
            actionOnGet: OnTakeFromPool,
            actionOnRelease: OnReturnToPool,
            actionOnDestroy: OnDestroyObject,
            collectionCheck: true,
            defaultCapacity: initialSize,
            maxSize: maxSize);
    }

    // Method called to create a new object when the pool is empty
    private GameObject CreatePooledItem()
    {
        var go = Instantiate(prefab, transform);

        var returnToPool = go.GetComponent<ReturnToPool>();
        returnToPool.Pool = pool;

        return go;
    }

    // Method called when an object is taken from the pool
    private void OnTakeFromPool(GameObject go)
    {
        go.SetActive(true);
    }

    // Method called when an object is returned to the pool
    private void OnReturnToPool(GameObject go)
    {
        go.SetActive(false);
    }

    // Method called if the pool is full and a returned object needs to be destroyed
    private void OnDestroyObject(GameObject go)
    {
        Destroy(go);
    }

    // Public method to get an object from the pool
    public GameObject Get(Vector3 position, Quaternion rotation)
    {
        var go = pool.Get();
        go.transform.SetPositionAndRotation(position, rotation);
        return go;
    }
}