using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDash : MonoBehaviour
{
    [SerializeField] private PlayerCollisionCheck collisionCheck;
    [SerializeField] private PlayerController controller;
    [SerializeField] private CentralizedValues values;

    [SerializeField] private float dashForce;
    [SerializeField] private float dragValueWhileDashing;
    [SerializeField] private float disallowMovementForSeconds;
    [SerializeField] private float dashCooldown;

    private bool canDash = true;
    private Rigidbody rb_player;
    private Vector3 dashForceVector = Vector3.zero;


    private void OnEnable()
    {
        controller.onDashInvoked_started.AddListener(Dash);
    }

    private void OnDisable()
    {
        controller.onDashInvoked_started.RemoveListener(Dash);
    }

    private void Start()
    {
        rb_player = GetComponent<Rigidbody>();
    }
    public void Dash()
    {
        if (canDash == true)
        {
            if (controller.IsOnBeat == true)
            {
                values.OnBeatActions++;
                values.CurrentCombo_Value = values.CurrentCombo_Value + 1;
                values.LastActionOnBeat_Bool = true;
            }

            controller.AllowMovement = false;
            canDash = false;
            rb_player.linearDamping = 0f;

            StartCoroutine(ResetDash(disallowMovementForSeconds,dashCooldown));

            dashForceVector.x = controller.moveDirection.x * dashForce;
            dashForceVector.y = 0f;
            dashForceVector.z = controller.moveDirection.z * dashForce;

            rb_player.AddForce(dashForceVector, ForceMode.Impulse);
        }
    }

    private IEnumerator ResetDash(float _allowMovementTimer, float _dashCooldown)
    {
        //this WaitForSeconds stack up and need to be subtracted
        yield return new WaitForSeconds(_allowMovementTimer);
        controller.AllowMovement = true;
        yield return new WaitForSeconds(_dashCooldown - _allowMovementTimer);
        canDash = true;
        collisionCheck.exitingSlope = false;

        yield break;
    }
}
