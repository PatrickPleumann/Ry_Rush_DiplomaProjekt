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
        //Maybe more spawnpoints all around the map, because it can happen, that no enemies respawn if the player
        //is to far away from any spawnpoint, so the "search spawnpoint" method would iterate for the length of all spawnpoints
        // and won´t repeat after this. This action gets called again after another enemy dies, but the issue stays the same
        // a quick fix could be to increase the spawn distance to the player and place some spawnpoints on a "valid distance to player grid"
        for (int i = 0; i < transform.childCount; i++)
        {
            allSpawnpoints.Enqueue(transform.GetChild(i).gameObject);
        }
    }
}