using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponAnimBehaviour : MonoBehaviour
{
    [SerializeField] private PlayerController controller;
    [SerializeField] private Animator animator;
    private void Awake()
    {
        float temp = Mathf.Floor((103f / 60f) * 100) * 0.01f; // reduced to two digits after
        animator.speed *= temp;
    }
    private void OnEnable()
    {
        controller.onShootInvoked_started.AddListener(SetShotAnim);   
    }

    private void OnDisable()
    {
        controller.onShootInvoked_started.RemoveListener(SetShotAnim);
    }
    public void SetShotAnim()
    {
        animator.SetTrigger("ShootAnim");
    }

    public void SetReloadAnim()
    {
        animator.SetTrigger("ReloadAnim");
    }

}
