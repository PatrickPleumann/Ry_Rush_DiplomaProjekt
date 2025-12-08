using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerController controller;
    [SerializeField] private PlayerCollisionCheck collisionCheck;
    [SerializeField] private Transform playerTransform;

    // Components
    private Rigidbody rb_body;

    [Header("Gravity Values")]
    [SerializeField] private float gravityScale = 1f;
    [SerializeField] private float globalGravityForce = 9.81f;
    private Vector3 gravityForce;

    // for internal math
    private Vector3 moveDir;  //desired Direction
    private Vector3 lookDir;  //desired Rotation
    private Vector3 currentVelocity;
    private Vector3 targetVelocity;
    private Vector3 playerAcceleration;

    [Header("Move & Dash Speed Values")]
    [SerializeField] private float airAccel = 2f;
    [SerializeField] private float groundAccel = 5f;
    [SerializeField] private float maxSpeed = 8f;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float jumpForce = 10;
    private float tempAcc = 0f;
    private Vector2 clampVector;

    private void Start()
    {
        rb_body = GetComponent<Rigidbody>();
        gravityForce = gravityScale * globalGravityForce * Vector3.down;
    }

    private void FixedUpdate()
    {
        PlayerMove();

        Debug.Log(moveDir);

        PlayerGravity();
    }

    public void SetDesiredRotation(/*decoupled params with goal rotation, still need to figure out how and which*/)
    {
        //only sets the desired rotation, everything else happens automaticly maybe in Fixed or LateUpdate
    }
    public Vector3 SetDesiredDirection(/*InputAction.CallbackContext context*/)
    {
        moveDir.x = controller.move.action.ReadValue<Vector2>().x;
        moveDir.y = 0f;
        moveDir.z = controller.move.action.ReadValue<Vector2>().y;
        return moveDir;
    }

    private void PlayerMove()
    {
        tempAcc = collisionCheck.isGrounded ? groundAccel : airAccel;

        moveDir = SetDesiredDirection();

        currentVelocity.x = rb_body.linearVelocity.x;
        currentVelocity.z = rb_body.linearVelocity.z;
        currentVelocity.y = 0f;

        targetVelocity = new Vector3(moveDir.x, 0f, moveDir.z);

        if (moveDir == Vector3.zero)
            targetVelocity = currentVelocity * -tempAcc;
        else
            targetVelocity *= tempAcc;

        targetVelocity = playerTransform.TransformDirection(targetVelocity);

        rb_body.AddForce(targetVelocity -= currentVelocity, ForceMode.Acceleration);

        Debug.Log(rb_body.linearVelocity);
    }

    private void PlayerGravity() // custom gravity for gravity Scale manipulation
    {
        rb_body.AddForce(gravityForce, ForceMode.Acceleration);
    }

    public void PlayerLook()
    {

    }

    public void PlayerJump(InputAction.CallbackContext context)
    {
        rb_body.AddForce(0f, rb_body.mass * jumpForce, 0f, ForceMode.Impulse);
    }
    public void PlayerDash()
    {

    }

    public void PlayerWallJump()
    {

    }
    public void PlayerShoot()
    {

    }
}
