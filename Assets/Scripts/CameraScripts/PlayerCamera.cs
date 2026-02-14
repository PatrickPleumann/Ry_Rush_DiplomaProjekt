using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerController controller;
    [SerializeField] private Transform playerOrientation;
    [SerializeField] private CentralizedValues values;

    [Header("Camera Values")]
    [SerializeField] public float sensitivityX;
    [SerializeField] public float sensitivityY;

    [SerializeField] private float smoothingTime = 20f; //TODO: if player option, this value HAS to be clamped between aproximatly 10 - 30 depends on framerate
    [SerializeField] private float cameraClampUp = 89f;
    [SerializeField] private float cameraClampDown = -85f;

    private float xRotation;
    private float yRotation;

    private float xSmoothRot;
    private float ySmoothRot;

    private Vector2 mousePos;

    private void Start()
    {
        ySmoothRot = mousePos.x;
        xSmoothRot = mousePos.y;// needs to get at least one value otherwise lerped values can stuck the camera
    }
    private void LateUpdate()
    {
        UpdateCamera();
        UpdateRotation();
    }

    private void UpdateCamera()
    {
        mousePos = controller.Look.action.ReadValue<Vector2>(); // needs to stay as it this, does not work with onValueChanged

        mousePos.x *= sensitivityX;
        mousePos.y *= sensitivityY;

        yRotation += mousePos.x;
        xRotation -= mousePos.y;

        xRotation = Mathf.Clamp(xRotation, cameraClampDown, cameraClampUp);

        xSmoothRot = Mathf.Lerp(xSmoothRot, xRotation, smoothingTime * Time.deltaTime);
        ySmoothRot = Mathf.Lerp(ySmoothRot, yRotation, smoothingTime * Time.deltaTime);
    }

    private void UpdateRotation() // smoothed
    {
        transform.rotation = Quaternion.Euler(xSmoothRot, ySmoothRot, 0f);
        playerOrientation.rotation = Quaternion.Euler(0f, ySmoothRot, 0f);
    }
}
