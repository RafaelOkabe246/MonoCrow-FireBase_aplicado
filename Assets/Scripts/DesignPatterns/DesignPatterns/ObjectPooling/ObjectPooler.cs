using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectPooler : Singleton<ObjectPooler>
{
    [System.Serializable]
    public struct Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
        public int count;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;
    public Dictionary<string, int> poolObjectCount;
    private UnityAction<string> action;

    private void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        poolObjectCount = new Dictionary<string, int>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            poolDictionary.Add(pool.tag, objectPool);
            poolObjectCount.Add(pool.tag, 0);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist!");
            return null;
        }

        GameObject objectToSpawn = DequeueFromPool(tag);

        if (objectToSpawn == null)
            return null;

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        IPooledObject pooledObj = objectToSpawn.GetComponent<IPooledObject>();
        ObjectFromPool objFromPool = objectToSpawn.GetComponent<ObjectFromPool>();

        objFromPool.OnDisablePooledObject.RemoveAllListeners();
        objFromPool.OnDisablePooledObject.AddListener(delegate { ReEnqueueObject(tag, objectToSpawn); });

        if (pooledObj != null)
        {
            pooledObj.OnObjectSpawn();
        }

        return objectToSpawn;
    }

    public GameObject DequeueFromPool(string tag)
    {
        foreach (Pool pool in pools)
        {
            if (pool.tag != tag)
                continue;

            if (poolDictionary[tag].Count == 0)
            {
                if (poolObjectCount[tag] < pool.size)
                {
                    GameObject newObj = Instantiate(pool.prefab);
                    newObj.transform.SetParent(gameObject.transform);
                    newObj.SetActive(false);

                    poolObjectCount[tag]++;
                    poolDictionary[tag].Enqueue(newObj);
                }
                else
                    return null;
            }

            return poolDictionary[tag].Dequeue();
        }

        return null;
    }

    public void ReEnqueueObject(string tag, GameObject objectToEnqueue) 
    {
        poolDictionary[tag].Enqueue(objectToEnqueue);
    }

    public GameObject SpawnFromPoolAndAddParent(string tag, Vector3 position, Quaternion rotation, Transform parent)
    {
        GameObject objectToSpawn = SpawnFromPool(tag, position, rotation);
        if(objectToSpawn != null)
            objectToSpawn.transform.SetParent(parent);

        objectToSpawn.transform.localPosition = Vector3.zero;

        return objectToSpawn;
    }
}
