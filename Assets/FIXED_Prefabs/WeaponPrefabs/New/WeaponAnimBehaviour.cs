using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponAnimBehaviour : MonoBehaviour
{

    [SerializeField] private Animator animator;
    public void SetShotAnim(InputAction.CallbackContext ctx)
    {
        animator.SetTrigger("ShootAnim");
    }

    public void SetReloadAnim(InputAction.CallbackContext ctx)
    {
        animator.SetTrigger("ReloadAnim");
    }

}
