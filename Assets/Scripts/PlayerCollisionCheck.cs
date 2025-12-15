using UnityEngine;

public class PlayerCollisionCheck : MonoBehaviour
{
    [SerializeField] private Transform groundCheckRoot;
    [SerializeField] private Transform wallCheckRoot;

    [SerializeField] private Vector3 groundCheckPos;
    [SerializeField] private Vector3 groundCheckSize;

    [SerializeField] private LayerMask groundCheckLayers;
    [SerializeField] private LayerMask wallCheckLayers;

    [Header("Slope Handling")]
    [SerializeField] private float maxSlopeAngle;
    [SerializeField] private float slopeRayLength = 0.1f;
    public RaycastHit slopeHit;

    [SerializeField] private float wallCheckRadius;

    public bool IsGrounded = true;
    public bool TouchesWall;
    public bool onSlope;

    public Collider[] WallsTouched;

    private void FixedUpdate()
    {
        IsGrounded = Physics.OverlapBox(groundCheckRoot.position, groundCheckSize / 2, Quaternion.identity, groundCheckLayers).Length > 0;
        // hier noch Walls Toched in etwa dasselbe + TouchesWall als bool abfragen
        Debug.Log(IsGrounded);
        onSlope = OnSlope();
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
