using UnityEngine;

public class PatrolState<T> : BaseState<T> where T : EnemyController
{
    public PatrolState(T _controller) : base(_controller)
    {

    }

    public override BaseState<T> CheckConditions()
    {
        return null;
    }

    public override void EnterState()
    {
        Debug.Log("Enter State: Patrol");
        controller.animator.SetTrigger("WalkAnim");
    }

    public override void UpdateState()
    {

    }

    public override void ExitState()
    {
        controller.animator.ResetTrigger("WalkAnim");
    }

}
