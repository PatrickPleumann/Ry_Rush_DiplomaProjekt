using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerMovement_New movement;
    [SerializeField] private PlayerShooting shooting;
    [SerializeField] private PlayerAim playerAim;
    [SerializeField] private PlayerDash playerDash;
    [SerializeField] private AudioHandler audioHandler;

    [Header("Input References")]
    [SerializeField] public InputActionReference Move;
    [SerializeField] public InputActionReference Look;
    [SerializeField] public InputActionReference Jump;
    [SerializeField] public InputActionReference Dash;
    [SerializeField] public InputActionReference Shoot;
    [SerializeField] public InputActionReference Sprint;
    [SerializeField] public InputActionReference Aim;

    [Header("Centralized Values")]
    [SerializeField] public Transform Orientation;
    public Vector3 moveInput;
    public Vector3 moveDirection;
    public bool AllowMovement = true;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void OnEnable()
    {
        Jump.action.started += movement.Jump;
        Jump.action.started += movement.WallJump;

        Shoot.action.started += shooting.Player_ShootWeapon;
        Shoot.action.started += PlaySingleShootSound;

        Aim.action.started += playerAim.Zoom_True;
        Aim.action.started += playerAim.ReduceMouseSensitivity;

        Aim.action.canceled += playerAim.Zoom_False;
        Aim.action.canceled += playerAim.ReIncreaseReducedMouseSensitivity;

        Dash.action.started += playerDash.Dash;

    }

    private void Update()
    {
        GetMoveDirection();
    }
    private void OnDisable()
    {
        Jump.action.started -= movement.Jump;
        Jump.action.started -= movement.WallJump;

        Shoot.action.started -= shooting.Player_ShootWeapon;

        Aim.action.started -= playerAim.Zoom_True;
        Aim.action.started -= playerAim.ReduceMouseSensitivity;

        Aim.action.canceled -= playerAim.Zoom_False;
        Aim.action.canceled -= playerAim.ReIncreaseReducedMouseSensitivity;

        Dash.action.started -= playerDash.Dash;
    }
    private void PlaySingleShootSound(InputAction.CallbackContext ctx)
    {
        audioHandler.PlaySound_sourceShooting(audioHandler.playerShoot[Random.Range(0, audioHandler.playerShoot.Length)]);
    }

    private void GetMoveDirection()
    {
        moveInput.x = Move.action.ReadValue<Vector2>().x;
        moveInput.y = 0f;
        moveInput.z = Move.action.ReadValue<Vector2>().y;

        moveDirection = Orientation.forward * moveInput.z + Orientation.right * moveInput.x;
    }
}


