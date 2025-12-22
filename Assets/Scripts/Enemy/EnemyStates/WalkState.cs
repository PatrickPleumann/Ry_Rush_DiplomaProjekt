using UnityEngine;

public class WalkState<T> : BaseState<T> where T : EnemyController
{
    public WalkState(T _controller) : base(_controller) {}

    public override BaseState<T> CheckConditions()
    {
        if (controller.SqrDistanceToPlayer <= controller.SqrMaxShootingDistance)
        {
            return new ShootState<T>(controller);
        }
        return null;
    }

    public override void EnterState()
    {
        Debug.Log(controller.SqrDistanceToPlayer);
        Debug.Log("Enter State: Walk");
        controller.Animator.SetTrigger("Walk");
        controller.Agent.speed = controller.Data.enemyMaxSpeedWalking;
    }

    public override void ExitState()
    {
        controller.Animator.ResetTrigger("Walk");
    }

    public override void UpdateState()
    {
        // set agent destination to patrol transforms array
    }
}
