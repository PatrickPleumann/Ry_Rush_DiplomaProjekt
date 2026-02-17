using System.Collections;
using System.Diagnostics.Eventing.Reader;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement_New : MonoBehaviour
{

    [Header("General")]
    [SerializeField] private PlayerController controller;
    [SerializeField] private PlayerCollisionCheck collisionCheck;
    [SerializeField] private CentralizedValues values;

    private Rigidbody rb_player;

    private Vector3 speedControl;
    private Vector3 speedControlLimit;
    private float currentMaxMoveSpeed;
    private Vector3 additionalGravityVector;


    [Header("Ground Movement")]
    [SerializeField] private float groundMoveSpeed = 4f;
    [SerializeField] private float acceleration = 70f;
    [SerializeField] private float groundDragValue = 5f;

    [Header("Jump & Air Movement")]
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float jumpCooldown = 0.25f;
    [SerializeField] private float airMultiplier = 0.4f;
    [SerializeField] private float additionalGravityScaleMultiplier = 1;

    [Header("Wallrunning & Walljumping")]
    [SerializeField] private float wallJumpUpForce;
    [SerializeField] private float wallJumpSideForce;
    [SerializeField] private float pushToWallForce;
    [SerializeField] private float wallRunForce;
    [SerializeField] private float maxWallRunSpeed;
    [SerializeField] private float maxWallRunTime;
    [SerializeField] public bool Wallrunning;


    private float wallRunTimer;
    private bool allowWallJump;
    private Vector3 wallJumpForceApplied;

    [Header("Exiting Wall")]
    [SerializeField] private float exitWallTime;


    [Header("Movement States")]
    public MovementState State;
    [SerializeField] private float stateTransitionTimerValue = 0.1f;

    public enum MovementState
    {
        GroundMoving,
        WallRunning,
        WallJumping,
        OnSlope,
        Air
    }

    private void Awake()
    {
        rb_player = GetComponent<Rigidbody>();
        rb_player.freezeRotation = true;
        speedControl = new Vector3(0f, 0f, 0f); // y value won´t get touched anymore
    }

    private void OnEnable()
    {
        controller.onJumpInvoked_started.AddListener(Jump);
        controller.onJumpInvoked_started.AddListener(WallJump);
        controller.onMoveInvoked.AddListener(MovePlayer_Sound);
    }

    private void OnDisable()
    {
        controller.onJumpInvoked_started.RemoveListener(Jump);
        controller.onJumpInvoked_started.RemoveListener(WallJump);
        controller.onMoveInvoked.RemoveListener(MovePlayer_Sound);
    }

    private void Update()
    {
        StateHandler();

        StateMachine();
    }

    private void FixedUpdate()
    {
        if (values.AllowInput_Bool == true)
        {
            if (Wallrunning && controller.AllowMovement == true)
                WallRunning();

            else if (controller.AllowMovement == true)
                MovePlayer();

        }
        if (controller.AllowMovement == true)
            PlayerSpeedControl();

        ApplyGroundDrag(collisionCheck.IsGrounded);

        if (collisionCheck.IsGrounded == false) 
            ApplyAdditionalGravity();
    }

    private void ApplyAdditionalGravity()
    {
        additionalGravityVector.x = 0f;
        additionalGravityVector.y = additionalGravityScaleMultiplier * -9.81f;
        additionalGravityVector.z = 0f;
        rb_player.AddForce(additionalGravityVector, ForceMode.Acceleration);
    }

    private void MovePlayer()
    {
        if (collisionCheck.exitingSlope == false && collisionCheck.OnSlope() == true)
        {
            rb_player.AddForce(GetSlopeMoveDirection() * acceleration, ForceMode.Force);
            if (rb_player.linearVelocity.y > 0)
                rb_player.AddForce(Vector3.down * acceleration);
        }

        else if (collisionCheck.IsGrounded == true)
            rb_player.AddForce(controller.moveDirection.normalized * acceleration, ForceMode.Force);

        else if (collisionCheck.IsGrounded == false)
            rb_player.AddForce(controller.moveDirection.normalized * acceleration * airMultiplier, ForceMode.Force);

        rb_player.useGravity = !collisionCheck.OnSlope();
    }

    private void MovePlayer_Sound(InputAction.CallbackContext ctx)
    {
        if (ctx.started == true && collisionCheck.IsGrounded)
            AudioHandler.Instance.PlaySound_sourcePlayerMovement(AudioHandler.Instance.playerWalk_Long);

        else if (ctx.canceled == true)
            AudioHandler.Instance.PlaySound_sourcePlayerMovement(null);
    }

    private void PlayerSpeedControl()
    {
        if (collisionCheck.exitingSlope == false && collisionCheck.OnSlope() == true)
        {
            if (rb_player.linearVelocity.sqrMagnitude > (currentMaxMoveSpeed * currentMaxMoveSpeed))
                rb_player.linearVelocity = rb_player.linearVelocity.normalized * currentMaxMoveSpeed;
        }

        else
        {
            speedControl.x = rb_player.linearVelocity.x;
            speedControl.z = rb_player.linearVelocity.z;

            speedControlLimit = speedControl.normalized * currentMaxMoveSpeed;

            if (speedControl.sqrMagnitude > (currentMaxMoveSpeed * currentMaxMoveSpeed))
            {
                speedControlLimit.y = rb_player.linearVelocity.y;
                rb_player.linearVelocity = speedControlLimit;
            }

            speedControlLimit.y = 0f;
        }
    }

    private void ApplyGroundDrag(bool _isGrounded)
    {
        rb_player.linearDamping = _isGrounded ? groundDragValue : 0;
    }

    public void Jump()
    {

        if (collisionCheck.canJump == true && collisionCheck.IsGrounded == true)
        {
            AudioHandler.Instance.PlaySound_sourcePlayerMovement(
                AudioHandler.Instance.playerJump[Random.Range(0,AudioHandler.Instance.playerJump.Length)]);

            //if on beat.... maybe decrease if NOT on beat
            if (controller.IsOnBeat == true)
            {
                values.OnBeatActions++;
                values.CurrentCombo_Value = values.CurrentCombo_Value + 1;
                values.LastActionOnBeat_Bool = true;
            }
            collisionCheck.canJump = false;
            collisionCheck.exitingSlope = true;
            SwitchState(MovementState.Air);
            rb_player.linearVelocity = new Vector3(rb_player.linearVelocity.x, 0f, rb_player.linearVelocity.z);
            rb_player.AddForce(rb_player.transform.up * jumpForce, ForceMode.Impulse);
            StartCoroutine(ResetJump(jumpCooldown));
        }

        else if (collisionCheck.canJump == true && collisionCheck.IsGrounded == false && collisionCheck.canDoubleJump == true)
        {
            AudioHandler.Instance.PlaySound_sourcePlayerMovement(
                AudioHandler.Instance.playerJump[Random.Range(0, AudioHandler.Instance.playerJump.Length)]);

            //if on beat.... maybe decrease if NOT on beat
            if (controller.IsOnBeat == true)
            {
                values.OnBeatActions++;
                values.CurrentCombo_Value = values.CurrentCombo_Value + 1;
                values.LastActionOnBeat_Bool = true;
            }
            collisionCheck.canDoubleJump = false;
            rb_player.linearVelocity = new Vector3(rb_player.linearVelocity.x, 0f, rb_player.linearVelocity.z);
            rb_player.AddForce(rb_player.transform.up * jumpForce, ForceMode.Impulse);
            StartCoroutine(ResetJump(jumpCooldown));
        }
    }

    private IEnumerator ResetJump(float _jumpCooldown)
    {
        yield return new WaitForSeconds(_jumpCooldown);
        collisionCheck.canJump = true;
        collisionCheck.exitingSlope = false;
    }

    private IEnumerator StateTransitionTimer(float _transitionTimer)
    {
        yield return new WaitForSeconds(_transitionTimer);
    }

    private void SwitchState(MovementState _state)
    {
        State = _state;
    }

    private void StateHandler()
    {
        if (Wallrunning)
        {
            SwitchState(MovementState.WallRunning);
            currentMaxMoveSpeed = maxWallRunSpeed;
        }

        else if (collisionCheck.IsGrounded && !collisionCheck.OnSlope())
        {
            SwitchState(MovementState.GroundMoving);
            currentMaxMoveSpeed = groundMoveSpeed;
        }

        else if (collisionCheck.OnSlope())
            SwitchState(MovementState.OnSlope);

        else if (!collisionCheck.IsGrounded && !collisionCheck.OnSlope())
            SwitchState(MovementState.Air);
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(controller.moveDirection, collisionCheck.SlopeHit.normal).normalized;
    }

    private void StateMachine()
    {
        if (collisionCheck.AllowWallRun() == true && controller.moveInput.z > 0 && !collisionCheck.exitingWall)
        {
            if (!Wallrunning)
            {
                StartWallRun();
                collisionCheck.canDoubleJump = false;
                allowWallJump = true;
            }
        }
        else if (collisionCheck.exitingWall)
        {
            if (Wallrunning)
                StopWallRun();
        }

        else
        {
            if (Wallrunning)
            {
                StopWallRun();
                allowWallJump = false;
            }
        }
    }

    private void StartWallRun()
    {
        AudioHandler.Instance.PlaySound_sourcePlayerMovement(AudioHandler.Instance.playerWallrun);
        Wallrunning = true;
    }

    private void WallRunning()
    {
        rb_player.useGravity = false;
        rb_player.linearVelocity = new Vector3(rb_player.linearVelocity.x, 0f, rb_player.linearVelocity.z);

        //TODO: figure out how to get rid of new Vector3 step below
        Vector3 wallNormal = collisionCheck.OnRightWall ? collisionCheck.hitWallRight.normal : collisionCheck.hitWallLeft.normal;

        Vector3 wallForward = Vector3.Cross(wallNormal, collisionCheck.WallCheckRoot.up);

        if ((controller.Orientation.forward - wallForward).sqrMagnitude > (controller.Orientation.forward - -wallForward).sqrMagnitude)
            wallForward = -wallForward;

        if (Wallrunning)
            rb_player.AddForce(wallForward * wallRunForce, ForceMode.Force);

        if (!(collisionCheck.OnLeftWall && controller.moveInput.x > 0) && !(collisionCheck.OnRightWall && controller.moveInput.x < 0))
            rb_player.AddForce(-wallNormal * pushToWallForce, ForceMode.Force);
    }

    private void StopWallRun()
    {
        AudioHandler.Instance.PlaySound_sourcePlayerMovement(null);
        collisionCheck.canDoubleJump = true;
        Wallrunning = false;
    }

    public void WallJump()
    {
        if (collisionCheck.exitingWall == false && Wallrunning && collisionCheck.canJump == true)
        {
            AudioHandler.Instance.PlaySound_sourcePlayerMovement(
                AudioHandler.Instance.playerJump[Random.Range(0, AudioHandler.Instance.playerJump.Length)]);

            if (controller.IsOnBeat == true)
            {
                values.OnBeatActions++;
                values.CurrentCombo_Value = values.CurrentCombo_Value + 1;
                values.LastActionOnBeat_Bool = true;
            }
            collisionCheck.exitingWall = true;
            StartCoroutine(ExitWallTimer());
            SwitchState(MovementState.WallJumping);
            Vector3 wallNormal = collisionCheck.OnRightWall ? collisionCheck.hitWallRight.normal : collisionCheck.hitWallLeft.normal;

            wallJumpForceApplied = rb_player.transform.up * wallJumpUpForce + wallNormal * wallJumpSideForce;

            rb_player.linearVelocity = new Vector3(rb_player.linearVelocity.x, 0f, rb_player.linearVelocity.z);
            rb_player.AddForce(wallJumpForceApplied, ForceMode.Impulse);
        }
    }


    private IEnumerator ExitWallTimer() // takes a float and a bool and set´s boo
    {
        yield return new WaitForSeconds(exitWallTime);
        collisionCheck.exitingWall = false;
        yield break;
    }
}
