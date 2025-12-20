using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public Enemy_FSM<EnemyController> controller;

    [Header("References")]
    [SerializeField] public Animator animator;
    [SerializeField] public Transform player;
    [SerializeField] public Transform sightRoot;
    [SerializeField] public NavMeshAgent agent;
    [SerializeField] public EnemyFSM_Data data;
    
    public float SqrDistanceToPlayer { get; private set; }
    public float SqrMinDistanceToPlayer { get; private set; }
    public float SqrMaxDistanceToPlayer { get; private set; }
    public float SqrWalkToPlayerRange { get; private set; }
    public float SqrRunToPlayerRange { get; private set; }
    public float SqrMinShootingDistance { get; private set;}
    public float SqrMaxShootingDistance { get; private set; }

    private void Awake()
    {
        SqrMinShootingDistance = data.minShootingDistance * data.minShootingDistance;
        SqrMaxShootingDistance = data.maxShootingDistance * data.maxShootingDistance;

        SqrMinDistanceToPlayer = data.minDistanceToPlayer * data.minDistanceToPlayer;
        SqrMaxDistanceToPlayer = data.maxDistanceToPlayer * data.maxDistanceToPlayer;

        SqrWalkToPlayerRange = data.walkToPlayerRange * data.walkToPlayerRange;
        SqrRunToPlayerRange  = data.runToPlayerRange  * data.runToPlayerRange;
    }

    private void Start()
    {
        SqrDistanceToPlayer = CheckDistanceToPlayer(); // check this at LEAST once before entering any state
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

        return Mathf.Abs(Vector3.SqrMagnitude(player.position - sightRoot.position));
    }
}
