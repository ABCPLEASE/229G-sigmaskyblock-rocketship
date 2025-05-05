using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public static ObjectPooler Instance;
    private void Awake() => Instance = this;

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
            return null;
        }

        Queue<GameObject> objectPool = poolDictionary[tag];

        foreach (GameObject pooledObj in objectPool)
        {
            if (!pooledObj.activeInHierarchy)
            {
                pooledObj.SetActive(true);
                pooledObj.transform.position = position;
                pooledObj.transform.rotation = rotation;

                // If it's a meteorite, deactivate it after 10 seconds instead of destroying it
                if (pooledObj.CompareTag("Meteorite"))
                {
                    StartCoroutine(DeactivateObject(pooledObj, 10f)); // Deactivate after 10 seconds
                }

                return pooledObj;
            }
        }

        Debug.LogWarning("No available objects in pool for tag " + tag);
        return null;
    }

    // Coroutine to deactivate the object after a delay
    private IEnumerator DeactivateObject(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);  // Deactivate object after the specified delay
    }
}


