using NUnit.Framework.Interfaces;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.AI;
public class IdleState<T> : BaseState<T> where T : EnemyController
{
    private float swapStateTimer;
    private bool sawPlayer = false;
    private float animTimer;
    private float sqrMaxDistanceToPlayer;
    public IdleState(T _controller) : base(_controller) // animator state is called "breathes"
    {
        swapStateTimer = controller.data.swapStateTimer;
        animTimer = controller.data.idleAnimTimer;

        sqrMaxDistanceToPlayer = controller.data.maxDistanceToPlayer * controller.data.maxDistanceToPlayer;
    }

    public override BaseState<T> CheckConditions()
    {
        if (controller.SqrDistanceToPlayer <= sqrMaxDistanceToPlayer)
        {
            sawPlayer = true;
            if (swapStateTimer <= 0)
            {
                return new ChaseState<T>(controller);
            }
        }
        return null;
    }

    public override void EnterState()
    {
        Debug.Log("Enter State: Idle");
        //set all agent properties here
        controller.animator.SetTrigger("IdleAnim");
        controller.agent.ResetPath();

    }

    public override void UpdateState()
    {
        if (sawPlayer)
            swapStateTimer -= Time.deltaTime;

        animTimer -= Time.deltaTime;
        if (animTimer <= 0f)
        {
            controller.animator.SetTrigger("IdleAnim2");
            animTimer = 15f;
        }

        //swap idle animations       
    }

    public override void ExitState()
    {
        //reset all agent properties here
        controller.animator.ResetTrigger("IdleAnim");
        controller.animator.ResetTrigger("IdleAnim2");
    }
}