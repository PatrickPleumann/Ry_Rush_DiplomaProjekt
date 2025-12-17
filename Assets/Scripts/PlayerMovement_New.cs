using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement_New : MonoBehaviour
{

    [Header("General")]
    [SerializeField] private PlayerController controller;
    [SerializeField] private PlayerCollisionCheck collisionCheck;
    [SerializeField] private Transform orientation;

    private Rigidbody rb_player;
    private Vector3 moveInput; // from readValue<Vector2>()
    private Vector3 moveDirection;
    private Vector3 speedControl;
    private Vector3 speedControlLimit;
    private Vector3 wallForward;
    private float currentMaxMoveSpeed;
    private bool exitingSlope;


    [Header("Ground Movement")]
    [SerializeField] private float groundMoveSpeed = 4f;
    [SerializeField] private float acceleration = 70f;
    [SerializeField] private float groundDragValue = 5f;

    [Header("Jump & Air Movement")]
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float jumpCooldown = 0.25f;
    [SerializeField] private float airMultiplier = 0.4f;
    private bool canJump = true;

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
    private bool exitingWall;
    [SerializeField] private float exitWallTime;


    [Header("Movement States")]
    public MovementState State;
    [SerializeField] private float stateTransitionTimerValue = 0.1f;
    private bool canSwitchState;
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
        canSwitchState = true;
    }

    private void Update()
    {
        GetMoveDirection();

        StateHandler();

        StateMachine();
    }

    private void FixedUpdate()
    {
        if (Wallrunning)
            WallRunning();
        else
            MovePlayer();

        PlayerSpeedControl();
        ApplyGroundDrag(collisionCheck.IsGrounded);

    }

    private void GetMoveDirection()
    {
        moveInput.x = controller.move.action.ReadValue<Vector2>().x;
        moveInput.z = controller.move.action.ReadValue<Vector2>().y;
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * moveInput.z + orientation.right * moveInput.x;

        if (exitingSlope == false && collisionCheck.OnSlope() == true)
        {
            rb_player.AddForce(GetSlopeMoveDirection() * acceleration, ForceMode.Force);
            if (rb_player.linearVelocity.y > 0)
                rb_player.AddForce(Vector3.down * acceleration);
        }

        else if (collisionCheck.IsGrounded == true)
            rb_player.AddForce(moveDirection.normalized * acceleration, ForceMode.Force);

        else if (collisionCheck.IsGrounded == false)
            rb_player.AddForce(moveDirection.normalized * acceleration * airMultiplier, ForceMode.Force);

        rb_player.useGravity = !collisionCheck.OnSlope();
    }

    private void PlayerSpeedControl()
    {
        if (exitingSlope == false && collisionCheck.OnSlope() == true)
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

    public void Jump(InputAction.CallbackContext context)
    {
        if (canJump && collisionCheck.IsGrounded)
        {
            canJump = false;
            exitingSlope = true;
            SwitchState(MovementState.Air);
            rb_player.linearVelocity = new Vector3(rb_player.linearVelocity.x, 0f, rb_player.linearVelocity.z);
            rb_player.AddForce(rb_player.transform.up * jumpForce, ForceMode.Impulse);

            StartCoroutine(ResetJump(jumpCooldown));
        }
    }

    private IEnumerator ResetJump(float _jumpCooldown)
    {
        yield return new WaitForSeconds(_jumpCooldown);
        canJump = true;
        exitingSlope = false;
    }

    private IEnumerator StateTransitionTimer(float _transitionTimer)
    {
        yield return new WaitForSeconds(_transitionTimer);
        canSwitchState = true;
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
        return Vector3.ProjectOnPlane(moveDirection, collisionCheck.SlopeHit.normal).normalized;
    }

    private void StateMachine()
    {
        if (collisionCheck.AllowWallRun() == true && moveInput.z > 0 && !exitingWall)
        {
            if (!Wallrunning)
            {
                StartWallRun();
                allowWallJump = true;
            }
        }
        else if (exitingWall)
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
        Wallrunning = true;
    }

    private void WallRunning()
    {
        rb_player.useGravity = false;
        rb_player.linearVelocity = new Vector3(rb_player.linearVelocity.x, 0f, rb_player.linearVelocity.z);

        //TODO: figure out how to get rid of new Vector3 step below
        Vector3 wallNormal = collisionCheck.OnRightWall ? collisionCheck.hitWallRight.normal : collisionCheck.hitWallLeft.normal;

        Vector3 wallForward = Vector3.Cross(wallNormal, collisionCheck.wallCheckRoot.up);

        if ((orientation.forward - wallForward).sqrMagnitude > (orientation.forward - -wallForward).sqrMagnitude)
            wallForward = -wallForward;

        if (Wallrunning)
            rb_player.AddForce(wallForward * wallRunForce, ForceMode.Force);

        if (!(collisionCheck.OnLeftWall && moveInput.x > 0) && !(collisionCheck.OnRightWall && moveInput.x < 0))
            rb_player.AddForce(-wallNormal * pushToWallForce, ForceMode.Force);
    }

    private void StopWallRun()
    {
        Wallrunning = false;
    }

    public void WallJump(InputAction.CallbackContext context)
    {
        if (exitingWall == false && Wallrunning)
        {
            exitingWall = true;
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
        exitingWall = false;
        yield break;
    }
}
