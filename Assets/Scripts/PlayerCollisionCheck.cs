using UnityEngine;

public class PlayerCollisionCheck : MonoBehaviour
{
    //checks for Ground and for Walls >> sets different bools for each
    public bool isGrounded;
    public bool touchesWall;
    public Collider[] wallsTouched;

    private void Start()
    {
        isGrounded = true;
    }
    private void FixedUpdate()
    {
        //is Grounded && touchesWall
    }
}
