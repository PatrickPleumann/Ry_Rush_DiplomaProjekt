using UnityEditor;
using UnityEngine;

public class ShootState<T> : BaseState<T> where T : EnemyController
{
    private bool canShoot = true;
    private float shootingTimer;
    private float min;
    private float max;
    public ShootState(T _controller) : base(_controller)
    {
        shootingTimer = 0f;
        min = controller.SqrMinShootingDistance;
        max = controller.SqrMaxShootingDistance;
    }

    public override BaseState<T> CheckConditions()
    {
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
        //shoot
        shootingTimer -= Time.deltaTime;
        if (controller.SqrDistanceToPlayer > min && controller.SqrDistanceToPlayer < max)
        {
            controller.agent.destination = controller.player.position;
            Debug.Log(controller.SqrDistanceToPlayer);
        }

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
