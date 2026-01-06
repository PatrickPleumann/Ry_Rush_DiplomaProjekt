using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_BeatVisualizer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BeatTracking beatTracking;
    [Space]
    [SerializeField] public Image RightBeatVisualizer;
    [SerializeField] public Image RightBeatVisualizer_2;
    [Space]
    [SerializeField] public Image leftBeatVisualizer;
    [SerializeField] public Image leftBeatVisualizer_2;

    [Header("Pulse To Beat")]
    [SerializeField] private float _pulseSize = 1.15f;
    [SerializeField] private float _returnSpeed = 3f;
    private Vector3 _startSize = new Vector3(1, 1, 1);

    private float rightValue;
    private float leftValue;

    private float valuePerSample;
    private void OnEnable()
    {
        beatTracking.onBeatInvoke += PulseToBeat;
    }

    private void OnDisable()
    {
        beatTracking.onBeatInvoke -= PulseToBeat;
    }
    private void Start()
    {
        rightValue = RightBeatVisualizer.rectTransform.localPosition.x;
        leftValue = leftBeatVisualizer.rectTransform.localPosition.x;

        valuePerSample = (rightValue / (float)beatTracking.samplesPerBeat);
    }

    private void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, _startSize, Time.deltaTime * _returnSpeed);
    }
    private void LateUpdate()
    {
        BeatVisualization();
    }

    private void BeatVisualization()
    {
        RightBeatVisualizer.transform.localPosition = 
            new Vector3(rightValue - (valuePerSample * beatTracking.currentSamples_UI),0f,0f);

        leftBeatVisualizer.transform.localPosition =
            new Vector3(leftValue + (valuePerSample * beatTracking.currentSamples_UI), 0f, 0f);
    }

    public void PulseToBeat()
    {
        transform.localScale = _startSize * _pulseSize;
    }
}
