using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private CentralizedValues values;
    [SerializeField] private ObjectPoolBehaviour enemyObjPool;
    [SerializeField] private ObjectPoolBehaviour vfxObjPool;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float timeBetweenVFXAndEnemySpawn = 1;
    [SerializeField][Range(3f, 5f)] private float minSpawnDistanceToPlayer;
    [SerializeField][Range(10f, 20f)] private float maxSpawnDistanceToPlayer;

    [SerializeField] private EnemySpawnLogic spawns;
 

    private void OnDisable()
    {
        values.EnemyCount_onValueChanged.RemoveListener(ValidateRespawnAction);
    }

    private void Start()
    {
        //spawn 30
        values.EnemyCount_onValueChanged.AddListener(ValidateRespawnAction);
    }

    private void ValidateRespawnAction(int _enemyCount)
    {
        if (_enemyCount >= 30)
            return;

        for (int i = 0; i < spawns.allSpawnpoints.Count; i++)
        {
            var spawnpoint = spawns.allSpawnpoints.Dequeue();
            if (CheckSpawnDistanceToPlayer(spawnpoint))
            {
                StartCoroutine(OnValidate_RespawnEnemy(spawnpoint));
                Debug.Log("Nach Coroutine");
                break;
            }
            else
                continue;
        }
    }

    private IEnumerator OnValidate_RespawnEnemy(GameObject _spawn)
    {
        GameObject _vfx = OnValidate_GetVFXPrefab();
        GameObject _enemy = OnValidate_GetEnemyPrefab();

        if (_vfx == null || _enemy == null)
            yield break;

        _vfx.transform.position = _spawn.transform.position;
        _vfx.SetActive(true);

        yield return new WaitForSeconds(timeBetweenVFXAndEnemySpawn);

        _enemy.transform.position = _spawn.transform.position;
        _enemy.SetActive(true);

        spawns.allSpawnpoints.Enqueue(_spawn);
        values.EnemysAliveCount = values.EnemysAliveCount + 1; //TODO: dangerous

        Debug.Log("In Coroutine");

        yield return new WaitForEndOfFrame();
    }

    private GameObject OnValidate_GetVFXPrefab()
    {
        if (vfxObjPool.objectPool.Count > 0)
            return vfxObjPool.DeQueueObject();

        Debug.Log("VFX OBJ Pool is empty");
        return null;
    }

    private GameObject OnValidate_GetEnemyPrefab()
    {
        if (enemyObjPool.objectPool.Count > 0)
            return enemyObjPool.DeQueueObject();

        Debug.Log("Enemy OBJ Pool is empty");
        return null;
    }

    private bool CheckSpawnDistanceToPlayer(GameObject _spawnPoint) // change this to near player
    {
        var min = minSpawnDistanceToPlayer * minSpawnDistanceToPlayer;
        var max = maxSpawnDistanceToPlayer * maxSpawnDistanceToPlayer;
        var temp = _spawnPoint.transform.position - playerTransform.transform.position;
        var sqrAbs = Mathf.Abs(temp.sqrMagnitude);

        if (sqrAbs > min && sqrAbs < max)
        {
            return true;
        }
        return false;
    }
}
