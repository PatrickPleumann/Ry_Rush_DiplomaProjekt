using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class SlowMotionHandler : MonoBehaviour
{
    [SerializeField] private CentralizedValues values;
    [SerializeField] private PlayerController controller;
    [SerializeField] private SettingsData data;

    [SerializeField] private float duration = 1;
    [SerializeField] private float slowMotionSoundPitchRate = 3f;

    [SerializeField] private float minTimeScale;
    [SerializeField] private float maxTimeScale;

    private Coroutine decreaseTimeScale;
    private Coroutine increaseTimeScale;
    private float currentPitch;

    private void OnEnable()
    {
        controller.OnSlowMotion_started.AddListener(StartSlowMotion);
        controller.OnSlowMotion_canceled.AddListener(StopSlowMotion);

        values.DisAllowSlowMotion.AddListener(StopSlowMotion);

        if (data.SlowMotionOnAim == true)
        {
            controller.OnAimInvoked_started.AddListener(StartSlowMotion);
            controller.OnAimInvoked_canceled.AddListener(StopSlowMotion);
        }
    }

    private void OnDisable()
    {
        controller.OnSlowMotion_started.RemoveListener(StartSlowMotion);
        controller.OnSlowMotion_canceled.RemoveListener(StopSlowMotion);

        values.DisAllowSlowMotion.RemoveListener(StopSlowMotion);

        if (data.SlowMotionOnAim == true)
        {
            controller.OnAimInvoked_started.RemoveListener(StartSlowMotion);
            controller.OnAimInvoked_canceled.RemoveListener(StopSlowMotion);
        }
    }

    private void StartSlowMotion()
    {
        if (increaseTimeScale != null)
        {
            StopCoroutine(increaseTimeScale);
        }
        increaseTimeScale = null;
        decreaseTimeScale = StartCoroutine(DecreaseTimeScale());
    }

    private void StopSlowMotion()
    {
        if (decreaseTimeScale != null)
            StopCoroutine(decreaseTimeScale);

        decreaseTimeScale = null;
        increaseTimeScale = StartCoroutine(ReIncreaseTimeScale());
    }

    private IEnumerator DecreaseTimeScale()
    {
        while (Time.timeScale > minTimeScale)
        {
            Time.timeScale = Mathf.Lerp(maxTimeScale, minTimeScale, duration);
            currentPitch = Mathf.Lerp(maxTimeScale, minTimeScale, duration) * slowMotionSoundPitchRate;
            values.OnSlowMotionPitchSource.Invoke(currentPitch);
        }
        yield return new WaitForEndOfFrame();
    }
    private IEnumerator ReIncreaseTimeScale()
    {
        while (Time.timeScale < maxTimeScale)
        {
            Time.timeScale = Mathf.Lerp(minTimeScale, maxTimeScale, duration);
            currentPitch = Mathf.Lerp(minTimeScale,maxTimeScale, duration);
            values.OnSlowMotionPitchSource.Invoke(currentPitch);
        }
        yield return new WaitForEndOfFrame();
    }
}
