using UnityEngine;

[CreateAssetMenu(fileName = "EnemyFSM_Data", menuName = "Scriptable Objects/EnemyFSM_Data")]
public class EnemyFSM_Data : ScriptableObject
{
    [Header("Shooting Distance")]
    [SerializeField] public float minShootingDistance;
    [SerializeField] public float maxShootingDistance;

    [Header("Chasing Distance")]
    [SerializeField] public float minDistanceToPlayer;
    [SerializeField] public float maxDistanceToPlayer;

    [Header("Timer Values")]
    [SerializeField] public float idleTimer;

    [Header("Enemy Movement Speed Values")]
    [SerializeField] public float enemyMaxSpeedWalking;
    [SerializeField] public float enemyMaxSpeedRunning;

    [Header("Enemy Range Behaviour Values")]
    [SerializeField] public float walkToPlayerRange;
    [SerializeField] public float runToPlayerRange;
    
}
