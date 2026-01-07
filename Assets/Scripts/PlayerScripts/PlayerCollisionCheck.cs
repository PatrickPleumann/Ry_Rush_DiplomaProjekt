using UnityEngine;

public class PlayerCollisionCheck : MonoBehaviour
{
    [Header("Ground Check")]
    [SerializeField] public bool IsGrounded { get; private set; }
    [SerializeField] private LayerMask groundCheckLayers;
    [SerializeField] private Transform groundCheckRoot;
    [SerializeField] private Vector3 groundCheckPos;
    [SerializeField] private Vector3 groundCheckSize;


    [Header("Wallruning Detection")]
    [SerializeField] public bool AboveGround { get; private set; }
    [SerializeField] private LayerMask wallCheckLayers;
    [SerializeField] private Transform orientation;
    [SerializeField] public Transform wallCheckRoot;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private float minJumpHeight;

    public bool OnLeftWall { get; private set; }
    public bool OnRightWall { get; private set; }
    public RaycastHit hitWallLeft;
    public RaycastHit hitWallRight;


    [Header("Slope Handling")]
    [SerializeField] public RaycastHit SlopeHit;
    [SerializeField] private float maxSlopeAngle;
    [SerializeField] private float slopeRayLength = 0.1f;
    [SerializeField] private float wallCheckRadius;
    private float currentAngle = 0f;

    private void FixedUpdate()
    {
        IsGrounded = Physics.OverlapBox(groundCheckRoot.position, groundCheckSize / 2, Quaternion.identity, groundCheckLayers).Length > 0;
        AboveGround = AboveGroundForWallRun();

        CheckForWall();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawCube(groundCheckRoot.position, groundCheckSize);
        Gizmos.DrawRay(wallCheckRoot.position, orientation.right * wallCheckDistance);
        Gizmos.DrawRay(wallCheckRoot.position, -orientation.right * wallCheckDistance);
        Gizmos.DrawRay(transform.position, Vector3.down * minJumpHeight);
    }

    public bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out SlopeHit, slopeRayLength))
        {
            currentAngle = Vector3.Angle(Vector3.up, SlopeHit.normal);
            return currentAngle < maxSlopeAngle && (currentAngle != 0);
        }
        return false;
    }

    private void CheckForWall()
    {
        OnRightWall = Physics.Raycast(wallCheckRoot.position, orientation.right, out hitWallRight, wallCheckDistance, wallCheckLayers);
        OnLeftWall = Physics.Raycast(wallCheckRoot.position, -orientation.right, out hitWallLeft, wallCheckDistance, wallCheckLayers);
    }

    private bool AboveGroundForWallRun()
    {
        return !Physics.Raycast(groundCheckRoot.position, Vector3.down, minJumpHeight, groundCheckLayers);
    }


    public bool AllowWallRun()
    {
        if ((OnLeftWall || OnRightWall) && AboveGroundForWallRun() == true)
        {
            return true;
        }
        return false;
    }
}
