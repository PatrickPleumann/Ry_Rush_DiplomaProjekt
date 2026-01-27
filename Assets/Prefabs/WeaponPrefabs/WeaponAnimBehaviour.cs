using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponAnimBehaviour : MonoBehaviour
{
    [SerializeField] private PlayerController controller;
    [SerializeField] private Animator animator;

    private void OnEnable()
    {
        controller.onShootInvoked_started.AddListener(SetShotAnim);   
    }

    private void OnDisable()
    {
        controller.onShootInvoked_started.RemoveListener(SetShotAnim);
    }
    public void SetShotAnim(InputAction.CallbackContext ctx)
    {
        animator.SetTrigger("ShootAnim");
    }

    public void SetReloadAnim(InputAction.CallbackContext ctx)
    {
        animator.SetTrigger("ReloadAnim");
    }

}
