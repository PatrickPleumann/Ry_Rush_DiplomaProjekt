using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
//using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Rendering.Universal;

public class GlobalVolume_ValueApplier : MonoBehaviour
{
    [Header("References")]
    //[SerializeField] private PostProcessVolume pp_volume;
    [SerializeField] private CentralizedValues values;
    [SerializeField] private Volume vol;


    [Header("Volume Components & Volume Intensities")]

    [SerializeField] private Vignette vignette;
    [SerializeField] private float vignette_Intensity;

    [SerializeField] private float BloodyVignette_MaxIntensity;

    [Space]

    [SerializeField] private LensDistortion lensDistortion;
    [SerializeField] private float LensDistortion_Intensity;
    [SerializeField] private float LensDistortion_TimeValue;


    private void OnEnable()
    {
        values.OnDashExecuted.AddListener(ApplyLensDistortion);
    }
    private void OnDisable()
    {
        values.OnDashExecuted.RemoveListener(ApplyLensDistortion);
    }
    private void Start()
    {
        var temp = vol.profile.TryGet<Vignette>(out vignette);

        vignette.intensity.value = 100;
        

        //pp_volume.profile.TryGetSettings(out vignette);
        //pp_volume.profile.TryGetSettings(out lensDistortion);
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
