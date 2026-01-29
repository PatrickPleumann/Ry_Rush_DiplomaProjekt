using UnityEngine;
using UnityEngine.AI;

public class ChaseState<T> : BaseState<T> where T : EnemyController
{
    private float chaseStateTimer;
    public ChaseState(T _controller) : base(_controller)
    {
        chaseStateTimer = 1f;
    }

    public override BaseState<T> CheckConditions()
    {
        if (controller.enemyIsDead == true)
        {
            return new DyingState<T>(controller);
        }
        if (controller.SqrDistanceToPlayer <= controller.SqrDesiredShootingRange)
        {
            return new ShootState<T>(controller);
        }
        if (controller.SqrStopChaseDistance < controller.SqrDistanceToPlayer)
        {
            return new IdleState<T>(controller);
        }
        return null;
    }

    public override void EnterState()
    {
        controller.Data.canSeePlayer = true;
    }

    public override void ExitState()
    {
        controller.Animator.ResetTrigger("WalkAnim");
    }

    public override void UpdateState()
    {
        controller.UpdateEnemyRotation();
        chaseStateTimer -= Time.deltaTime;

        if (chaseStateTimer <= 0)
        {
            controller.Animator.SetTrigger("WalkAnim");
            controller.Agent.speed = controller.Data.enemyMaxSpeedWalking;
            controller.Agent.destination = controller.Player.position;
        }
    }
}
