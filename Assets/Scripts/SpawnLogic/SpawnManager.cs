using Cysharp.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    /// <summary>
    /// Whole class takes care of spawning and respawning enemies at game start and throughout the session
    /// </summary>
    [Header("References")]
    [SerializeField] private CentralizedValues values;
    [SerializeField] private SongData data;
    [SerializeField] private ObjectPoolBehaviour enemyObjPool;
    [SerializeField] private ObjectPoolBehaviour vfxObjPool;
    [SerializeField] private EnemySpawnLogic spawns;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameObject allEnemies;


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


    private float timeInSecondsPerSample;
    private float timePerBeat;
    private int currentSamplesInBeat;

    private CancellationTokenSource cts;

    private void Awake()
    {
        cts = new();
    }

    private void Start()
    {
        timePerBeat = (60f / data.BPM);
        timeInSecondsPerSample = (1f / data.SamplesPerBeat);

        min = minSpawnDistanceToPlayer * minSpawnDistanceToPlayer;
        max = maxSpawnDistanceToPlayer * maxSpawnDistanceToPlayer;

        values.EnemysAliveCount = 0;

        SpawnEnemysOnGameStart();

        values.EnemyCount_OnValueChanged.AddListener(ValidateRespawn);
        values.OnSessionEnds.AddListener(DespawnAllEnemies);
    }

    private void DespawnAllEnemies()
    {
        allEnemies.SetActive(false);
    }

    private void OnDisable()
    {
        values.EnemyCount_OnValueChanged.RemoveListener(ValidateRespawn);
        values.OnSessionEnds.RemoveListener(DespawnAllEnemies);
    }


    /// <summary>
    /// Gets a very precise time in seconds till the next beat appears. It is used to synchronize on beat behaviour to enemies spawning for example.
    /// Value is also floored to two digits after zero.
    /// </summary>
    /// <returns></returns>

    private float GetTimeTillNextBeatInSecondsFloored()
    {
        var temp = (values.CurrentSamples * timeInSecondsPerSample);
        return Utility.FloorFloatToTwoDigits(temp);
    }

    private float GetTimeTillNextBeatCanSpawnEnemy(float _currentTimeTillNextBeat)
    {
        if (_currentTimeTillNextBeat > timeBetweenVFXAndEnemySpawn)
            return _currentTimeTillNextBeat;

        else
        {
            var temp = _currentTimeTillNextBeat;

            while (temp < timeBetweenVFXAndEnemySpawn)
                temp += timePerBeat;

            return (temp - timeBetweenVFXAndEnemySpawn);
        }
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

        var time = GetTimeTillNextBeatCanSpawnEnemy(GetTimeTillNextBeatInSecondsFloored());
        await UniTask.Delay((int)(time * 1000));

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
