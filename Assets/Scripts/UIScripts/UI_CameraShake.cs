using UnityEngine;
using UnityEngine.InputSystem;

public class UI_CameraShake : MonoBehaviour
{
    [SerializeField] private PlayerController controller;
    [SerializeField] private float lerpTime;
    [SerializeField] private float shakeIntensity;

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
        controller.OnShootInvoked_started.AddListener(ShakeCamera);
    }

    private void OnDisable()
    {
        controller.OnShootInvoked_started.RemoveListener(ShakeCamera);
    }

    private void ShakeCamera()
    {
        var temp = Random.insideUnitSphere;
        transform.localPosition = transform.localPosition + new Vector3(0,temp.y,0f) * shakeIntensity;
    }

    private void ResetCamera()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, initialPosition, Time.deltaTime * lerpTime);
    }
}
