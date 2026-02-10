using System;
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
    [SerializeField] private CentralizedValues values;

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
    [SerializeField] private InputActionReference Esc;

    [Space]

    [Header("Unity Events")]
    [HideInInspector] public UnityEvent onShootInvoked_started;

    [HideInInspector] public UnityEvent onAimInvoked_started;
    [HideInInspector] public UnityEvent onAimInvoked_canceled;

    [HideInInspector] public UnityEvent onJumpInvoked_started;

    [HideInInspector] public UnityEvent onDashInvoked_started;

    [HideInInspector] public UnityEvent onSlowMotion_started;
    [HideInInspector] public UnityEvent onSlowMotion_canceled;

    [HideInInspector] public UnityEvent onReload_started;
    [HideInInspector] public UnityEvent onReload_Finished;

    [HideInInspector] public UnityEvent onEsc_started;

    [Space]

    [Header("Centralized Values")]
    [SerializeField] public Transform Orientation;
    [SerializeField] private float shootingCooldownTimer;
    [SerializeField] public bool SlowMotion_OnAim;

    [HideInInspector] public Vector3 moveInput; // into CV_SO
    [HideInInspector] public Vector3 moveDirection;

    public bool AllowMovement;
    [HideInInspector] public bool IsOnBeat; // into CV_SO
    [HideInInspector] public bool LastActionOnBeat; // into CV_SO
    [HideInInspector] public bool isReloading = false; // into CV_SO
    [HideInInspector] public bool canShoot = true; // into CV_SO

    [Space]

    [SerializeField] public int maxCurrentAmmo;
    [SerializeField] public int maxRemainingAmmo;

    [Space]

    public int CurrentAmmo; // into CV_SO
    public int RemainingAmmo; // into CV_SO 
    private float lastActionOnBeatTime; // into CV_SO

    private void Awake()
    {
        RemainingAmmo = maxRemainingAmmo; // into CV_SO
        CurrentAmmo = maxCurrentAmmo; // into CV_SO
        Cursor.lockState = CursorLockMode.Locked;
        IsOnBeat = false; // into CV_SO
    }

    private void OnEnable()
    {
        OnSessionStart_AddAllListeners();
    }
    private void OnSessionStart_AddAllListeners()
    {
        Jump.action.started += ProcessJumpInput;

        Shoot.action.started += ProcessShootInput;

        Aim.action.started += ProcessAimInput;
        Aim.action.canceled += ProcessAimInput;

        Dash.action.started += ProcessDashInput;

        SlowMotion.action.started += ProcessSlowMotionInput;
        SlowMotion.action.canceled += ProcessSlowMotionInput;

        Reload.action.started += ProcessReloadInput;

        Esc.action.started += ProcessEscInput;
    }



    private void OnDisable()
    {
        OnSessionEnded_RemoveAllListeners();
    }
    public void OnSessionEnded_RemoveAllListeners()
    {
        Jump.action.started -= ProcessJumpInput;

        Shoot.action.started -= ProcessShootInput;

        Aim.action.started -= ProcessAimInput;
        Aim.action.canceled -= ProcessAimInput;

        Dash.action.started -= ProcessDashInput;

        SlowMotion.action.started -= ProcessSlowMotionInput;
        SlowMotion.action.canceled -= ProcessSlowMotionInput;

        Reload.action.started -= ProcessReloadInput;

        Esc.action.started -= ProcessEscInput;
    }

    private void Start()
    {
        lastActionOnBeatTime = (((1 / beat.bpm) * 60) - beat.beatOffsetMultiplier); // into CV_SO
    }

    private void Update()
    {
        GetMoveDirection();
        IsOnBeat = beat.Return_IsOnBeat(); // into CV_SO
    }

    #region Process Input Methods
    private void ProcessShootInput(InputAction.CallbackContext ctx)
    {
        if (values.AllowInput_Bool == true)
        {
            if (canShoot == true && CurrentAmmo > 0) // into CV_SO
            {
                AudioHandler.Instance.PlaySound_sourceShooting(AudioHandler.Instance.playerShoot);
                CurrentAmmo--; // into CV_SO
                StartCoroutine(ShootTimer());
                onShootInvoked_started.Invoke();
            }

            else if (isReloading == false && CurrentAmmo == 0 && CurrentAmmo < maxCurrentAmmo && RemainingAmmo > 0)
            {
                AudioHandler.Instance.PlaySound_sourceShooting(AudioHandler.Instance.noAmmoClick);
                ProcessReloadInput(ctx);
            }
            else
                AudioHandler.Instance.PlaySound_sourceShooting(AudioHandler.Instance.noAmmoClick);
        }
    }
    private void ProcessReloadInput(InputAction.CallbackContext ctx)
    {
        if (values.AllowInput_Bool == true && isReloading == false && CurrentAmmo < maxCurrentAmmo && RemainingAmmo > 0)
        {
            isReloading = true;
            onReload_started.Invoke();
            if (IsOnBeat == true)
            {
                values.OnBeatActions++;
                values.CurrentCombo_Value = values.CurrentCombo_Value + 1;
                values.LastActionOnBeat_Bool = true;
            }
        }
    }
    private void ProcessJumpInput(InputAction.CallbackContext ctx)
    {
        if (values.AllowInput_Bool == true && ctx.started)
            onJumpInvoked_started.Invoke();
    }
    private void ProcessAimInput(InputAction.CallbackContext ctx)
    {
        if (values.AllowInput_Bool == true && ctx.started == true)
            onAimInvoked_started.Invoke();

        else
            onAimInvoked_canceled.Invoke();
    }
    private void ProcessSlowMotionInput(InputAction.CallbackContext ctx)
    {
        if (values.AllowInput_Bool == true && ctx.started == true)
        {
            onSlowMotion_started.Invoke();
        }
        else
            onSlowMotion_canceled.Invoke();
    }
    private void ProcessDashInput(InputAction.CallbackContext ctx)
    {
        if (values.AllowInput_Bool == true && ctx.started == true)
            onDashInvoked_started.Invoke();
        }
    private void ProcessEscInput(InputAction.CallbackContext context)
    {
        onEsc_started.Invoke();
    }
    #endregion
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
        yield return new WaitForEndOfFrame();
    } //TODO: Check if necessary
    private IEnumerator ShootTimer()
    {
        canShoot = false;
        yield return new WaitForSeconds(shootingCooldownTimer);
        canShoot = true;
    }
}


