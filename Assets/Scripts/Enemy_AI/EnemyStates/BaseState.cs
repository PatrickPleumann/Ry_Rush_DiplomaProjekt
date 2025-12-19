public abstract class BaseState<T> where T : EnemyController
{
    public T controller;

    public BaseState(T _controller)
    {
        controller = _controller;
    }
    public abstract BaseState<T> CheckConditions();
    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
}