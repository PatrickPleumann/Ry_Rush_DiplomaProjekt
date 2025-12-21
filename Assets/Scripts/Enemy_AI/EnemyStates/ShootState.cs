using UnityEditor;
using UnityEngine;

public class ShootState<T> : BaseState<T> where T : EnemyController
{
    private bool canShoot = true;
    private float min; // for shortness
    private float max; // for shortness
    public ShootState(T _controller) : base(_controller)
    {
        min = controller.SqrMinShootingDistance;
        max = controller.SqrMaxShootingDistance;
    }

    public override BaseState<T> CheckConditions()
    {
        if (controller.SqrDistanceToPlayer > max)
        {
            return new ChaseState<T>(controller);
        }
        if (controller.SqrDistanceToPlayer > controller.SqrDistancePlayerInSight)
        {
            return new IdleState<T>(controller);
        }
        if (controller.SqrDistanceToPlayer < controller.SqrMinShootingDistance)
        {
            return new WalkBackwardsState<T>(controller);
        }
        return null;
    }

    public override void EnterState()
    {
        Debug.Log("Enter State: ShootState");
        //controller.agent.updatePosition = false; // could work maybe
        controller.agent.updateRotation = true;
        controller.agent.speed = 0.05f; // just for turning the enemy while shooting
    }

    public override void UpdateState()
    {
        if (controller.SqrDistanceToPlayer > min && controller.SqrDistanceToPlayer < max)
        {
            controller.agent.destination = controller.player.position;
        }

        if (canShoot == true &&  (controller.SqrDistanceToPlayer <= controller.SqrDesiredShootingRange))
        {
            controller.animator.SetTrigger("ShootAnim");
            canShoot = false;
        }
    }

    public override void ExitState()
    {
        Debug.Log("Exit State: ShootState");
        controller.animator.ResetTrigger("ShootAnim");
    }
}
