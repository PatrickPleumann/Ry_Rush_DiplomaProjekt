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

    [Header("Enemy Movement Speed Values")]
    [SerializeField] public float enemyMaxSpeedWalking;
    [SerializeField] public float enemyMaxSpeedRunning;

    [Header("Enemy Range Behaviour Values")]
    [SerializeField] public float walkToPlayerRange;
    [SerializeField] public float runToPlayerRange;
    [SerializeField] public float playerFledRange;

    [Header("Timer Values")]
    [SerializeField] public float idleAnimTimer;
    [SerializeField] public float idleTimer;
    [SerializeField] public float swapStateTimer;

    [Header("Player in Sight Values")]
    [SerializeField] public bool canSeePlayer;
}
