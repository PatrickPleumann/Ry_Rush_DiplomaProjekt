using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EnemyController : MonoBehaviour
{
    public Enemy_FSM<EnemyController> controller;

    [Header("References")]
    [SerializeField] public EnemyObjectPool Pool; // no in use currently
    [SerializeField] public Animator Animator;
    [SerializeField] public Transform Player;
    [SerializeField] public Transform SightRoot;
    [SerializeField] public NavMeshAgent Agent;
    [SerializeField] public EnemyFSM_Data Data;
    [SerializeField] public Transform ThisEnemy;

    public float EnemyDirSmoothSpeed;
    public float EnemyHealth;    
    
    private Vector3 targetDirection;
    private Vector3 newDirection;
    private Vector3 newDirectionSmoothed;

    #region Sqare Distances for better Performace
    public float SqrDistanceToPlayer { get; private set; }
    public float SqrMinDistanceToPlayer { get; private set; }
    public float SqrMaxDistanceToPlayer { get; private set; }
    public float SqrWalkToPlayerInRange { get; private set; }
    public float SqrRunToPlayerInRange { get; private set; }
    public float SqrMinShootingDistance { get; private set;}
    public float SqrMaxShootingDistance { get; private set; }

    public float SqrDistancePlayerInSight { get; private set; }

    public float SqrDesiredShootingRange { get; private set; }
    #endregion
    private void Awake()
    {
        EnemyDirSmoothSpeed = Data.enemyDirSmoothSpeed;
        EnemyHealth = Data.enemyHealth;
        SqrMinShootingDistance = Data.minShootingDistance * Data.minShootingDistance;
        SqrMaxShootingDistance = Data.maxShootingDistance * Data.maxShootingDistance;

        SqrMinDistanceToPlayer = Data.minDistanceToPlayer * Data.minDistanceToPlayer;
        SqrMaxDistanceToPlayer = Data.maxDistanceToPlayer * Data.maxDistanceToPlayer;

        SqrWalkToPlayerInRange = Data.walkToPlayerInRange * Data.walkToPlayerInRange;
        SqrRunToPlayerInRange  = Data.runToPlayerInRange  * Data.runToPlayerInRange;

        SqrDesiredShootingRange = Data.desiredShootingDistance * Data.desiredShootingDistance;
        SqrDistancePlayerInSight = Data.playerInSightDistance * Data.playerInSightDistance;
    }

    private void Start()
    {
        SqrDistanceToPlayer = CheckDistanceToPlayer(); // check this once before entering any state, all following stateSwitchBehaviours depend on that value
        controller = new Enemy_FSM<EnemyController>(this);
        controller.currentState.EnterState(); //point of entry
    }

    private void Update()
    {
        controller.Update();
        SqrDistanceToPlayer = CheckDistanceToPlayer();
    }

    private float CheckDistanceToPlayer()
    {
        return Mathf.Abs(Vector3.SqrMagnitude(Player.position - transform.position));
    }

    public void UpdateEnemyRotation()
    {
        targetDirection = Player.position - transform.position;
        newDirection = Vector3.RotateTowards(transform.forward, targetDirection, 100, 0);

        newDirectionSmoothed = Vector3.Slerp(transform.forward, newDirection, EnemyDirSmoothSpeed * Time.deltaTime);

        transform.rotation = Quaternion.LookRotation(newDirectionSmoothed);
    }
}
