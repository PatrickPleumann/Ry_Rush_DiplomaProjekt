using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

public class MuzzleFlash : MonoBehaviour
{
    [SerializeField] private PlayerController controller;
    [SerializeField] private Transform muzzleOrigin;

    private VisualEffect muzzleFlash;

    private void Awake()
    {
        muzzleFlash = GetComponent<VisualEffect>();
    }

    private void OnEnable()
    {
        controller.onShootInvoked_started.AddListener(ShowMuzzleFlash);
    }

    private void OnDisable()
    {
        controller.onShootInvoked_started.RemoveListener(ShowMuzzleFlash);
    }

    private void ShowMuzzleFlash(InputAction.CallbackContext ctx)
    {
        muzzleFlash.Play();
    }
}
