using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string name;
        public List<GameObject> prefab;
        public int size;
    }

    public static ObjectPooler Instance;

    public List<Pool> pools;

    public Dictionary<string, Queue<GameObject>> poolDictionary;

    public List<GameObject> destroylist = new List<GameObject>();
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                for (int a = 0; a < pool.prefab.Count; a++)
                {
                    GameObject obj = Instantiate(pool.prefab[a],gameObject.transform.parent.transform);
                    obj.SetActive(false);
                    objectPool.Enqueue(obj);
                    destroylist.Add(obj);

                }
            }
            poolDictionary.Add(pool.name, objectPool);
        }

    }

    public GameObject SpawnForGameObject(string name, Vector3 position, Quaternion rotation, Transform parent)
    {
        GameObject objectToSpawn = poolDictionary[name].Dequeue();
        destroylist.Remove(objectToSpawn);
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.parent = parent;
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        //objectToSpawn.GetComponent<Rigidbody>().AddForce(transform.forward * 5000);
        poolDictionary[name].Enqueue(objectToSpawn);
        return objectToSpawn;
    }


}