using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class UI_CameraShake : MonoBehaviour
{
    [SerializeField] private PlayerController controller;
    [SerializeField] private float lerpTime;
    [SerializeField] private float shakeIntensity;

    private Vector3 currentPosition;
    private Vector3 initialPosition;

    private void Awake()
    {
        initialPosition = transform.localPosition;
    }

    private void Update()
    {
        ResetCamera();
    }
    private void OnEnable()
    {
        controller.onShootInvoked_started.AddListener(ShakeCamera);
    }

    private void OnDisable()
    {
        controller.onShootInvoked_started.RemoveListener(ShakeCamera);
    }

    private void ShakeCamera(InputAction.CallbackContext ctx)
    {
        transform.localPosition = transform.localPosition + Random.insideUnitSphere * shakeIntensity;
        currentPosition = transform.localPosition;
    }

    private void ResetCamera()
    {
        transform.localPosition = Vector3.Lerp(currentPosition, initialPosition, lerpTime);
    }
}
