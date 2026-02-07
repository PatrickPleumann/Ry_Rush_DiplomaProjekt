using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class MoveWeaponLerped : MonoBehaviour
{
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float lerpIntensityRot;
    [SerializeField] private float lerpIntensityPos;
    public bool isAiming;
    private void LateUpdate()
    {
        if (isAiming == false) // need to get this from a onValueChanged Event from a SO
        {
            gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, playerCamera.rotation, Time.deltaTime * lerpIntensityRot);
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, orientation.position, Time.deltaTime * lerpIntensityPos);
        }
    }
}
