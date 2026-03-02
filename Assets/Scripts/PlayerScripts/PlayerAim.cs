using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerAim : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform weaponHold;
    [SerializeField] private Transform weaponAim;
    [SerializeField] private Camera playerCam;
    [SerializeField] private GameObject playerWeapon;
    [SerializeField] private PlayerCamera cam;
    [SerializeField] private PlayerController controller;
    [SerializeField] private CentralizedValues values;
    
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

    private void OnEnable()
    {
        controller.OnAimInvoked_started.AddListener(Zoom_True);
        controller.OnAimInvoked_started.AddListener(ReduceMouseSensitivity);

        controller.OnAimInvoked_canceled.AddListener(Zoom_False);
        controller.OnAimInvoked_canceled.AddListener(ReIncreaseReducedMouseSensitivity);
    }

    private void OnDisable()
    {
        controller.OnAimInvoked_started.RemoveListener(Zoom_True);
        controller.OnAimInvoked_started.RemoveListener(ReduceMouseSensitivity);

        controller.OnAimInvoked_canceled.RemoveListener(Zoom_False);
        controller.OnAimInvoked_canceled.RemoveListener(ReIncreaseReducedMouseSensitivity);
    }

    private void LateUpdate()
    {
        if (isZoomedIn == true && playerCam.fieldOfView > zoomedInFOV)
            ZoomIn();

        else if (playerCam.fieldOfView < zoomedOutFOV)
            ZoomOut();
    }

    private void ZoomIn()
    {
        //if on beat.... maybe decrease if NOT on beat

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
    public void Zoom_True()
    {
        if (controller.IsOnBeat == true)
        {
            values.OnBeatActions++;
            values.LastActionOnBeat_Bool = true;
            values.CurrentCombo_Value = values.CurrentCombo_Value + 1;
        }
        if (isZoomedIn == false)
            isZoomedIn = true;
    }

    public void Zoom_False()
    {
        if (isZoomedIn == true)
            isZoomedIn = false;
    }

    public void ReduceMouseSensitivity()
    {
        tempSensX = cam.sensitivityX;
        tempSensY = cam.sensitivityY;
        cam.sensitivityX = cam.sensitivityX * X_Sens_Multiplier;
        cam.sensitivityY = cam.sensitivityY * Y_Sens_Multiplier;
    }
    public void ReIncreaseReducedMouseSensitivity()
    {
        if (tempSensX != 0 && tempSensY != 0)
        {
            cam.sensitivityX = tempSensX;
            cam.sensitivityY = tempSensY;
        }
    }
}
