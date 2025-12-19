using UnityEngine;

public class Enemy_FSM<T> where T : EnemyController
{
    public BaseState<T> currentState;
    public T controller;
    public Enemy_FSM(T _controller)
    {
        controller = _controller;

        currentState = new IdleState<T>(controller);
    }

    /// <summary>
    /// Update method for updating states in FSM. Gets called every frame from MonoBehaviour script.
    /// </summary>
    public void Update()
    {
        BaseState<T> newState = currentState.CheckConditions();
        if (newState is not null)
        {
            currentState.ExitState();
            newState.EnterState();
            currentState = newState;
        }
        currentState.UpdateState();
    }
}
