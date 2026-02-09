using System.ComponentModel;
using System.Drawing.Text;
using System.Security.Policy;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "CentralizedValues", menuName = "Scriptable Objects/CentralizedValues")]


public class CentralizedValues : ScriptableObject
{

    private void SetDefaultValues()
    {
        CurrentCombo_Value = 1;
        CurrentComboOvershoot_Value = 0;
        CurrentScore_Value = 0;
        currentSamples_UI = 0;
    }
    /// <summary>
    /// Every single Properties is seperated with a [Space]
    /// Those properties single usecase is to transfer data towards another system after they were changed. This happens eventbased.
    /// Those "action data values" are handled seperately from values which are gathered 
    /// in the main menu, because they are changed permanently while runtime.
    /// </summary>

    [Header("Input Values")]
    [SerializeField] private float moveInput_Value;
    [HideInInspector] public UnityEvent<float> MoveInput_OnValueChanged;
    public float MoveInput_Value
    {
        get => moveInput_Value;
        set
        {
            if (moveInput_Value != value)
            {
                moveInput_Value = value;
                MoveInput_OnValueChanged.Invoke(moveInput_Value);
            }
        }
    }

    [Space]
    [Header("Beattracking Values")]
    [SerializeField] private int currentSamples_UI;
    [HideInInspector] public UnityEvent<int> CurrentSamples_OnValueChanged;
    public int CurrentSamples_Value
    {
        get => currentSamples_UI;
        set
        {
            if (currentSamples_UI != value)
            {
                currentSamples_UI = value;
                CurrentSamples_OnValueChanged.Invoke(currentSamples_UI);
            }
        }
    }

    [Space]
    [Header("Ingame UI")]
    [SerializeField] public int currentCombo_MaxValue;
    [SerializeField] private int currentCombo_Value;
    [HideInInspector] public UnityEvent<int> CurrentCombo_OnValueChanged;
    public int CurrentCombo_Value
    {
        get => currentCombo_Value;
        set
        {
            if (currentCombo_Value != value && value <= currentCombo_MaxValue && value > 0)
            {
                currentCombo_Value = value;
            }
            CurrentCombo_OnValueChanged.Invoke(currentCombo_Value);
        }
    }

    [Space]
    [SerializeField] public int ComboMaxOvershoot;
    [SerializeField] private int currentComboOvershoot;
    [HideInInspector] public UnityEvent<int> ComboOvershoot_OnValueChanged;
    public int CurrentComboOvershoot_Value
    {
        get => currentComboOvershoot;
        set
        {
            if (currentComboOvershoot != value && value <= ComboMaxOvershoot)
            {
                currentComboOvershoot = value;
                ComboOvershoot_OnValueChanged.Invoke(currentComboOvershoot);
            }
        }
    }

    [Space]
    [Header("Ammo Values")]
    [SerializeField] private int currentAmmo;
    [HideInInspector] public UnityEvent<int> CurrentAmmo_OnValueChanged;
    public int CurrentAmmo_Value
    {
        get => currentAmmo;
        set
        {
            if (currentAmmo != value)
            {
                currentAmmo = value;
                CurrentAmmo_OnValueChanged.Invoke(currentAmmo);
            }
        }
    }

    [Space]
    [Header("Score Values")]
    [SerializeField] private float currentScore_Value;
    [HideInInspector] public UnityEvent<float> CurrentScore_OnValueChanged;
    public float CurrentScore_Value
    {
        get => currentScore_Value;
        set
        {
            if (currentScore_Value != value)
            {
                currentScore_Value = value;
                CurrentScore_OnValueChanged.Invoke(currentScore_Value);
            }
        }
    }
}
