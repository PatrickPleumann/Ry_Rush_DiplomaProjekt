using UnityEngine;

[CreateAssetMenu(fileName = "EnemyFSM_Data", menuName = "Scriptable Objects/EnemyFSM_Data")]
public class EnemyFSM_Data : ScriptableObject
{
    [Header("Enemy Health")]
    [SerializeField] public float enemyHealth;

    [Header("Enemy Shooting Values")]
    [SerializeField] public float minShootingDistance;
    [SerializeField] public float maxShootingDistance;
    [SerializeField] public float betweenShotsTimer;

    [Header("Enemy Chasing Distance")]
    [SerializeField] public float minDistanceToPlayer;
    [SerializeField] public float maxDistanceToPlayer;

    [Header("Enemy Movement Speed Values")]
    [SerializeField] public float enemyMaxSpeedWalking;
    [SerializeField] public float enemyMaxSpeedRunning;

    [Header("Enemy Range Behaviour Values")]
    [SerializeField] public float walkToPlayerInRange;
    [SerializeField] public float runToPlayerInRange;
    [SerializeField] public float playerFledRange;
    [SerializeField] public float desiredShootingDistance;

    [Header("Enemy Timer Values")]
    [SerializeField] public float idleAnimTimer;
    [SerializeField] public float idleTimer;
    [SerializeField] public float swapStateTimer;

    [Header("Enemy Player in Sight Values")]
    [SerializeField] public float playerInSightDistance;
    [SerializeField] public bool canSeePlayer;
}
