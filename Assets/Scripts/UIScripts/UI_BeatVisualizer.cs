using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_BeatVisualizer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BeatTracking beatTracking;
    [Space]
    [SerializeField] public Image RightBeatVisualizer;
    [Space]
    [SerializeField] public Image leftBeatVisualizer;
    [Space]
    [SerializeField] public Image leftMin;
    [SerializeField] public Image leftMax;

    [SerializeField] public Image rightMin;
    [SerializeField] public Image rightMax;

    [Header("Pulse To Beat")]
    [SerializeField] private float pulseSize = 1.15f;
    [SerializeField] private float returnSpeed = 3f;
    private Vector3 _startSize = new Vector3(1, 1, 1);

    private float rightValue;
    private float leftValue;

    private float valuePerSample;
    //private void OnEnable()
    //{
    //    beatTracking.onBeatInvoke += PulseToBeat;
    //}

    //private void OnDisable()
    //{
    //    beatTracking.onBeatInvoke -= PulseToBeat;
    //}
    private void Start()
    {
        rightValue = RightBeatVisualizer.rectTransform.localPosition.x;
        leftValue = leftBeatVisualizer.rectTransform.localPosition.x;

        valuePerSample = (rightValue / (float)beatTracking.samplesPerBeat);
    }

    private void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, _startSize, Time.deltaTime * returnSpeed);
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
        transform.localScale = _startSize * pulseSize;
    }
}
