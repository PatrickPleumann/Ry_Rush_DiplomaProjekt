using UnityEngine;

public class DyingState<T> : BaseState<T> where T : EnemyController
{
    private float despawnTimer;
    public DyingState(T _controller) : base(_controller)
    {
        despawnTimer = 4f;
    }

    public override BaseState<T> CheckConditions()
    {
        return null;
    }

    public override void EnterState()
    {
        Debug.Log("Enter State: DyingState");
    }

    public override void ExitState()
    {
        //nothing happens here
    }

    public override void UpdateState()
    {
        despawnTimer -= Time.deltaTime;
        if (despawnTimer <= 0f)
        {
            controller.gameObject.SetActive(false);
            //revert all properties to default & send back zu the object pool
        }
    }
}
