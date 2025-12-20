using UnityEngine;

public class ShootState<T> : BaseState<T> where T : EnemyController
{
    public ShootState(T _controller) : base(_controller)
    {
    }

    public override BaseState<T> CheckConditions()
    {
        return null;
    }

    public override void EnterState()
    {
        controller.animator.SetTrigger("Shoot");
    }

    public override void ExitState()
    {
        controller.animator.ResetTrigger("Shoot");
    }

    public override void UpdateState()
    {
        throw new System.NotImplementedException();
    }
}
