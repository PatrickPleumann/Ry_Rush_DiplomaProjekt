using Mono.Cecil.Cil;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.VFX;

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
    [SerializeField] private Transform enemyWeaponOrigin;

    [SerializeField] private VisualEffect enemy_muzzleFlash;

    private RaycastHit hitInfo;

    [SerializeField] private CentralizedValues values;

    [SerializeField] private int initalPoints = 500; // this values gets increased or reduced based on the hits to kill and the multipliers
    private int hitsTillDeath; // set default

    [SerializeField] private Rigidbody[] allRagdollRigidbodies;
    //[SerializeField] public EnemyMaxChasingCounter maxEnemiesChasing;  // need to rethink whole logic behind this behaviour

    public float EnemyDirSmoothSpeed;
    [SerializeField] private float despawnTimer = 1;
    public bool canChase = true;

    public float EnemyHealth;

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
        allRagdollRigidbodies = GetComponentsInChildren<Rigidbody>();

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
        SetInactiveRagdollRigibodies();
        Pool = GetComponentInParent<ObjectPoolBehaviour>();
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
        values.Kills++;
        enemyIsDead = true;
        values.EnemysAliveCount = values.EnemysAliveCount - 1;
        ActivateRagdoll();
        //SetActive Ragdoll, SetInactiveAgent & apply force to last hit.point
        CalculatePoints();
        StartCoroutine(DeathTimer());
    }

    private void CalculatePoints()
    {
        if (hitsTillDeath > 0)
        {
            var temp = ((float)(initalPoints / hitsTillDeath)); // hier noch multiplier hinzufügen
            values.CurrentScore_Value = values.CurrentScore_Value + temp;
        }
        else
            Debug.Log("Hits till death are below 1, which can´t be");
    }


    public void TakeDamage(float _dmgAmount)
    {
        if (enemyIsDead == false)
        {
            values.ShotsHit++;
            EnemyHealth -= _dmgAmount;
            hitsTillDeath++;
        }

        if (EnemyHealth <= 0 && enemyIsDead == false)
            EnemyDying();
    }

    private void ActivateRagdoll()
    {
        Animator.enabled = false;
        SetActiveRagdollRigidbodies();
    }

    private void SetActiveRagdollRigidbodies() // this is necessary or all rigidbodies, which are suppressed by the animator will unload an
    {                                          // enormous amount of energy, which causes very weird ragdoll behaviour
        if (allRagdollRigidbodies != null)
        {
            foreach (var item in allRagdollRigidbodies)
                item.isKinematic = false;
        }
    }

    private void SetInactiveRagdollRigibodies() // this is necessary or all rigidbodies, which are suppressed by the animator, will unload an
    {                                           // enormous amount of energy, which causes very weird ragdoll behaviour
        if (allRagdollRigidbodies != null)
        {
            foreach (var item in allRagdollRigidbodies)
                item.isKinematic = true;
        }
    }

    private IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(despawnTimer);

        if (Pool != null)
        {
            SetDefaultValues();
            Pool.EnqueueObject(gameObject);
        }

        else
            Destroy(gameObject);

        yield return new WaitForEndOfFrame();
    }

    public void SetDefaultValues()
    {
        SetInactiveRagdollRigibodies();
        //reset hits till kill
        //reset animator
        //more to come...
    }

    public void Enemy_Shoot()
    {
        var temp = Physics.Raycast(enemyWeaponOrigin.position, enemyWeaponOrigin.forward, out hitInfo, 20f);
        enemy_muzzleFlash.Play();
        if (temp == true && hitInfo.transform.TryGetComponent(out PlayerHealthSystem hit))
        {
            if (hit == true && hit != null)
                hit.TakeDamage_Player();
        }
    }
}
