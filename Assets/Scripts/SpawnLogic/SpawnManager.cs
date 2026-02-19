using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Drawing.Design;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SpawnManager : MonoBehaviour
{
    /// <summary>
    /// Whole class takes care of spawning and respawning enemies at the begin and throughout the session
    /// </summary>
    [Header("References")]
    [SerializeField] private CentralizedValues values;
    [SerializeField] private ObjectPoolBehaviour enemyObjPool;
    [SerializeField] private ObjectPoolBehaviour vfxObjPool;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private EnemySpawnLogic spawns;

    [Header("Adjustable Values")]
    [SerializeField][Range(3f, 5f)] private float minSpawnDistanceToPlayer;
    [SerializeField][Range(10f, 20f)] private float maxSpawnDistanceToPlayer;
    [SerializeField] private float timeBetweenVFXAndEnemySpawn = 1;
    [SerializeField] private int EnemyCountOnGameStart;

    [SerializeField] private float slowMotionBasedDelay; // this value can be used as a multiplier for the time delay on spawn

    [Header("Nice to know values")]
    [SerializeField] private float min;
    [SerializeField] private float max;
    [SerializeField] private Vector3 sqrMagnitude;

    private void OnDisable()
    {
        values.EnemyCount_onValueChanged.RemoveListener(ValidateRespawnAction);
    }

    private void Start()
    {
        min = minSpawnDistanceToPlayer * minSpawnDistanceToPlayer;
        max = maxSpawnDistanceToPlayer * maxSpawnDistanceToPlayer;

        values.EnemysAliveCount = 0;
        SpawnEnemysOnGameStart();

        values.EnemyCount_onValueChanged.AddListener(ValidateRespawnAction);
    }

    private void SpawnEnemysOnGameStart()
    {
        for (int i = 0; i < EnemyCountOnGameStart; i++)
        {
            GameObject tempSpawn;
            GameObject tempEnemy;
            if (spawns.allSpawnpoints != null)
                tempSpawn = spawns.allSpawnpoints.Dequeue();

            else break;

            if (enemyObjPool.objectPool != null)
                tempEnemy = enemyObjPool.DeQueueObject();

            else break;

            tempEnemy.transform.position = tempSpawn.transform.position;
            tempEnemy.SetActive(true);
            spawns.allSpawnpoints.Enqueue(tempSpawn);

            values.EnemysAliveCount = values.EnemysAliveCount + 1; // Spawn Enemy on value changed subscribes after this method is finished
        }
    }

    private void ValidateRespawnAction(int _enemyCount)
    {
        if (_enemyCount >= EnemyCountOnGameStart)
            return;

        for (int i = 0; i < spawns.allSpawnpoints.Count; i++)
        {
            var spawnpoint = spawns.allSpawnpoints.Dequeue();
            if (CheckSpawnDistanceToPlayer(spawnpoint))
            {
                StartCoroutine(OnValidate_RespawnEnemy(spawnpoint));
                Debug.Log("Respawn in progress");
                break;
            }
            else
            {
                spawns.allSpawnpoints.Enqueue(spawnpoint);
                continue;
            }
        }
    }

    private IEnumerator OnValidate_RespawnEnemy(GameObject _spawn)
    {
        GameObject _vfx = OnValidate_GetVFXPrefab();
        GameObject _enemy = OnValidate_GetEnemyPrefab();

        if (_vfx == null || _enemy == null)
        {
            spawns.allSpawnpoints.Enqueue(_spawn);
            yield break;
        }

        AudioHandler.Instance.PlayActionAmbience_2_Sounds
            (AudioHandler.Instance.enemySpawnSounds[Random.Range(0, AudioHandler.Instance.enemySpawnSounds.Length)]);

        _vfx.TryGetComponent(out ParticleSystem vfx_Effect);
        _vfx.transform.position = _spawn.transform.position;
        _vfx.SetActive(true);
        vfx_Effect.Play();

        yield return new WaitForSeconds(timeBetweenVFXAndEnemySpawn);

        _enemy.transform.position = _spawn.transform.position;
        _enemy.SetActive(true);

        spawns.allSpawnpoints.Enqueue(_spawn);

        yield return new WaitForSeconds(1);
        vfxObjPool.EnqueueObject(_vfx);

        values.EnemysAliveCount = values.EnemysAliveCount + 1; //TODO: dangerous
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
        sqrMagnitude = _spawnPoint.transform.position - playerTransform.transform.position;

        if (sqrMagnitude.sqrMagnitude > min && sqrMagnitude.sqrMagnitude < max)
        {
            return true;    
        }
        return false;
    }
}
