using System.ComponentModel;
using UnityEngine;
using UnityEngine.AI;
public class IdleState<T> : BaseState<T> where T : EnemyController
{
    private float timer;
    public IdleState(T _controller) : base(_controller) // animator state is called "breathes"
    {
        timer = controller.data.idleTimer;
    }

    public override BaseState<T> CheckConditions()
    {
        if (timer <= 0f)
        {
            
        }
        
        //can return other states immedietly
        return null;
    }

    public override void EnterState()
    {
        //set all agent properties here
        controller.animator.SetTrigger("Idle");
        controller.agent.ResetPath();
        
    }

    public override void UpdateState()
    {
        timer -= Time.deltaTime;

    }

    public override void ExitState()
    {
        //reset all agent properties here
        controller.animator.ResetTrigger("Idle");
    }
}