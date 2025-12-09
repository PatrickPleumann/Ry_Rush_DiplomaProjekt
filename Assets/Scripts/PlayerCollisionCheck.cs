using UnityEngine;

public class PlayerCollisionCheck : MonoBehaviour
{
    [SerializeField] private Transform groundCheckRoot;
    [SerializeField] private Transform wallCheckRoot;

    [SerializeField] private Vector3 groundCheckPos;
    [SerializeField] private Vector3 groundCheckSize;

    [SerializeField] private LayerMask groundCheckLayers;
    [SerializeField] private LayerMask wallCheckLayers;

    [SerializeField] private float wallCheckRadius;

    public bool IsGrounded = true;
    public bool TouchesWall;

    public Collider[] WallsTouched;

    private void FixedUpdate()
    {
        IsGrounded = Physics.OverlapBox(groundCheckRoot.position, groundCheckSize / 2, Quaternion.identity, groundCheckLayers).Length > 0;
        // hier noch Walls Toched in etwa dasselbe + TouchesWall als bool abfragen
    }
}
