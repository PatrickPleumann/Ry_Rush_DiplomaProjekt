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

    //[SerializeField] public float maxCombo = 5;
    //[SerializeField] public float maxOvershoot = 0; // maximum missed beats till combo counter decreases on it´s own
    //[Space]

    //private float currentScore = 0;
    private void OnEnable()
    {
        values.CurrentScore_OnValueChanged.AddListener(ShowScore);
        values.CurrentCombo_OnValueChanged.AddListener(ComboCounter_TextShift);
        //values.decreaseCombo += DecreaseComboCounter;
    }

    private void OnDisable()
    {
        values.CurrentScore_OnValueChanged.RemoveListener(ShowScore);
        values.CurrentCombo_OnValueChanged.RemoveListener(ComboCounter_TextShift);
        //values.decreaseCombo -= DecreaseComboCounter;
    }
    private void ComboCounter_TextShift(int _value)
    {
        if (comboCounterOvershoot.activeSelf == false && _value >= values.currentCombo_MaxValue && values.CurrentComboOvershoot_Value > 0)
        {
            comboCounterNormal.SetActive(false);
            comboCounterOvershoot.SetActive(true);
            comboCounter_Overshoot.text = comboCounter_Text + _value;
        }

        else if (values.CurrentCombo_Value < values.currentCombo_MaxValue)
        {
            comboCounterOvershoot.SetActive(false);
            comboCounterNormal.SetActive(true);
            comboCounter_Normal.text = comboCounter_Text + _value;
        }
    }

    private void Start()
    {
        comboCounter_Text = "Combo Counter: "; 
        score.text = "Score: " + values.CurrentScore_Value;
        comboCounter_Normal.text = comboCounter_Text;
    }

    //private void DecreaseComboCounter()
    //{
    //    if (values.CurrentComboOvershoot_Value > 0 && values.CurrentCombo_Value == values.currentCombo_MaxValue)
    //        values.CurrentComboOvershoot_Value = values.CurrentComboOvershoot_Value - 1;

    //    else if (values.CurrentComboOvershoot_Value <= 0 && values.CurrentCombo_Value > 1)
    //        values.CurrentCombo_Value = values.CurrentCombo_Value - 1;

    //    comboCounter_Text = "Combo Counter: " + values.CurrentCombo_Value;
    //}

    private void ShowScore(float _value)
    {
        score.text = "Score: " + _value;
    }

    //private void ShowComboCounter(int _value)
    //{
    //    comboCounter_Normal.text = "Combo Counter: " + _value;
    //}
}
