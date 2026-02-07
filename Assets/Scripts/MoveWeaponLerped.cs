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
        if (isAiming == false) // need to get this from a onValueChanged Event from a SO
        {
            gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, playerCamera.rotation, Time.deltaTime * lerpIntensityRot);
            gameObject.transform.position = Vector3.SmoothDamp(gameObject.transform.position, orientation.position, ref refVector, smoothDampSpeed);
        }
        else
        {
            gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, playerCamera.rotation, Time.deltaTime * lerpIntensityRot);
            gameObject.transform.position = orientation.position;
        }
    }
}
