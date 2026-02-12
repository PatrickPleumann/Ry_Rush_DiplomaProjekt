using UnityEngine;
using UnityEngine.UI;

public class DynamicCrosshair : MonoBehaviour
{
    //need to precisely go 300 units for each beattracking visualizer
    [SerializeField] private BeatTracking beatTracking;

    [SerializeField] private float leftOffset; // -230 currently
    [SerializeField] private float rightOffset; // 230 currently

    [SerializeField] private float leftValue;  //-300 currently
    [SerializeField] private float rightValue; // 300 currently

    [SerializeField] private Image leftVisualizer;
    [SerializeField] private Image rightVisualizer;

    [SerializeField] private Color onBeatColor;
    [SerializeField] private Color offBeatColor;


    private float valuePerSample;


    private void OnEnable()
    {
        //change color depends on if on beat is true or false
    }

    private void OnDisable()
    {
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
