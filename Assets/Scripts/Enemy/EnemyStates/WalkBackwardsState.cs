using UnityEngine;

public class WalkBackwardsState<T> : BaseState<T> where T : EnemyController
{
    private float min; // for shortness
    private float max; // for shortness

    public WalkBackwardsState(T _controller) : base(_controller)
    {
        min = controller.SqrMinShootingDistance;
        max = controller.SqrMaxShootingDistance;
    }

    public override BaseState<T> CheckConditions()
    {
        if (controller.SqrDistanceToPlayer <= max && controller.SqrDistanceToPlayer >= min)
        {
            return new ShootState<T>(controller);
        }
        return null;
    }

    public override void EnterState()
    {
        controller.Agent.speed = 1.5f;
        controller.Agent.updateRotation = false;
        Debug.Log("Enter State: WalkBackwardsState");
        controller.Animator.SetTrigger("BackwardsAnim");
    }

    public override void UpdateState()
    {
        controller.Agent.destination = -controller.Player.position;
        controller.UpdateEnemyRotation();
    }

    public override void ExitState()
    {
        Debug.Log("Exit State: WalkBackwardsState");
        controller.Animator.ResetTrigger("BackwardsAnim");
    }
}
