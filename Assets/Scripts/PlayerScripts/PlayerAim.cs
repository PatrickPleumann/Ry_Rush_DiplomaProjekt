using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAim : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform weaponHold;
    [SerializeField] private Transform weaponAim;
    [SerializeField] private Camera playerCam;
    [SerializeField] private GameObject playerWeapon;
    [SerializeField] private PlayerCamera cam;
    [Space]
    [Header("Zoom In & Out Values")]
    [SerializeField] private float zoomedOutFOV;
    [SerializeField] private float zoomedInFOV;
    [SerializeField] private float smoothedZoomTime;
    [Space]
    [Header("Reduced Values while Aiming")]
    [SerializeField] private float sensitivity_X_DividedBy = 2; // never keep this value empty
    [SerializeField] private float sensitivity_Y_DividedBy = 2; // never keep this value empty
    [Space]
    public bool isZoomedIn = false;

    private float tempSensX = 0f;
    private float tempSensY = 0f;

    //
    private float X_Sens_Multiplier;
    private float Y_Sens_Multiplier;

    private void Awake()
    {
        playerCam.fieldOfView = zoomedOutFOV;

        if (sensitivity_X_DividedBy < 1)
            sensitivity_X_DividedBy = 1;

        if (sensitivity_Y_DividedBy < 1)
            sensitivity_Y_DividedBy = 1;

        X_Sens_Multiplier = 1 / sensitivity_X_DividedBy;
        Y_Sens_Multiplier = 1 / sensitivity_Y_DividedBy;
    }

    private void LateUpdate()
    {
        if (isZoomedIn == true)
            ZoomIn();

        else
            ZoomOut();
    }

    private void ZoomIn()
    {
        playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, zoomedInFOV, smoothedZoomTime * Time.deltaTime);
        playerWeapon.transform.position = Vector3.Lerp(playerWeapon.transform.position, weaponAim.position, smoothedZoomTime * Time.deltaTime);
        playerWeapon.transform.rotation = Quaternion.Lerp(playerWeapon.transform.rotation, weaponAim.rotation, smoothedZoomTime * Time.deltaTime);
    }

    private void ZoomOut()
    {
        playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, zoomedOutFOV, smoothedZoomTime * Time.deltaTime);
        playerWeapon.transform.position = Vector3.Lerp(playerWeapon.transform.position, weaponHold.position, smoothedZoomTime * Time.deltaTime);
        playerWeapon.transform.rotation = Quaternion.Lerp(playerWeapon.transform.rotation, weaponHold.rotation, smoothedZoomTime * Time.deltaTime);
    }
    public void Zoom_True(InputAction.CallbackContext ctx)
    {
        if (isZoomedIn == false)
            isZoomedIn = true;
    }

    public void Zoom_False(InputAction.CallbackContext ctx)
    {
        if (isZoomedIn == true)
            isZoomedIn = false;
    }

    public void ReduceMouseSensitivity(InputAction.CallbackContext ctx)
    {
        tempSensX = cam.sensitivityX;
        tempSensY = cam.sensitivityY;
        cam.sensitivityX = cam.sensitivityX * X_Sens_Multiplier;
        cam.sensitivityY = cam.sensitivityY * Y_Sens_Multiplier;
    }
    public void ReIncreaseReducedMouseSensitivity(InputAction.CallbackContext ctx)
    {
        if (tempSensX != 0 && tempSensY != 0)
        {
            cam.sensitivityX = tempSensX;
            cam.sensitivityY = tempSensY;
        }
    }
}
