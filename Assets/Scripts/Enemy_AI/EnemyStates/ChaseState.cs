using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState<T> : BaseState<T> where T : EnemyController
{
    //walk & run distance to player (for shortness)
    private float walk;
    private float run;
    public ChaseState(T _controller) : base(_controller) 
    {
        walk = controller.SqrWalkToPlayerRange;
        run = controller.SqrRunToPlayerRange;
    }

    public override BaseState<T> CheckConditions()
    {

        return null;
    }

    public override void EnterState()
    {
        Debug.Log("Enter State: Chase");

        Debug.Log(controller.SqrDistanceToPlayer);
        Debug.Log(controller.SqrMaxDistanceToPlayer);
        controller.data.canSeePlayer = true;
        controller.agent.destination = controller.player.position;
    }

    public override void ExitState()
    {
        controller.agent.isStopped = true;
        controller.agent.ResetPath();
        controller.data.canSeePlayer = false;
    }

    public override void UpdateState()
    {
        if (controller.SqrDistanceToPlayer >= walk && controller.SqrDistanceToPlayer <= run)
        {
            controller.animator.SetTrigger("RunAnim");
            controller.agent.speed = controller.data.enemyMaxSpeedRunning;
        }
        else if(controller.SqrDistanceToPlayer < walk)
        {
            controller.animator.SetTrigger("WalkAnim");
            controller.agent.speed = controller.data.enemyMaxSpeedWalking;
        }

        controller.agent.destination = controller.player.position;
    }
}
