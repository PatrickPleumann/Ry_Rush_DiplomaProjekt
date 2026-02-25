using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponAnimBehaviour : MonoBehaviour
{
    [SerializeField] private PlayerController controller;
    [SerializeField] private Animator animator;
    [SerializeField] private SongData data;

    private void Start()
    {
        var temp = data.BPM / 60;
        temp = Utility.FloorFloatToTwoDigits(temp);
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
