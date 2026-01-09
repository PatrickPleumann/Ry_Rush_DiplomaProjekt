using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerController controller;
    [SerializeField] private Transform playerOrientation;

    [Header("Camera Values")]
    [SerializeField] public float sensitivityX;
    [SerializeField] public float sensitivityY;
    [SerializeField] private float smoothingTime = 20f; //TODO: if player option, this value HAS to be clamped between aproximatly 10 - 30 depends on framerate
                                                        //TODO: below 10 appears to be too slow and above 30 can cause issues while having very low frames
    [SerializeField] private float cameraClampUp = 89f;
    [SerializeField] private float cameraClampDown = -85f;


    private float xRotation;
    private float yRotation;

    private float xSmoothRot;
    private float ySmoothRot;

    private Vector2 mousePos;

    private void Start()
    {
        //Application.targetFrameRate = 100;
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
        mousePos = controller.look.action.ReadValue<Vector2>();

        mousePos.x *= sensitivityX;
        mousePos.y *= sensitivityY;

        //old
        yRotation += mousePos.x;
        xRotation -= mousePos.y;

        xRotation = Mathf.Clamp(xRotation, cameraClampDown, cameraClampUp);

        //old
        xSmoothRot = Mathf.Lerp(xSmoothRot, xRotation, smoothingTime * Time.deltaTime);
        ySmoothRot = Mathf.Lerp(ySmoothRot, yRotation, smoothingTime * Time.deltaTime);
    }

    private void UpdateRotation()
    {
        //old
        transform.rotation = Quaternion.Euler(xSmoothRot, ySmoothRot, 0f);
        playerOrientation.rotation = Quaternion.Euler(0f, ySmoothRot, 0f);
    }
}
