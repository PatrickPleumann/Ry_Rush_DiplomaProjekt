using UnityEngine;

public class WalkBackwardsState<T> : BaseState<T> where T : EnemyController
{
    public WalkBackwardsState(T _controller) : base(_controller)
    {
    }

    public override BaseState<T> CheckConditions()
    {
        return null;
    }

    public override void EnterState()
    {
    }

    public override void ExitState()
    {
    }

    public override void UpdateState()
    {
    }
}
