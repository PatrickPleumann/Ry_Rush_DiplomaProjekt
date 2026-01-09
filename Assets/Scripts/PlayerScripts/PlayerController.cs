using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerMovement_New movement;
    [SerializeField] private PlayerShooting shooting;
    [SerializeField] private AudioHandler audioHandler;
    [SerializeField] private PlayerAim playerAim;

    [Header("Input References")]
    [SerializeField] public InputActionReference move;
    [SerializeField] public InputActionReference look;
    [SerializeField] public InputActionReference jump;
    [SerializeField] public InputActionReference dash;
    [SerializeField] public InputActionReference shoot;
    [SerializeField] public InputActionReference sprint;
    [SerializeField] public InputActionReference aim;

    private void Awake()
    {

        Cursor.lockState = CursorLockMode.Locked;
    }
    private void OnEnable()
    {
        jump.action.started += movement.Jump;
        jump.action.started += movement.WallJump;

        shoot.action.started += shooting.Player_ShootWeapon;
        shoot.action.started += PlaySingleShootSound;

        aim.action.started += playerAim.Zoom_True;
        aim.action.started += playerAim.ReduceMouseSensitivity;

        aim.action.canceled += playerAim.Zoom_False;
        aim.action.canceled += playerAim.ReIncreaseReducedMouseSensitivity;

    }

    private void OnDisable()
    {
        jump.action.started -= movement.Jump;
        jump.action.started -= movement.WallJump;

        shoot.action.started -= shooting.Player_ShootWeapon;

        aim.action.started -= playerAim.Zoom_True;
        aim.action.started -= playerAim.ReduceMouseSensitivity;

        aim.action.canceled -= playerAim.Zoom_False;
        aim.action.canceled -= playerAim.ReIncreaseReducedMouseSensitivity;
    }

    private void PlaySingleShootSound(InputAction.CallbackContext ctx)
    {
        audioHandler.PlaySound_sourceShooting(audioHandler.playerShoot[Random.Range(0, audioHandler.playerShoot.Length)]);
    }
}
