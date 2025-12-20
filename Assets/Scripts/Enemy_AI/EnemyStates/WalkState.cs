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
        controller.animator.SetTrigger("Walk");
        controller.agent.speed = controller.data.enemyMaxSpeedWalking;
    }

    public override void ExitState()
    {
        controller.animator.ResetTrigger("Walk");
    }

    public override void UpdateState()
    {
        // set agent destination to patrol transforms array
    }
}
