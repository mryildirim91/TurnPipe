using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool : MonoBehaviour
{
    public static GameObjectPool Instance { get; private set; }

    [System.Serializable]
    private class Pool
    {
        public string name;
        public GameObject prefab;
        public int amount;
    }

    [SerializeField]
    private List<Pool> _pools;

    private Dictionary<string, Queue<GameObject>> _poolDict;

    private void Awake()
    {
        Instance = this;
        _poolDict = new Dictionary<string, Queue<GameObject>>();

        FillThePool();
    }

    private void FillThePool()
    {
        foreach (Pool pool in _pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.amount; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            _poolDict.Add(pool.name, objectPool);
        }
    }

    private void RefillThePool(string name)
    {
        foreach (Pool pool in _pools)
        {
            if (pool.name == name) 
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                _poolDict[name].Enqueue(obj);
            }
        }
    }

    public GameObject SpawnFromPool(string name)
    {
        if (!_poolDict.ContainsKey(name))
        {
            Debug.LogError("Key does not exist");
            return null;
        }

        if(_poolDict[name].Count < 1)
        {
            RefillThePool(name);
        }    

        if (_poolDict[name].Count > 0)
        {
            GameObject obj = _poolDict[name].Dequeue();
            obj.SetActive(true);

            StartCoroutine(ReturnToPool(obj, name));

            return obj;
        }

        return null;
    }

    private IEnumerator ReturnToPool(GameObject obj, string name)
    {
        yield return BetterWaitForSeconds.Wait(1.7f);
        obj.SetActive(false);
        _poolDict[name].Enqueue(obj);
    }
}
