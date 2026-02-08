using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerReload : MonoBehaviour
{
    [SerializeField] private PlayerController controller;
    [SerializeField] private Animator animator;

    private void OnEnable()
    {
        controller.onReload_started.AddListener(StartReloadAnimation);
    }

    private void OnDisable()
    {
        controller.onReload_started.RemoveListener(StartReloadAnimation);
    }

    private void StartReloadAnimation()
    {
        animator.SetTrigger("ReloadAnim");
    }

    public void StartReload()
    {
        controller.canShoot = false;
    }

    public void FinishReload()
    {
        if ((controller.RemainingAmmo + controller.CurrentAmmo) >= controller.maxCurrentAmmo) // weird but works
        {
            controller.RemainingAmmo -= (controller.maxCurrentAmmo - controller.CurrentAmmo);
            controller.CurrentAmmo = controller.maxCurrentAmmo;
        }
        else // weird but works
        {
            controller.CurrentAmmo = (controller.CurrentAmmo + controller.RemainingAmmo);
            controller.RemainingAmmo = 0;
        }

        controller.onReload_Finished.Invoke();
        controller.isReloading = false;
        controller.canShoot = true;
    }
}
