using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerMovement_New movement;
    [SerializeField] private PlayerDash playerDash;
    [SerializeField] private BeatTracking beat;
    [SerializeField] public Scoreboard_UI scoreboard;
    [SerializeField] private WeaponAnimBehaviour weaponAnims;


    [Header("Input References")]
    [SerializeField] public InputActionReference Move;
    [SerializeField] public InputActionReference Look;
    [SerializeField] public InputActionReference Jump;
    [SerializeField] public InputActionReference Dash;
    [SerializeField] public InputActionReference Shoot;
    [SerializeField] public InputActionReference Sprint;
    [SerializeField] public InputActionReference Aim;
    [SerializeField] public InputActionReference SlowMotion;

    [Header("Centralized Values")]
    [SerializeField] public Transform Orientation;
    [SerializeField] private float shootingCooldownTimer;
    [SerializeField] public bool SlowMotion_OnAim;

    public Vector3 moveInput;
    public Vector3 moveDirection;
    public bool AllowMovement;
    public bool IsOnBeat;
    public bool LastActionOnBeat;



    public UnityEvent<InputAction.CallbackContext> onShootInvoked_started;

    public UnityEvent<InputAction.CallbackContext> onAimInvoked_started;
    public UnityEvent<InputAction.CallbackContext> onAimInvoked_canceled;

    public UnityEvent<InputAction.CallbackContext> onJumpInvoked_started;

    public UnityEvent<InputAction.CallbackContext> onDashInvoked_started;

    public UnityEvent<InputAction.CallbackContext> onSlowMotion_started;
    public UnityEvent<InputAction.CallbackContext> onSlowMotion_canceled;

    private float lastActionOnBeatTime;
    private bool canShoot = true;



    private void Awake()
    {

        Cursor.lockState = CursorLockMode.Locked;
        AllowMovement = true;
        IsOnBeat = false;
    }
    private void OnEnable()
    {

        Jump.action.started += onJumpInvoked_started.Invoke;

        Shoot.action.started += ProcessShootInput;

        Aim.action.started += onAimInvoked_started.Invoke;
        Aim.action.canceled += onAimInvoked_canceled.Invoke;

        Dash.action.started += onDashInvoked_started.Invoke;

        SlowMotion.action.started += onSlowMotion_started.Invoke;
        SlowMotion.action.canceled += onSlowMotion_canceled.Invoke;
    }
    private void Start()
    {
        lastActionOnBeatTime = (((1 / beat.bpm) * 60) - beat.beatOffsetMultiplier);
        Debug.Log(lastActionOnBeatTime + 0.1f);
    }


    private void Update()
    {
        GetMoveDirection();
        IsOnBeat = beat.Return_IsOnBeat();
    }
    private void OnDisable()
    {
        Jump.action.started -= onJumpInvoked_started.Invoke;

        Shoot.action.started -= onShootInvoked_started.Invoke;

        Aim.action.started -= onAimInvoked_started.Invoke;
        Aim.action.canceled -= onAimInvoked_canceled.Invoke;

        Dash.action.started -= onDashInvoked_started.Invoke;

        SlowMotion.action.started -= onSlowMotion_started.Invoke;
        SlowMotion.action.canceled -= onSlowMotion_canceled.Invoke;
    }

    private void ProcessShootInput(InputAction.CallbackContext ctx)
    {
        if (canShoot == true)
        {
            StartCoroutine(ShootTimer());
            onShootInvoked_started.Invoke(ctx);
        }
    }
    private void GetMoveDirection()
    {
        moveInput.x = Move.action.ReadValue<Vector2>().x;
        moveInput.y = 0f;
        moveInput.z = Move.action.ReadValue<Vector2>().y;

        moveDirection = Orientation.forward * moveInput.z + Orientation.right * moveInput.x;
    }
    private IEnumerator LastActionOnBeatTimer()
    {
        LastActionOnBeat = true;
        yield return new WaitForSeconds(lastActionOnBeatTime);
        LastActionOnBeat = false;
        yield break;
    }

    private IEnumerator ShootTimer()
    {
        canShoot = false;
        yield return new WaitForSeconds(shootingCooldownTimer);
        canShoot = true;
    }
}


