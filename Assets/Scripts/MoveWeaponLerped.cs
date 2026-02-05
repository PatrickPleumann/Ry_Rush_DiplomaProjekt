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
        if (isAiming == false)
        {
            gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, orientation.rotation, 0.5f + Time.deltaTime * lerpIntensityPos);
            gameObject.transform.position = Vector3.Slerp(gameObject.transform.position, playerCamera.position, 0.5f + Time.deltaTime * lerpIntensityPos);
        }
    }
}
