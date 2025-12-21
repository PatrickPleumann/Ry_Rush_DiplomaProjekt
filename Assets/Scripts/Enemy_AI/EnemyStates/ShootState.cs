using UnityEngine;

public class ShootState<T> : BaseState<T> where T : EnemyController
{
    private bool canShoot = true;
    private float shootingTimer;
    public ShootState(T _controller) : base(_controller)
    {
        shootingTimer = 0f;
    }

    public override BaseState<T> CheckConditions()
    {
        return null;
    }

    public override void EnterState()
    {
        Debug.Log("Enter State: ShootState");
        controller.agent.updatePosition = false; // could work maybe
        controller.agent.updateRotation = true;
        controller.agent.speed = 1f; // just for turning the enemy while shooting
    }

    public override void UpdateState()
    {
        //shoot
        shootingTimer -= Time.deltaTime;
        controller.agent.destination = controller.player.position;
        if (canShoot == true && shootingTimer <= 0f && (controller.SqrDistanceToPlayer <= controller.SqrDesiredShootingRange))
        {
            controller.animator.SetTrigger("ShootAnim");
            canShoot = false;
            shootingTimer = controller.data.betweenShotsTimer;
        }
    }

    public override void ExitState()
    {
        Debug.Log("Exit State: ShootState");
        controller.animator.ResetTrigger("ShootAnim");
    }
}
