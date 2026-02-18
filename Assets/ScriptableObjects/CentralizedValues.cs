using System.ComponentModel;
using System.Drawing.Text;
using System.Security.Policy;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "CentralizedValues", menuName = "Scriptable Objects/CentralizedValues")]


public class CentralizedValues : ScriptableObject
{

    public void SetDefaultValues()
    {
        CurrentCombo_Value = 1;
        CurrentComboOvershoot_Value = 0;
        CurrentScore_Value = 0;
        currentSamples_UI = 0;

        ShotsHit = 0;
        ShotsFired = 0;
        Kills = 0;
        OnBeatActions = 0;

        AllowInput_Bool = true;

        TimeBetweenBeats = 0f;

        enemysAliveCount = 0;

        PlayerCurrentHealth = 100;
        playerMaxHealth = 100;
    }
    /// <summary>
    /// Every single Properties is seperated with a [Space]
    /// Those properties single usecase is to transfer data towards another system after they were changed. This happens eventbased.
    /// Those "action data values" are handled seperately from values which are gathered 
    /// in the main menu, because they are changed permanently while runtime.
    /// </summary>

    [Header("Input Values")]
    [SerializeField] private Vector2 moveInput_Value;
    [HideInInspector] public UnityEvent<Vector2> MoveInput_OnValueChanged;
    public Vector2 MoveInput_Value
    {
        get => moveInput_Value;
        set
        {
            if (AllowInput_Bool == true && moveInput_Value != value)
            {
                moveInput_Value = value;
                MoveInput_OnValueChanged.Invoke(moveInput_Value);
            }
        }
    }

    [SerializeField] public Vector2 LookInput_Value;

    [SerializeField] public bool AllowInput_Bool = true;

    [Space]
    [Header("Beattracking Values")]
    [SerializeField] public float TimeBetweenBeats;
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
            }
            CurrentSamples_OnValueChanged.Invoke(currentSamples_UI);
        }
    }

    [Space]

    [HideInInspector] public UnityAction decreaseCombo;
    public bool LastActionOnBeat_Bool = false;

    [Space]
    [Header("Ingame UI")]

    [SerializeField] public int currentCombo_MaxValue;
    [SerializeField] private int currentCombo_Value;
    [HideInInspector] public UnityEvent<int> CurrentCombo_OnValueChanged;
    public int CurrentCombo_Value
    {
        get => currentCombo_Value; // muhahaha
        set
        {
            if (currentCombo_Value != value && value <= currentCombo_MaxValue && value > 0 && CurrentComboOvershoot_Value <= 0)
                currentCombo_Value = value;

            else if (currentCombo_Value == currentCombo_MaxValue && CurrentComboOvershoot_Value < ComboMaxOvershoot)
                CurrentComboOvershoot_Value = CurrentComboOvershoot_Value + 1;

            CurrentCombo_OnValueChanged.Invoke(CurrentCombo_Value);
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
                currentComboOvershoot = value;
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

    [SerializeField] public float ShotsFired;
    [SerializeField] public float ShotsHit;
    [SerializeField] public float Kills;
    [SerializeField] public int OnBeatActions;

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


    [Header("Ingame Values")]
    [SerializeField] public float playerMaxHealth;
    [SerializeField] private float playerCurrentHealth;
    [HideInInspector] public UnityEvent<float> PlayerCurrentHealth_onValueChanged;
    public float PlayerCurrentHealth
    {
        get => playerCurrentHealth;
        set
        {
            if (playerCurrentHealth != value)
            {
                if (value > playerMaxHealth)
                    playerCurrentHealth = playerMaxHealth;

                else
                    playerCurrentHealth = value;

                Debug.Log("Wurde überschrieben");
                PlayerCurrentHealth_onValueChanged.Invoke(playerCurrentHealth);
            }
        }
    }

    [SerializeField] private int enemysAliveCount;
    [HideInInspector] public UnityEvent<int> EnemyCount_onValueChanged;
    public int EnemysAliveCount
    {
        get => enemysAliveCount;
        set 
        {
            if (enemysAliveCount != value)
            {
                enemysAliveCount = value;
                EnemyCount_onValueChanged.Invoke(enemysAliveCount);
            }
        }
    }



    [Space]
    [Header("Game Manager")]
    [HideInInspector] public UnityEvent onSessionEnds;
    [HideInInspector] public UnityEvent onDashExecuted;
    [HideInInspector] public UnityEvent onEnemyHit;

}
