using TMPro;
using UnityEngine;

public class Scoreboard_UI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BeatTracking beatTracking;
    [SerializeField] private CentralizedValues values;
 
    [SerializeField] private GameObject comboCounterNormal;
    [SerializeField] private GameObject comboCounterOvershoot;
    [Space]

    [SerializeField] private TMP_Text comboCounter_Normal;
    [SerializeField] private TMP_Text comboCounter_Overshoot;
    [SerializeField] private Material overshoot_Material;

    private string comboCounter_Text;

    [SerializeField] private TMP_Text score;
    [Space]


    [Header("Combo values")]
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
        values.CurrentScore_OnValueChanged.AddListener(IncreaseScore);
        values.CurrentCombo_OnValueChanged.AddListener(IncreaseComboCounter);
    }

    private void ComboCounterShapeShift(int _value)
    {
        if (comboCounterOvershoot.activeSelf == false && _value == values.currentCombo_MaxValue && values.CurrentComboOvershoot > 0)
        {
            comboCounterNormal.SetActive(false);
            comboCounter_Overshoot.text = comboCounter_Text;
            comboCounterOvershoot.SetActive(true);

        }
        else if (values.CurrentCombo_Value < values.currentCombo_MaxValue)
        {
            comboCounterNormal.SetActive(true);
            comboCounter_Normal.text = comboCounter_Text;
            comboCounterOvershoot.SetActive(false);
        }
    }

    private void Start()
    {
        comboCounter_Text = "Combo Counter: " + currentCombo;
        score.text = "Score: " + currentScore;
        comboCounter_Normal.text = comboCounter_Text;
    }
    public void IncreaseComboCounter(int _value)
    {
        //if (currentCombo < maxCombo && currentOvershoot == 0)
        //    currentCombo++;

        //else if (currentOvershoot < maxOvershoot && currentCombo == maxCombo)
        //    currentOvershoot++;
        ComboCounterShapeShift(_value);

        comboCounter_Text = "Combo Counter: " + _value;
    }

    public void DecreaseComboCounter()
    {
        if (currentOvershoot > 0 && currentCombo == maxCombo)
            currentOvershoot--;

        else if (currentOvershoot <= 0 && currentCombo > 1)
            currentCombo--;

        comboCounter_Text = "Combo Counter: " + currentCombo;
    }

    public void IncreaseScore(float _points)
    {
        currentScore += (_points * currentCombo);
        score.text = "Score: " + currentScore;
    }
}
