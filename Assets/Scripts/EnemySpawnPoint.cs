using System.Collections;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    [SerializeField] private float timeTillSpawn;
    [SerializeField] private ObjectPoolBehaviour vfx_objectPool;

    public void OnSpawn(GameObject _enemyPrefab)
    {
        StartCoroutine(SpawnpointBehaviour(timeTillSpawn, _enemyPrefab));
    }

    private IEnumerator SpawnpointBehaviour(float _timeTillSpawn, GameObject _enemyPrefab)
    {
        Debug.Log("Spawn enemy with vfx effect");
        SpawnVFXEffect();

        yield return new WaitForSeconds(_timeTillSpawn);

        SpawnEnemy(_enemyPrefab);

        yield return null;
    }

    public void SpawnEnemy(GameObject _enemyPrefab)
    {
        _enemyPrefab.transform.position = gameObject.transform.position;
        _enemyPrefab.SetActive(true);
    }
    private void SpawnVFXEffect()
    {
        var temp = vfx_objectPool.DeQueueObject();
        temp.transform.position = gameObject.transform.position;
        temp.SetActive(true);
    }

}
