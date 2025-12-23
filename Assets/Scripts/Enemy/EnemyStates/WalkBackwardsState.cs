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
        Debug.Log("Enter State: WalkBackwardsState");
        controller.Animator.SetTrigger("BackwardsAnim");
    }

    public override void UpdateState()
    {
    }

    public override void ExitState()
    {
        Debug.Log("Exit State: WalkBackwardsState");
        controller.Animator.ResetTrigger("BackwardsAnim");
    }

}
