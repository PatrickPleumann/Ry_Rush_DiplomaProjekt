public class IdleState<T> : BaseState<T> where T : EnemyController
{

    public IdleState(T _controller) : base(_controller) // animator state is called "breathes"
    {

    }

    public override BaseState<T> CheckConditions()
    {
        return null;
    }

    public override void EnterState()
    {

    }

    public override void UpdateState()
    {

    }

    public override void ExitState()
    {

    }
}