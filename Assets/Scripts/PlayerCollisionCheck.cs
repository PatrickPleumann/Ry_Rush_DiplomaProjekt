using UnityEngine;

public class PlayerCollisionCheck : MonoBehaviour
{

    [Header("Ground Check")]
    [SerializeField] public bool IsGrounded = true;
    [SerializeField] private LayerMask groundCheckLayers;
    [SerializeField] private Transform groundCheckRoot;
    [SerializeField] private Vector3 groundCheckPos;
    [SerializeField] private Vector3 groundCheckSize;


    [Header("Wallruning Detection")]
    [SerializeField] private LayerMask wallCheckLayers;
    [SerializeField] private Transform wallCheckRoot;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private float minJumpHeight;
    private RaycastHit hitLeftWall;
    private RaycastHit hitRightWall;
    private bool onLeftWall;
    private bool onRightWall;


    [Header("Slope Handling")]
    [SerializeField] public RaycastHit slopeHit;
    [SerializeField] private float maxSlopeAngle;
    [SerializeField] private float slopeRayLength = 0.1f;
    [SerializeField] private float wallCheckRadius;

    private void FixedUpdate()
    {
        IsGrounded = Physics.OverlapBox(groundCheckRoot.position, groundCheckSize / 2, Quaternion.identity, groundCheckLayers).Length > 0;
        Debug.Log(IsGrounded);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(groundCheckPos, groundCheckSize);
    }

    public bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, slopeRayLength))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && (angle != 0);
        }
        return false;
    }
}
