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
        controller.onSlowMotion_started.AddListener(StartSlowMotion);
        controller.onSlowMotion_canceled.AddListener(StopSlowMotion);

        values.DisAllowSlowMotion.AddListener(StopSlowMotion);

        if (data.SlowMotionOnAim == true)
        {
            controller.onAimInvoked_started.AddListener(StartSlowMotion);
            controller.onAimInvoked_canceled.AddListener(StopSlowMotion);
        }
    }

    private void OnDisable()
    {
        controller.onSlowMotion_started.RemoveListener(StartSlowMotion);
        controller.onSlowMotion_canceled.RemoveListener(StopSlowMotion);

        values.DisAllowSlowMotion.RemoveListener(StopSlowMotion);

        if (data.SlowMotionOnAim == true)
        {
            controller.onAimInvoked_started.RemoveListener(StartSlowMotion);
            controller.onAimInvoked_canceled.RemoveListener(StopSlowMotion);
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
