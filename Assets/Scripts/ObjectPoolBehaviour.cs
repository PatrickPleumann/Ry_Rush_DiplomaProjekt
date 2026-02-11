using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ObjectPoolBehaviour : MonoBehaviour
{
    public Queue<GameObject> objectPool = new Queue<GameObject>();
    [SerializeField] private GameObject objectToPool;   // every object to pool has to have ISetDefaultSettings, even if it does nothing!
    [SerializeField] private int amountOfObjects;
    private void Awake()
    {
        if (objectToPool != null)
            InitObjectPool();
    }

    public void EnqueueObject(GameObject _objectToPool)
    {
        var isValid = _objectToPool.TryGetComponent(out ISetDefaultValues temp);
        if (isValid)
        {
            _objectToPool.SetActive(false); 
            temp.SetDefaultValues();
            objectPool.Enqueue(_objectToPool);
        }
    }

    public GameObject DeQueueObject()
    {
        return objectPool.Dequeue();
    }

    private void InitObjectPool()
    {
        for (int i = 0; i < amountOfObjects; i++)
        {
            var temp = Instantiate(objectToPool);
            temp.SetActive(false);
            temp.transform.SetParent(gameObject.transform);
            temp.transform.position = gameObject.transform.position;
            objectPool.Enqueue(temp);
        }
    }
}
