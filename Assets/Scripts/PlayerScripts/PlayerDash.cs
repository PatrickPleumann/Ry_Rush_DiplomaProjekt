using System.Collections;
using System.Threading.Tasks;
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
            values.onDashExecuted.Invoke();

            AudioHandler.Instance.PlayOneShot(
                AudioHandler.Instance.playerDash[Random.Range(0, AudioHandler.Instance.playerDash.Length)]);

            if (controller.IsOnBeat == true)
            {
                values.OnBeatActions++;
                values.CurrentCombo_Value = values.CurrentCombo_Value + 1;
                values.LastActionOnBeat_Bool = true;
            }

            controller.AllowMovement = false;
            canDash = false;
            rb_player.linearDamping = 0f;

            ResetDash(disallowMovementForSeconds, dashCooldown);

            dashForceVector.x = controller.moveDirection.x * dashForce;
            dashForceVector.y = 0f;
            dashForceVector.z = controller.moveDirection.z * dashForce;

            rb_player.AddForce(dashForceVector, ForceMode.Impulse);
        }
    }


    private async void ResetDash(float _allowMovementTimer, float _dashCooldown)
    {
        await Task.Delay((int)(_allowMovementTimer * 1000));
        controller.AllowMovement = true;
        await Task.Delay((int)((_dashCooldown - _allowMovementTimer) * 1000));
        canDash = true;
        collisionCheck.exitingSlope = false;
    }
}
