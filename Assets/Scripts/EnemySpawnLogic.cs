using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnLogic : MonoBehaviour
{
    [SerializeField] private ObjectPoolBehaviour enemyObjectPool;
    [SerializeField] private Queue<GameObject> allSpawnpoints;
    [SerializeField] private Transform playerTransform;
    private void Awake()
    {
        allSpawnpoints = new();
        InitSpawnpointPool();
        SpawnEnemys();
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

            var temp = enemyObjectPool.DeQueueObject();
            var spawnPoint = allSpawnpoints.Dequeue();
            if (CheckIfSpawnPointIsBehindPlayer(spawnPoint) == true)
            {
                temp.transform.position = spawnPoint.transform.position;
                temp.SetActive(true);
            }
            allSpawnpoints.Enqueue(spawnPoint);
        }
    }

    private void SpawnEnemys()
    {
        SpawnEnemy();
        SpawnEnemy();
        SpawnEnemy();
        SpawnEnemy();
    }

    private bool CheckIfSpawnPointIsBehindPlayer(GameObject _spawnPoint)
    {
        Debug.Log("Vec:" + (_spawnPoint.transform.position - playerTransform.position).normalized);
        Debug.Log("Dot:" + Vector3.Dot((_spawnPoint.transform.position - playerTransform.position).normalized, playerTransform.forward));
        if (Vector3.Dot((_spawnPoint.transform.position - playerTransform.position).normalized, playerTransform.forward) < 0)
        {
            return true;
        }
        return false;
    }
}
