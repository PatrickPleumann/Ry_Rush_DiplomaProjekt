using UnityEngine;

public class MoveWeaponLerped : MonoBehaviour
{
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float lerpIntensityRot;
    [SerializeField] private float smoothDampSpeed; // lower value = faster damping

    [SerializeField] private Vector3 refVector2;

    public bool isAiming;
    private Vector3 refVector = Vector3.zero;

    private void LateUpdate()
    {
        gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, playerCamera.rotation, Time.deltaTime * lerpIntensityRot);
        gameObject.transform.position = orientation.position;
    }
}
