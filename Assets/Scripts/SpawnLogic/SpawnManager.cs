using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    /// <summary>
    /// Whole class takes care of spawning and respawning enemies at game start and throughout the session
    /// </summary>
    [Header("References")]
    [SerializeField] private CentralizedValues values;
    [SerializeField] private ObjectPoolBehaviour enemyObjPool;
    [SerializeField] private ObjectPoolBehaviour vfxObjPool;
    [SerializeField] private EnemySpawnLogic spawns;
    [SerializeField] private Transform playerTransform;

    [Header("Adjustable Values")]
    [SerializeField][Range(10f, 25f)] private float minSpawnDistanceToPlayer;
    [SerializeField][Range(30f, 50f)] private float maxSpawnDistanceToPlayer;
    [SerializeField] private float enqueueVFXObjTime = 2;
    [SerializeField] private float timeBetweenVFXAndEnemySpawn = 1;
    [SerializeField] private int enemyCountOnGameStart;

    [SerializeField] private float slowMotionBasedDelay; // this value can be used as a multiplier for the time delay on spawn

    [Header("Nice to know values")]
    [SerializeField] private float min;
    [SerializeField] private float max;
    [SerializeField] private Vector3 spawnPointDistanceToPlayer;

    private CancellationTokenSource cts;

    private void Awake()
    {
        cts = new();
    }

    private void Start()
    {
        min = minSpawnDistanceToPlayer * minSpawnDistanceToPlayer;
        max = maxSpawnDistanceToPlayer * maxSpawnDistanceToPlayer;

        values.EnemysAliveCount = 0;

        SpawnEnemysOnGameStart();

        values.EnemyCount_onValueChanged.AddListener(ValidateRespawn);
    }
    private void OnDisable()
    {
        values.EnemyCount_onValueChanged.RemoveListener(ValidateRespawn);
    }

    private void SpawnEnemysOnGameStart()
    {
        for (int i = 0; i < enemyCountOnGameStart; i++)
        {
            GameObject tempSpawn;
            GameObject tempEnemy;

            if (spawns.AllSpawnpoints != null)
                tempSpawn = spawns.AllSpawnpoints.Dequeue();

            else break;

            if (enemyObjPool.objectPool != null)
                tempEnemy = enemyObjPool.DeQueueObject();

            else break;

            tempEnemy.transform.position = tempSpawn.transform.position;
            tempEnemy.SetActive(true);
            spawns.AllSpawnpoints.Enqueue(tempSpawn);

            values.EnemysAliveCount = values.EnemysAliveCount + 1; // Spawn Enemy on value changed subscribes after this method is finished
        }
    }

    private async void ValidateRespawn(int _enemyCount)
    {
        if (_enemyCount >= enemyCountOnGameStart)
            return;

        for (int i = 0; i < spawns.AllSpawnpoints.Count; i++)
        {
            var spawnpoint = spawns.AllSpawnpoints.Dequeue();
            if (CheckSpawnDistanceToPlayer(spawnpoint))
            {
                await OnValidateRespawnEnemy(spawnpoint, cts.Token);
                break;
            }
            else
            {
                spawns.AllSpawnpoints.Enqueue(spawnpoint);
                continue;
            }
        }
    }

    private async UniTask OnValidateRespawnEnemy(GameObject _spawn, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        GameObject _vfx = OnValidateGetVFXPrefab();
        GameObject _enemy = OnValidateGetEnemyPrefab();

        if (_vfx == null || _enemy == null)
        {
            spawns.AllSpawnpoints.Enqueue(_spawn);
            cts.Cancel();
        }

        AudioHandler.Instance.PlayActionAmbience_2_Sounds
            (AudioHandler.Instance.enemySpawnSounds[Random.Range(0, AudioHandler.Instance.enemySpawnSounds.Length)]);

        _vfx.TryGetComponent(out ParticleSystem vfxEffect);
        _vfx.transform.position = _spawn.transform.position;
        _vfx.SetActive(true);

        vfxEffect.Play();

        await UniTask.Delay((int)(timeBetweenVFXAndEnemySpawn * 1000));

        _enemy.transform.position = _spawn.transform.position;
        _enemy.SetActive(true);

        spawns.AllSpawnpoints.Enqueue(_spawn);

        await UniTask.Delay((int)(enqueueVFXObjTime * 1000));
        vfxObjPool.EnqueueObject(_vfx);

        values.EnemysAliveCount = values.EnemysAliveCount + 1;
        await UniTask.WaitForEndOfFrame();
    }

    private GameObject OnValidateGetVFXPrefab()
    {
        if (vfxObjPool.objectPool.Count > 0)
            return vfxObjPool.DeQueueObject();

        Debug.Log("VFX OBJ Pool is empty");
        return null;
    }

    private GameObject OnValidateGetEnemyPrefab()
    {
        if (enemyObjPool.objectPool.Count > 0)
            return enemyObjPool.DeQueueObject();

        Debug.Log("Enemy OBJ Pool is empty");
        return null;
    }

    private bool CheckSpawnDistanceToPlayer(GameObject _spawnPoint) // change this to near player
    {
        spawnPointDistanceToPlayer = _spawnPoint.transform.position - playerTransform.transform.position;

        if (Vector3.Dot(spawnPointDistanceToPlayer.normalized, playerTransform.forward) < 0)
            return false;

        if (spawnPointDistanceToPlayer.sqrMagnitude > min && spawnPointDistanceToPlayer.sqrMagnitude < max)
            return true;    

        return false;
    }
}
