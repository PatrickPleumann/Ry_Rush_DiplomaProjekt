using NUnit.Framework.Interfaces;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.AI;
public class IdleState<T> : BaseState<T> where T : EnemyController
{
    private bool sawPlayer = false;

    private float animTimer;
    private float chaseDistance;

    public IdleState(T _controller) : base(_controller) // animator state is called "breathes"
    {
        animTimer = controller.Data.idleAnimTimer;
        chaseDistance = controller.Data.maxDistanceToPlayer * controller.Data.maxDistanceToPlayer;
    }

    public override BaseState<T> CheckConditions()
    {
        if (controller.enemyIsDead == true)
        {
            return new DyingState<T>(controller);
        }
        if (controller.SqrDistanceToPlayer <= chaseDistance)
        {
            return new ChaseState<T>(controller);
        }
        return null;
    }

    public override void EnterState()
    {
        controller.Agent.updateRotation = false;
        controller.Animator.SetTrigger("IdleAnim");
        controller.Agent.ResetPath();
    }

    public override void UpdateState()
    {
        //if (sawPlayer)
        //    swapStateTimer -= Time.deltaTime;

        animTimer -= Time.deltaTime;
        if (animTimer <= 0f)
        {
            controller.Animator.SetTrigger("IdleAnim2");
            animTimer = Random.Range(5,30);
        }
    }

    public override void ExitState()
    {
        controller.Animator.ResetTrigger("IdleAnim");
        controller.Animator.ResetTrigger("IdleAnim2");
    }
}