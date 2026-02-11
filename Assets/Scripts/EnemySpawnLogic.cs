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

    public UnityEvent enemyCountReduced;
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
                spawnPoint.TryGetComponent(out EnemySpawnPoint spawn);
                var temp = enemyObjectPool.DeQueueObject();
                spawn.OnRespawn(temp);
                currentEnemyCount++;
            }
            allSpawnpoints.Enqueue(spawnPoint);
        }
    }

    private bool CheckIfSpawnPointIsBehindPlayer(GameObject _spawnPoint) // change this to near player
    {
        if (Vector3.Dot((_spawnPoint.transform.position - playerTransform.position).normalized, playerTransform.forward) > 0)
        {
            return true;
        }
        return false;
    }
}
