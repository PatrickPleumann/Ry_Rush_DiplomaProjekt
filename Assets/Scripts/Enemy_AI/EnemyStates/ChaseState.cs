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
        controller.data.canSeePlayer = true;
        controller.agent.destination = controller.player.position;
    }

    public override void ExitState()
    {
        controller.animator.ResetTrigger("RunAnim");
        controller.animator.ResetTrigger("WalkAnim");
    }

    public override void UpdateState()
    {
        chaseStateTimer -= Time.deltaTime;
        if (controller.SqrDistanceToPlayer >= walk && controller.SqrDistanceToPlayer <= run && chaseStateTimer <= 0f)
        {
            controller.animator.SetTrigger("RunAnim");
            controller.agent.speed = controller.data.enemyMaxSpeedRunning;
        }
        else if(controller.SqrDistanceToPlayer < walk && chaseStateTimer <= 0f)
        {
            controller.animator.SetTrigger("WalkAnim");
            controller.agent.speed = controller.data.enemyMaxSpeedWalking;
        }

        controller.agent.destination = controller.player.position;
    }
}
