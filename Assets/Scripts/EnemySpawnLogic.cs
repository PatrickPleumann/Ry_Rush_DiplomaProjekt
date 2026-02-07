using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawnLogic : MonoBehaviour
{
    [SerializeField] private ObjectPoolBehaviour enemyObjectPool;
    [SerializeField] private Queue<GameObject> allSpawnpoints;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private int EnemiesCountOnStart;

    public static UnityEvent enemyCountReduced;
    private int currentEnemyCount;
     
    private void OnEnable()
    {
        currentEnemyCount = 0;
        allSpawnpoints = new();
        InitSpawnpointPool();
        //StartCoroutine(SpawnEnemys(EnemiesCountOnStart));
        if (currentEnemyCount < EnemiesCountOnStart)
        {
            SpawnEnemy();
        }
    }

    private void Update()
    {
        if (currentEnemyCount < EnemiesCountOnStart)
        {
            SpawnEnemy();
        }
    }

    private void InitSpawnpointPool()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            allSpawnpoints.Enqueue(transform.GetChild(i).gameObject);
        }
    }
    public void SpawnEnemy()
    {
        if (enemyObjectPool.objectPool.Count > 0)
        {

            var spawnPoint = allSpawnpoints.Dequeue();
            if (CheckIfSpawnPointIsBehindPlayer(spawnPoint) == true)
            {
                var temp = enemyObjectPool.DeQueueObject();
                temp.transform.position = spawnPoint.transform.position;
                temp.SetActive(true);
                currentEnemyCount++;
            }
            allSpawnpoints.Enqueue(spawnPoint);
        }
    }

    private bool CheckIfSpawnPointIsBehindPlayer(GameObject _spawnPoint)
    {
        if (Vector3.Dot((_spawnPoint.transform.position - playerTransform.position).normalized, playerTransform.forward) < 0)
        {
            return true;
        }
        return false;
    }
}
