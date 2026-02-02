using TMPro;
using UnityEngine;

public class Scoreboard_UI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BeatTracking beatTracking;

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
        beatTracking.onBeatInvoke += ComboCounterShapeShift;
        ComboCounterShapeShift();
        
    }

    private void ComboCounterShapeShift()
    {
        if (comboCounterOvershoot.activeSelf == false && currentCombo == maxCombo && currentOvershoot > 0)
        {
            comboCounterNormal.SetActive(false);
            //overshoot_Material.SetFloat(ShaderUtilities.ID_FaceDilate, 0.2f * currentOvershoot); // does not work
            comboCounter_Overshoot.text = comboCounter_Text;
            comboCounterOvershoot.SetActive(true);

        }
        else if (currentCombo < maxCombo)
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
    public void IncreaseComboCounter()
    {
        if (currentCombo < maxCombo && currentOvershoot == 0)
            currentCombo++;

        else if (currentOvershoot < maxOvershoot && currentCombo == maxCombo)
            currentOvershoot++;

        comboCounter_Text = "Combo Counter: " + currentCombo;
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
