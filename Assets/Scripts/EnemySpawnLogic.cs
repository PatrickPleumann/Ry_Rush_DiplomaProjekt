using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawnLogic : MonoBehaviour
{
    [SerializeField] private CentralizedValues values;
    [SerializeField] private ObjectPoolBehaviour enemyObjectPool;
    [SerializeField] public Queue<GameObject> allSpawnpoints;
    [SerializeField] private Transform playerTransform;
    [SerializeField][Range(3f,5f)] private float minSpawnDistanceToPlayer;
    [SerializeField][Range(10f, 20f)] private float maxSpawnDistanceToPlayer;

    [SerializeField] private int EnemiesCountOnStart;

<<<<<<< HEAD
    public UnityEvent enemyCountReduced;
    private int currentEnemyCount;
     
=======
    private void Awake()
    {
        
    }
>>>>>>> 75f40a3405c628bf2e9361363869506e21b8bd01
    private void OnEnable()
    {
        allSpawnpoints = new();
    }

<<<<<<< HEAD
=======
    private void OnDisable()
    {
        //values.EnemyCount_onValueChanged.RemoveListener(SpawnEnemy);
    }

    private void Start()
    {
        InitSpawnpointPool();

        for (int i = 0; i < EnemiesCountOnStart; i++)
        {
            var temp = allSpawnpoints.Dequeue();
            temp.TryGetComponent(out EnemySpawnPoint spawn);
            spawn.SpawnEnemy(enemyObjectPool.DeQueueObject());
            allSpawnpoints.Enqueue(temp);
        }
    }
>>>>>>> 75f40a3405c628bf2e9361363869506e21b8bd01

    private void InitSpawnpointPool()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            allSpawnpoints.Enqueue(transform.GetChild(i).gameObject);
        }
    }
    public void SpawnEnemy(int _currentEnemys)
    {
        Debug.Log("Try to spawn enemy");
        if ((_currentEnemys < EnemiesCountOnStart) && (enemyObjectPool.objectPool.Count > 0))
        {
            var spawnPoint = allSpawnpoints.Dequeue();
            Debug.Log("Try to get valid spawn point");
            if (CheckSpawnDistanceToPlayer(spawnPoint) == true)
            {
                spawnPoint.TryGetComponent(out EnemySpawnPoint spawn);
                var temp = enemyObjectPool.DeQueueObject();
<<<<<<< HEAD
                spawn.OnRespawn(temp);
                currentEnemyCount++;
=======
                Debug.Log("Try to spawn enemy with vfx");
                spawn.OnSpawn(temp);
                values.EnemysAliveCount = values.EnemysAliveCount + 1;
>>>>>>> 75f40a3405c628bf2e9361363869506e21b8bd01
            }
            allSpawnpoints.Enqueue(spawnPoint);
        }
    }

    private bool CheckSpawnDistanceToPlayer(GameObject _spawnPoint) // change this to near player
    {
<<<<<<< HEAD
        if (Vector3.Dot((_spawnPoint.transform.position - playerTransform.position).normalized, playerTransform.forward) > 0)
=======
        var min = minSpawnDistanceToPlayer * minSpawnDistanceToPlayer;
        var max = maxSpawnDistanceToPlayer * maxSpawnDistanceToPlayer;
        var temp = _spawnPoint.transform.position - playerTransform.transform.position;
        var sqrAbs = Mathf.Abs(temp.sqrMagnitude);

        //if (Vector3.Dot((_spawnPoint.transform.position - playerTransform.position).normalized, playerTransform.forward) > 0)
        //{
        //    return true;
        //}
        if (sqrAbs > min && sqrAbs < max)
>>>>>>> 75f40a3405c628bf2e9361363869506e21b8bd01
        {
            return true;
        }
        return false;
    }
}
