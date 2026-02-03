using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EnemyController : MonoBehaviour, ISetDefaultValues
{
    public Enemy_FSM<EnemyController> controller;

    [Header("References")]
    [SerializeField] private ObjectPoolBehaviour Pool; // no in use currently
    [SerializeField] public Animator Animator;
    [SerializeField] public Transform Player;
    [SerializeField] public NavMeshAgent Agent;
    [SerializeField] public EnemyFSM_Data Data;
    [SerializeField] public Transform ThisEnemy;
    [SerializeField] public PlayerInfo playerInfo;

    [SerializeField] private CharacterJoint[] ragdollJoints;
    //[SerializeField] public EnemyMaxChasingCounter maxEnemiesChasing;  // need to rethink whole logic behind this behaviour

    public float EnemyDirSmoothSpeed;
    [SerializeField] private float despawnTimer = 1;
    public bool canChase = true;

    public float EnemyHealth;

    private Rigidbody enemy_RB;
    private Vector3 targetDirection;
    private Vector3 newDirection;
    private Vector3 newDirectionSmoothed;
    public bool enemyIsDead;

    #region Sqare Distances for better Performace
    public float SqrDistanceToPlayer { get; private set; }
    public float SqrMinDistanceToPlayer { get; private set; }
    public float SqrMaxDistanceToPlayer { get; private set; }
    public float SqrWalkToPlayerInRange { get; private set; }
    public float SqrStopChaseDistance { get; private set; }
    public float SqrMinShootingDistance { get; private set; }
    public float SqrMaxShootingDistance { get; private set; }

    public float SqrDistancePlayerInSight { get; private set; }

    public float SqrDesiredShootingRange { get; private set; }
    #endregion

    private void OnEnable()
    {
        Pool = GetComponentInParent<ObjectPoolBehaviour>();
        CacheSquaredValues();

        Player = playerInfo.PlayerPosition; // decent solution

        if (Player == null) // in case the decent solution dont works
            Player = FindFirstObjectByType<PlayerController>().transform;

        enemyIsDead = false;
    }

    private void CacheSquaredValues()
    {
        EnemyDirSmoothSpeed = Data.enemyDirSmoothSpeed;
        EnemyHealth = Data.enemyHealth;

        SqrMinShootingDistance = Data.minShootingDistance * Data.minShootingDistance;
        SqrMaxShootingDistance = Data.maxShootingDistance * Data.maxShootingDistance;

        SqrMinDistanceToPlayer = Data.minDistanceToPlayer * Data.minDistanceToPlayer;
        SqrMaxDistanceToPlayer = Data.maxDistanceToPlayer * Data.maxDistanceToPlayer;

        SqrWalkToPlayerInRange = Data.walkToPlayerInRange * Data.walkToPlayerInRange;

        SqrDesiredShootingRange = Data.desiredShootingDistance * Data.desiredShootingDistance;
        SqrDistancePlayerInSight = Data.playerInSightDistance * Data.playerInSightDistance;

        SqrStopChaseDistance = Data.stopChaseDistance * Data.stopChaseDistance;
    }

    private void Start()
    {
        enemy_RB = GetComponent<Rigidbody>();
        if (enemy_RB != null)
        {
            enemy_RB.useGravity = false;
        }
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
        newDirection = Vector3.RotateTowards(transform.forward, targetDirection, 1, 0);

        newDirectionSmoothed = Vector3.Slerp(transform.forward, newDirection, EnemyDirSmoothSpeed * Time.deltaTime);

        transform.rotation = Quaternion.LookRotation(newDirectionSmoothed);
    }

    private void EnemyDying()
    {
        enemyIsDead = true;
        SetDefaultValues();
        ActivateRagdoll();
        //SetActive Ragdoll, SetInactiveAgent & apply force to last hit.point
        StartCoroutine(DeathTimer());
    }


    public void TakeDamage(float _dmgAmount)
    {
        if (enemyIsDead == false)
            EnemyHealth -= _dmgAmount;

        if (EnemyHealth <= 0)
            EnemyDying();
    }

    private void ActivateRagdoll()
    {
        Animator.enabled = false;
    }

    private IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(despawnTimer);
        Pool.EnqueueObject(gameObject);
        yield return new WaitForEndOfFrame();
    }

    public void SetDefaultValues()
    {
        //reset hurtboxes
        //reset animator
        //reset all joint angles
        //reset object specific values back to default
    }
}
