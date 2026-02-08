using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class UI_Ammo : MonoBehaviour
{

    [SerializeField] private PlayerController controller;
    [SerializeField] private TMP_Text currentAmmo;
    [SerializeField] private TMP_Text remainingAmmo;

    private void OnEnable()
    {
        controller.onShootInvoked_started.AddListener(ShowCurrentAmmo_UI);
        controller.onReload_Finished.AddListener(ShowRemainingAmmo_UI);
    }

    private void OnDisable()
    {
        controller.onShootInvoked_started.RemoveListener(ShowCurrentAmmo_UI);
        controller.onReload_Finished.RemoveListener(ShowRemainingAmmo_UI);
    }
    private void Start()
    {
        currentAmmo.text = controller.CurrentAmmo.ToString();
        remainingAmmo.text = controller.RemainingAmmo.ToString();
    }

    private void ShowCurrentAmmo_UI()
    {
        currentAmmo.text = controller.CurrentAmmo.ToString();
    }

    private void ShowRemainingAmmo_UI()
    {
        remainingAmmo.text = controller.RemainingAmmo.ToString();
    }

    public void OnReloadFinished()
    {
        currentAmmo.text = controller.CurrentAmmo.ToString();
        remainingAmmo.text = controller.RemainingAmmo.ToString();
    }
}
