using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnLogic : MonoBehaviour
{
    public Queue<GameObject> AllSpawnpoints = new();
    private void Start()
    {
        InitSpawnpointPool();
    }
    private void InitSpawnpointPool()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            AllSpawnpoints.Enqueue(transform.GetChild(i).gameObject);
        }
    }
}