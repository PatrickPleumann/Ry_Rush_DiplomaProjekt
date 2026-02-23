using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class SlowMotionHandler : MonoBehaviour
{
    [SerializeField] private PlayerController controller;

    [SerializeField] private float duration = 1;
    [SerializeField] private float slowMotionSoundPitchRate = 3f;

    [SerializeField] private float minTimeScale;
    [SerializeField] private float maxTimeScale;

    private Coroutine decreaseTimeScale;
    private Coroutine increaseTimeScale;

    private void OnEnable()
    {
        controller.onSlowMotion_started.AddListener(StartSlowMotion);
        controller.onSlowMotion_canceled.AddListener(StopSlowMotion);

        if (controller.SlowMotion_OnAim == true)
        {
            controller.onAimInvoked_started.AddListener(StartSlowMotion);
            controller.onAimInvoked_canceled.AddListener(StopSlowMotion);
        }
    }

    private void OnDisable()
    {
        controller.onSlowMotion_started.RemoveListener(StartSlowMotion);
        controller.onSlowMotion_canceled.RemoveListener(StopSlowMotion);

        if (controller.SlowMotion_OnAim == true)
        {
            controller.onAimInvoked_started.RemoveListener(StartSlowMotion);
            controller.onAimInvoked_canceled.RemoveListener(StopSlowMotion);
        }
    }

    private void StartSlowMotion()
    {
        if (increaseTimeScale != null)  //TODO: nullcheck maybe dangerous, but isUnityNull() does not work
        {
            StopCoroutine(increaseTimeScale);
        }
        decreaseTimeScale = StartCoroutine(DecreaseTimeScale());
    }

    private void StopSlowMotion()
    {
        if (decreaseTimeScale != null) //TODO: nullcheck maybe dangerous, but isUnityNull() does not work
            StopCoroutine(decreaseTimeScale);

        increaseTimeScale = StartCoroutine(ReIncreaseTimeScale());
    }

    private IEnumerator DecreaseTimeScale()
    {
        while (Time.timeScale > minTimeScale)
        {
            Time.timeScale = Mathf.Lerp(maxTimeScale, minTimeScale, duration);
            AudioHandler.Instance.sourceShooting.pitch = Mathf.Lerp(maxTimeScale, minTimeScale, duration) * slowMotionSoundPitchRate;
        }
        yield return new WaitForEndOfFrame();
    }
    private IEnumerator ReIncreaseTimeScale()
    {
        while (Time.timeScale < maxTimeScale)
        {
            Time.timeScale = Mathf.Lerp(minTimeScale, maxTimeScale, duration);
            AudioHandler.Instance.sourceShooting.pitch = Mathf.Lerp(minTimeScale,maxTimeScale, duration);
        }
        yield return new WaitForEndOfFrame();
    }
}
