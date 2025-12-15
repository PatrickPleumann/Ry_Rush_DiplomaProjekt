using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private PlayerController controller;
    [SerializeField] private float sensitivityX;
    [SerializeField] private float sensitivityY;
    [SerializeField] private float smoothingTime = 20f;

    [SerializeField] private float cameraClampUp = 89f;
    [SerializeField] private float cameraClampDown = -89f;

    [SerializeField] private Transform playerOrientation;

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
    private void Update()
    {
        HandleMouseInput();
    }

    private void HandleMouseInput()
    {
        mousePos = controller.look.action.ReadValue<Vector2>();

        mousePos.x *= Time.deltaTime * sensitivityX;
        mousePos.y *= Time.deltaTime * sensitivityY;

        yRotation += mousePos.x;
        xRotation -= mousePos.y;

        xRotation = Mathf.Clamp(xRotation, cameraClampDown + 1, cameraClampUp - 1);

        xSmoothRot = Mathf.Lerp(xSmoothRot, xRotation, smoothingTime * Time.deltaTime);
        ySmoothRot = Mathf.Lerp(ySmoothRot, yRotation, smoothingTime * Time.deltaTime);

        transform.rotation = Quaternion.Euler(xSmoothRot, ySmoothRot, 0f);
        playerOrientation.rotation = Quaternion.Euler(0f, ySmoothRot, 0f);

    }
}
