using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class GlobalVolume_ValueApplier : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PostProcessVolume pp_volume;
    [SerializeField] private CentralizedValues values;

    [Header("Volume Components & Volume Intensities")]
    [SerializeField] private Vignette vignette;
    [SerializeField] private float BloodyVignette_MaxIntensity;

    [Space]

    [SerializeField] private LensDistortion lensDistortion;
    [SerializeField] private float LensDistortion_Intensity;
    [SerializeField] private float LensDistortion_TimeValue;
    private void Awake()
    {
        pp_volume = GetComponent<PostProcessVolume>();
    }

    private void OnEnable()
    {
        values.onDashExecuted += ApplyLensDistortion;
    }

    private void Start()
    {
        pp_volume.profile.TryGetSettings(out Vignette _vignette);
        vignette = _vignette;

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
        yield return new WaitForEndOfFrame();
        
    }
}
