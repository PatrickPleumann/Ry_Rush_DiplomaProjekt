using UnityEngine;

public class DyingState<T> : BaseState<T> where T : EnemyController
{
    public DyingState(T _controller) : base(_controller)
    {

    }

    public override BaseState<T> CheckConditions()
    {
        return null;
    }

    public override void EnterState()
    {
        controller.Agent.updatePosition = false;
        controller.Agent.updateRotation = false;
    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {
    }
}
