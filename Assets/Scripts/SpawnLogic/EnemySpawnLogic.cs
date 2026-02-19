using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawnLogic : MonoBehaviour
{
    public Queue<GameObject> allSpawnpoints = new();
    private void Start()
    {
        InitSpawnpointPool();
    }
    private void InitSpawnpointPool()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            allSpawnpoints.Enqueue(transform.GetChild(i).gameObject);
        }
    }
}