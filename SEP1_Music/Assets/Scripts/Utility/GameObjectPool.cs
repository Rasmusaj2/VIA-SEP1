using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool : MonoBehaviour
{
    public GameObject templateObject;

    // Only used by the inspector to set an initial size or view the current size.
    // Otherwise, pooledObjects.Count is used to keep track of the actual size
    [SerializeField]
    private int poolSize = 0;

    private List<GameObject> pooledObjects;

    public void Allocate(int newPoolSize)
    {
        if (newPoolSize == pooledObjects.Count)
        {
            // No action needed, the current size of the pool is already the requested size
            return;
        }
        else if (newPoolSize > pooledObjects.Count)
        {
            // Instantiate objects to fit new size
            for (int i = pooledObjects.Count; i < newPoolSize; i++)
            {
                GameObject clone = Instantiate(templateObject, transform);
                clone.SetActive(false); // New objects always start inactive. Must be acquired from the pool first before use
                pooledObjects.Add(clone);
            }
        }
        else
        {
            // Destroy excess objects
            for (int i = pooledObjects.Count - 1; i >= newPoolSize; i--)
            {
                Destroy(pooledObjects[i]);
            }
            pooledObjects.RemoveRange(newPoolSize, pooledObjects.Count - newPoolSize);
        }

        poolSize = newPoolSize;
    }

    public int GetSize()
    {
        return poolSize;
    }

    // Activates an object from the pool, and returns it. Returns null if the pool is empty (i.e. zero inactive objects)
    public GameObject Acquire()
    {
        // Find the first inactive object in the pool
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy) // Inactive?
            {
                pooledObjects[i].SetActive(true);
                return pooledObjects[i];
            }
        }

        // All objects are already in use
        return null;
    }

    // Deactivates the object and releases it back into the pool
    public void Release(GameObject gameObject)
    {
        // Mark an object as available by simply deactivating it
        gameObject.SetActive(false);
    }

    void Awake()
    {
        pooledObjects = new List<GameObject>();
        Allocate(poolSize);
    }
}