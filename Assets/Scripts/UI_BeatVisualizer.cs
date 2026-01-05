using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_BeatVisualizer : MonoBehaviour
{
    [SerializeField] private BeatTracking beatTracking;
    [Space]
    [SerializeField] public Image RightBeatVisualizer;
    [SerializeField] public Image RightBeatVisualizer_2;
    [Space]
    [SerializeField] public Image leftBeatVisualizer;
    [SerializeField] public Image leftBeatVisualizer_2;

    private float rightValue;
    private float leftValue;

    private float valuePerSample;
    private void Start()
    {
        rightValue = RightBeatVisualizer.rectTransform.localPosition.x;
        leftValue = leftBeatVisualizer.rectTransform.localPosition.x;

        valuePerSample = (rightValue / (float)beatTracking.samplesPerBeat);
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
}
