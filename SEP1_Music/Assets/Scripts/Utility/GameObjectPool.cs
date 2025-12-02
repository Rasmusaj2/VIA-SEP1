using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool : MonoBehaviour
{
    public GameObject templateObject;
    public int poolSize;

    private List<GameObject> pooledObjects;

    public GameObject Acquire(List<GameObject> pool)
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                pool[i].SetActive(true);
                return pool[i];
            }
        }

        return null;
    }

    public void Release(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }

    void Awake()
    {
        pooledObjects = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject clone = Instantiate(templateObject, transform);
            clone.SetActive(false);
            pooledObjects.Add(clone);
        }
    }
}