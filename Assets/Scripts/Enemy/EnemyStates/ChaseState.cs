using UnityEngine;
using UnityEngine.AI;

public class ChaseState<T> : BaseState<T> where T : EnemyController
{
    //local for shortness
    private float walk;
    private float run;
    private float chaseStateTimer = 1f;
    public ChaseState(T _controller) : base(_controller) 
    {
        walk = controller.SqrWalkToPlayerInRange;
        run = controller.SqrRunToPlayerInRange;
    }

    public override BaseState<T> CheckConditions()
    {
        if (controller.SqrDistanceToPlayer <= controller.SqrDesiredShootingRange)
        {
            return new ShootState<T>(controller);
        }
        return null;
    }

    public override void EnterState()
    {
        Debug.Log("Enter State: Chase");
        controller.Data.canSeePlayer = true;
        controller.Agent.destination = controller.Player.position;
    }

    public override void ExitState()
    {
        controller.Animator.ResetTrigger("RunAnim");
        controller.Animator.ResetTrigger("WalkAnim");
    }

    public override void UpdateState()
    {
        controller.UpdateEnemyRotation();

        chaseStateTimer -= Time.deltaTime;

        if (controller.SqrDistanceToPlayer >= walk && controller.SqrDistanceToPlayer <= run && chaseStateTimer <= 0f)
        {
            controller.Animator.SetTrigger("RunAnim");
            controller.Agent.speed = controller.Data.enemyMaxSpeedRunning;
        }
        else if(controller.SqrDistanceToPlayer < (walk - 1) && chaseStateTimer <= 0f)
        {
            controller.Animator.SetTrigger("WalkAnim");
            controller.Agent.speed = controller.Data.enemyMaxSpeedWalking;
        }

        controller.Agent.destination = controller.Player.position;
    }
}
