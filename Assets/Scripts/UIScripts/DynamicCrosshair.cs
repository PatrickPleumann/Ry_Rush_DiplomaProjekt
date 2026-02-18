using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class DynamicCrosshair : MonoBehaviour
{
    //need to precisely go 300 units for each beattracking visualizer
    [SerializeField] private BeatTracking beatTracking;
    [SerializeField] private CentralizedValues values;

    [SerializeField] private float leftOffset; // -230 currently
    [SerializeField] private float rightOffset; // 230 currently

    [SerializeField] private float leftValue;  //-300 currently
    [SerializeField] private float rightValue; // 300 currently

    [SerializeField] private Image leftVisualizer;
    [SerializeField] private Image rightVisualizer;

    [SerializeField] private Color onBeatColor;
    [SerializeField] private Color offBeatColor;

    [SerializeField] private CanvasGroup hitmarker_canvasGrp;
    [SerializeField] private float timeTillHitmarkerVanishes = 1;

    private Task currentHitmarkerVanishing;


    private float valuePerSample;


    private void OnEnable()
    {
        //change color depends on if on beat is true or false
        values.onEnemyHit.AddListener(ShowHitMarker);
    }

    private void OnDisable()
    {
        values.onEnemyHit.RemoveListener(ShowHitMarker);
        //change color depends on if on beat is true or false
    }
    private void Start()
    {
        valuePerSample = rightValue / (float)beatTracking.samplesPerBeat;
    }

    private void LateUpdate()
    {
        ChangeColor(beatTracking.isOnBeat);
        BeatVisualization();
    }

    private void BeatVisualization()
    {
        leftVisualizer.transform.localPosition = new Vector3
            (leftOffset + (valuePerSample * beatTracking.currentSamples_UI), 0f, 0f);

        rightVisualizer.transform.localPosition = new Vector3
            (rightOffset - (valuePerSample * beatTracking.currentSamples_UI), 0f, 0f);
    }

    private void ShowHitMarker()
    {
        hitmarker_canvasGrp.alpha = 1;
        HitMarkerVanishes(timeTillHitmarkerVanishes);
    }

    private async void HitMarkerVanishes(float _timeInSec)
    { // works clunky, but works. It would maybe
        while (hitmarker_canvasGrp.alpha > 0)
        {
            await Task.Delay((int)(Time.deltaTime * 1000 * _timeInSec));
            hitmarker_canvasGrp.alpha -= Time.deltaTime;
        }
    }

    private void ChangeColor(bool _onBeat)
    {
        if (_onBeat == true)
        {
            leftVisualizer.color = onBeatColor;
            rightVisualizer.color = onBeatColor;
        }
        else
        {
            leftVisualizer.color = offBeatColor;
            rightVisualizer.color = offBeatColor;
        }
    }
}
