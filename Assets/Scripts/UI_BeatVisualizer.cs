using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_BeatVisualizer : MonoBehaviour
{
    [SerializeField] private BeatTracking beatTracking;
    [Space]

    [SerializeField] public Image leftBeatVisualizer;
    [SerializeField] public Image RightBeatVisualizer;
    [Space]

    [SerializeField] public Image RightRoot;
    [SerializeField] public Image LeftRoot;
    [Space]

    [SerializeField] private RectTransform RightStart;
    [SerializeField] private RectTransform RightEnd;
    [Space]

    [SerializeField] private RectTransform LeftStart;
    [SerializeField] private RectTransform LeftEnd;
    [Space]

    private float Left_Hit;
    private float right_Hit;

    private float beatTrackingToImageMultiplier;
    private float interpolationValue;
    private void Start()
    {
        interpolationValue = 1 / beatTracking.samplesPerBeat;
    }

    private void LateUpdate()
    {
        InterpolateBetween();
    }

    private void InterpolateBetween()
    {
        RightBeatVisualizer.rectTransform.position = 
            Vector3.Lerp(RightStart.position, RightEnd.position, interpolationValue * beatTracking.currentSamples);
        //VisualMultiplier =  1 / samplesProBeat
        //VisualMultiplier = currentSamples
        //VisualMultiplier gehört in den wert T von Linearer Interpolation
    }
}
