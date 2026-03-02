using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
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
    [HideInInspector] public UnityEvent<InputAction.CallbackContext> OnMoveInvoked;

    [HideInInspector] public UnityEvent OnShootInvoked_started;

    [HideInInspector] public UnityEvent OnAimInvoked_started;
    [HideInInspector] public UnityEvent OnAimInvoked_canceled;

    [HideInInspector] public UnityEvent OnJumpInvoked_started;

    [HideInInspector] public UnityEvent OnDashInvoked_started;

    [HideInInspector] public UnityEvent OnSlowMotion_started;
    [HideInInspector] public UnityEvent OnSlowMotion_canceled;

    [HideInInspector] public UnityEvent OnReload_started;
    [HideInInspector] public UnityEvent OnReload_Finished;

    [HideInInspector] public UnityEvent OnEsc_started;

    [Space]

    [Header("Centralized Values")]
    [SerializeField] public Transform Orientation;
    [SerializeField] private float shootingCooldownTimer;

    [HideInInspector] public Vector3 MoveInput; // into CV_SO
    [HideInInspector] public Vector3 MoveDirection;

    public bool AllowMovement;
    [HideInInspector] public bool IsOnBeat; // into CV_SO
    [HideInInspector] public bool LastActionOnBeat; // into CV_SO
    [HideInInspector] public bool IsReloading = false; // into CV_SO
    [HideInInspector] public bool CanShoot = true; // into CV_SO

    [Space]

    [SerializeField] public int MaxCurrentAmmo;
    [SerializeField] public int MaxRemainingAmmo;

    [Space]

    public int CurrentAmmo; // into CV_SO
    public int RemainingAmmo; // into CV_SO 
    private float lastActionOnBeatTime; // into CV_SO

    private void Awake()
    {
        RemainingAmmo = MaxRemainingAmmo; // into CV_SO
        CurrentAmmo = MaxCurrentAmmo; // into CV_SO
        Cursor.lockState = CursorLockMode.Locked;
        IsOnBeat = false; // into CV_SO
    }

    private void OnEnable()
    {
        OnSessionStart_AddAllListeners();
    }
    private void OnSessionStart_AddAllListeners()
    {
        Move.action.started += ProcessMoveInput;
        Move.action.canceled += ProcessMoveInput;

        Jump.action.started += ProcessJumpInput;

        Shoot.action.started += ProcessShootInput;
        Shoot.action.canceled += ResetShoot;

        Aim.action.started += ProcessAimInput;
        Aim.action.canceled += ProcessAimInput;

        Dash.action.started += ProcessDashInput;

        SlowMotion.action.started += ProcessSlowMotionInput;
        SlowMotion.action.canceled += ProcessSlowMotionInput;

        Reload.action.started += ProcessReloadInput;

        Esc.action.started += ProcessEscInput;
    }

    private void ProcessMoveInput(InputAction.CallbackContext context)
    {
        OnMoveInvoked.Invoke(context);
    }

    private void OnDisable()
    {
        OnSessionEnded_RemoveAllListeners();
    }
    public void OnSessionEnded_RemoveAllListeners()
    {
        Move.action.started -= ProcessMoveInput;
        Move.action.canceled -= ProcessMoveInput;

        Jump.action.started -= ProcessJumpInput;

        Shoot.action.started -= ProcessShootInput;
        Shoot.action.canceled -= ResetShoot;

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
        if (values.AllowInput == true)
        {
            if (CanShoot == true && CurrentAmmo > 0) // into CV_SO
            {
                AudioHandler.Instance.PlaySound_sourceShooting(AudioHandler.Instance.playerShoot);
                CurrentAmmo--; // into CV_SO
                OnShootInvoked_started.Invoke();
            }

            else if (IsReloading == false && CurrentAmmo == 0 && CurrentAmmo < MaxCurrentAmmo && RemainingAmmo > 0)
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
        if (values.AllowInput == true && IsReloading == false && CurrentAmmo < MaxCurrentAmmo && RemainingAmmo > 0)
        {
            IsReloading = true;
            OnReload_started.Invoke();
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
        if (values.AllowInput == true && ctx.started)
            OnJumpInvoked_started.Invoke();
    }
    private void ProcessAimInput(InputAction.CallbackContext ctx)
    {
        if (values.AllowInput == true && ctx.started == true)
            OnAimInvoked_started.Invoke();

        else
            OnAimInvoked_canceled.Invoke();
    }
    private void ProcessSlowMotionInput(InputAction.CallbackContext ctx)
    {
        if (values.AllowInput == true && ctx.started == true)
        {
            OnSlowMotion_started.Invoke();
        }
        else
            OnSlowMotion_canceled.Invoke();
    }
    private void ProcessDashInput(InputAction.CallbackContext ctx)
    {
        if (values.AllowInput == true && ctx.started == true)
            OnDashInvoked_started.Invoke();
    }
    private void ProcessEscInput(InputAction.CallbackContext context)
    {
        OnEsc_started.Invoke();
    }
    #endregion
    private void GetMoveDirection()
    {
        if (AllowMovement == true)
        {
            MoveInput.x = Move.action.ReadValue<Vector2>().x;
            MoveInput.y = 0f;
            MoveInput.z = Move.action.ReadValue<Vector2>().y;

            MoveDirection = Orientation.forward * MoveInput.z + Orientation.right * MoveInput.x;
        }
    }
    private void ResetShoot(InputAction.CallbackContext ctx)
    {
        if (ctx.canceled == true)
        {
            CanShoot = true;
        }
    }
}


