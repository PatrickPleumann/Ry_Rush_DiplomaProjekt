using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerReload : MonoBehaviour
{
    [SerializeField] private PlayerController controller;
    [SerializeField] private Animator animator;

    private void OnEnable()
    {
        controller.OnReload_started.AddListener(StartReloadAnimation);
    }

    private void OnDisable()
    {
        controller.OnReload_started.RemoveListener(StartReloadAnimation);
    }

    private void StartReloadAnimation()
    {
        animator.SetTrigger("ReloadAnim");
    }

    public void StartReload()
    {
        controller.CanShoot = false;
    }

    public void FinishReload()
    {
        if ((controller.RemainingAmmo + controller.CurrentAmmo) >= controller.MaxCurrentAmmo) // weird but works
        {
            controller.RemainingAmmo -= (controller.MaxCurrentAmmo - controller.CurrentAmmo);
            controller.CurrentAmmo = controller.MaxCurrentAmmo;
        }
        else // weird but works
        {
            controller.CurrentAmmo = (controller.CurrentAmmo + controller.RemainingAmmo);
            controller.RemainingAmmo = 0;
        }

        controller.OnReload_Finished.Invoke();
        controller.IsReloading = false;
        controller.CanShoot = true;
    }
}
