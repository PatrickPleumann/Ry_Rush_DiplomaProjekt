using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class GlobalVolume_ValueApplier : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PostProcessVolume pp_volume;
    [SerializeField] private CentralizedValues values;

    [SerializeField] private Vignette vignette;
    [SerializeField] private float vignette_Intensity;
    [Header("Volume Components & Volume Intensities")]

    [SerializeField] private float BloodyVignette_MaxIntensity;

    [Space]

    [SerializeField] private LensDistortion lensDistortion;
    [SerializeField] private float LensDistortion_Intensity;
    [SerializeField] private float LensDistortion_TimeValue;


    private void OnEnable()
    {
        values.onDashExecuted.AddListener(ApplyLensDistortion);
    }
    private void OnDisable()
    {
        values.onDashExecuted.RemoveListener(ApplyLensDistortion);
    }
    private void Start()
    {
        pp_volume.profile.TryGetSettings(out vignette);
        pp_volume.profile.TryGetSettings(out lensDistortion);
    }
    private void ApplyBloodyVignette(float _intensity)
    {
        if (_intensity < BloodyVignette_MaxIntensity)
        {

        }
    }

    public void ApplyLensDistortion()
    {
        StartCoroutine(PingPongLenseDistortion());
    }

    private IEnumerator PingPongLenseDistortion()
    {
        yield return null;
    }
}
