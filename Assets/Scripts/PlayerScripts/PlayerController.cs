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

    [Space]

    [Header("Input References")]
    [SerializeField] public InputActionReference Move;
    [SerializeField] public InputActionReference Look;
    [SerializeField] private InputActionReference Jump;
    [SerializeField] private InputActionReference Dash;
    [SerializeField] private InputActionReference Shoot;
    [SerializeField] private InputActionReference Aim;
    [SerializeField] private InputActionReference SlowMotion;
    [SerializeField] private InputActionReference Reload;

    [Space]

    [Header("Unity Events")]
    [HideInInspector] public UnityEvent<InputAction.CallbackContext> onShootInvoked_started;

    [HideInInspector] public UnityEvent<InputAction.CallbackContext> onAimInvoked_started;
    [HideInInspector] public UnityEvent<InputAction.CallbackContext> onAimInvoked_canceled;

    [HideInInspector] public UnityEvent<InputAction.CallbackContext> onJumpInvoked_started;

    [HideInInspector] public UnityEvent<InputAction.CallbackContext> onDashInvoked_started;

    [HideInInspector] public UnityEvent<InputAction.CallbackContext> onSlowMotion_started;
    [HideInInspector] public UnityEvent<InputAction.CallbackContext> onSlowMotion_canceled;
    [HideInInspector] public UnityEvent<InputAction.CallbackContext> onReload_started;
    [HideInInspector] public UnityEvent onReload_Finished;


    [Space]

    [Header("Centralized Values")]
    [SerializeField] public Transform Orientation;
    [SerializeField] private float shootingCooldownTimer;
    [SerializeField] public bool SlowMotion_OnAim;

    [HideInInspector] public Vector3 moveInput;
    [HideInInspector] public Vector3 moveDirection;

    public bool AllowMovement;
    [HideInInspector] public bool IsOnBeat;
    [HideInInspector] public bool LastActionOnBeat;
    [HideInInspector] public bool isReloading = false;
    [HideInInspector] public bool canShoot = true;

    [Space]

    [SerializeField] public int maxCurrentAmmo;
    [SerializeField] public int maxRemainingAmmo;

    [Space]

    public int CurrentAmmo;
    public int RemainingAmmo;
    private float lastActionOnBeatTime;

    private void Awake()
    {
        RemainingAmmo = maxRemainingAmmo;
        CurrentAmmo = maxCurrentAmmo;
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

        Reload.action.started += ProcessReloadInput;

        onReload_started.AddListener(ProcessReloadInput);
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

        Reload.action.started -= ProcessReloadInput;

        onReload_started.RemoveListener(ProcessReloadInput);
    }


    private void Start()
    {
        lastActionOnBeatTime = (((1 / beat.bpm) * 60) - beat.beatOffsetMultiplier);
    }


    private void Update()
    {
        GetMoveDirection();
        IsOnBeat = beat.Return_IsOnBeat();
    }

    private void ProcessShootInput(InputAction.CallbackContext ctx)
    {
        if (canShoot == true && CurrentAmmo > 0)
        {
            CurrentAmmo--;
            StartCoroutine(ShootTimer());
            onShootInvoked_started.Invoke(ctx);
        }

        else if (isReloading == false && CurrentAmmo == 0 && CurrentAmmo < maxCurrentAmmo && RemainingAmmo > 0)
            ProcessReloadInput(ctx);
    }

    private void ProcessReloadInput(InputAction.CallbackContext ctx)
    {
        if (isReloading == false && CurrentAmmo < maxCurrentAmmo && RemainingAmmo > 0)
        {
            isReloading = true;
            onReload_started.Invoke(ctx);
        }
    }

    private void GetMoveDirection()
    {
        if (AllowMovement == true)
        {
            moveInput.x = Move.action.ReadValue<Vector2>().x;
            moveInput.y = 0f;
            moveInput.z = Move.action.ReadValue<Vector2>().y;

            moveDirection = Orientation.forward * moveInput.z + Orientation.right * moveInput.x;
        }
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


