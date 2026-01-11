using TMPro;
using UnityEngine;

public class Scoreboard_UI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BeatTracking beatTracking;
    [SerializeField] private TMP_Text comboCounter;
    [SerializeField] private TMP_Text score;
    [Space]
    [Header("Font Settings on Overshoot")]
    [SerializeField] private float fontSizeDefault;
    [SerializeField] private float fontSizeOvershoot;
    [Space]
    [SerializeField] public float currentCombo = 1;
    [SerializeField] public float currentOvershoot = 0;
    [Space]
    [SerializeField] public float maxCombo = 5;
    [SerializeField] public float maxOvershoot = 0; // maximum missed beats till combo counter decreases on it´s own
    [Space]
    public bool lastActionOnBeat;

    private float currentScore = 0;
    private void OnEnable()
    {
        beatTracking.onBeatInvoke += ComboCounterShapeShift;
    }

    private void ComboCounterShapeShift()
    {
        if (currentCombo == maxCombo && currentOvershoot > 0)
        {
            comboCounter.fontSize = fontSizeOvershoot;
            comboCounter.fontStyle = FontStyles.Italic;
            comboCounter.fontStyle = FontStyles.Bold;
        }
        else
        {
            comboCounter.fontSize = fontSizeDefault; 
        }
    }

    private void Start()
    {
        comboCounter.text = "Combo Counter: " + currentCombo;
        score.text = "Score: " + currentScore;
    }
    public void IncreaseComboCounter()
    {
        if (currentCombo < maxCombo && currentOvershoot == 0)
            currentCombo++;

        else if (currentOvershoot < maxOvershoot && currentCombo == maxCombo)
            currentOvershoot++;

            comboCounter.text = "Combo Counter: " + currentCombo;
    }

    public void DecreaseComboCounter()
    {
        if (currentOvershoot > 0 && currentCombo == maxCombo)
            currentOvershoot--;

        else if (currentOvershoot <= 0 && currentCombo > 1)
            currentCombo--;
    }

    public void HoldCurrentCombo()
    {
        lastActionOnBeat = true;
    }

    public void IncreaseScore(float _points)
    {
        currentScore += (_points * currentCombo);
        score.text = "Score: " + currentScore;
    }
}
