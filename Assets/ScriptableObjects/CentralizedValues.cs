using Unity.VisualScripting;
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

        AllowInput = true;

        TimeBetweenBeats = 0f;

        enemysAliveCount = 0;

        PlayerCurrentHealth = 100;
        PlayerMaxHealth = 100;

        GameIsPaused = false;
        PlayerIsDead = false;
        SessionIsOver = false;
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
            if (AllowInput == true && moveInput_Value != value)
            {
                moveInput_Value = value;
                MoveInput_OnValueChanged.Invoke(moveInput_Value);
            }
        }
    }

    [SerializeField] public Vector2 LookInput_Value;

    [SerializeField] public bool AllowInput = true;

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
    [SerializeField] public int CurrentComboMaxValue;
    [SerializeField] private int currentComboValue;
    [HideInInspector] public UnityEvent<int> CurrentCombo_OnValueChanged;
    public int CurrentCombo_Value
    {
        get => currentComboValue; // muhahaha
        set
        {
            if (currentComboValue != value && value <= CurrentComboMaxValue && value > 0 && CurrentComboOvershoot_Value <= 0)
                currentComboValue = value;

            else if (currentComboValue == CurrentComboMaxValue && CurrentComboOvershoot_Value < ComboMaxOvershoot)
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

    [Space]

    [Header("Ingame Values")]
    
    [SerializeField] public float PlayerMaxHealth;
    [SerializeField] private float playerCurrentHealth;
    [HideInInspector] public UnityEvent<float> PlayerCurrentHealth_OnValueChanged;
    public float PlayerCurrentHealth
    {
        get => playerCurrentHealth;
        set
        {
            if (playerCurrentHealth != value)
            {
                if (value > PlayerMaxHealth)
                    playerCurrentHealth = PlayerMaxHealth;

                else
                    playerCurrentHealth = value;

                Debug.Log("Wurde überschrieben");
                PlayerCurrentHealth_OnValueChanged.Invoke(playerCurrentHealth);
            }
        }
    }

    [SerializeField] private int enemysAliveCount;
    [HideInInspector] public UnityEvent<int> EnemyCount_OnValueChanged;
    public int EnemysAliveCount
    {
        get => enemysAliveCount;
        set
        {
            if (enemysAliveCount != value)
            {
                enemysAliveCount = value;
                EnemyCount_OnValueChanged.Invoke(enemysAliveCount);
            }
        }
    }

    [SerializeField] private bool gameIsPaused;
    [HideInInspector] public UnityEvent<bool> GameIsPaused_OnValueChangend;
    public bool GameIsPaused
    {
        get => gameIsPaused;
        set
        {
            if (gameIsPaused != value)
            {
                gameIsPaused = value;
                GameIsPaused_OnValueChangend.Invoke(gameIsPaused);
            }
        }
    }

    [SerializeField] private bool playerIsDead = false;
    [HideInInspector] public UnityEvent<bool> PlayerIsDead_OnValueChanged;
    public bool PlayerIsDead
    {
        get => playerIsDead;
        set
        {
            if (playerIsDead != value)
            {
                playerIsDead = value;
                PlayerIsDead_OnValueChanged.Invoke(playerIsDead);
            }
        }
    }

    [Space]

    [Header("Slow Motion Values")]

    [Space]

    public bool SessionIsOver = false;
    [HideInInspector] public UnityEvent DisAllowSlowMotion;
    [HideInInspector] public UnityEvent OnSessionEnds;
    [HideInInspector] public UnityEvent OnDashExecuted; // not in use, for post processing effects while dashing
    [HideInInspector] public UnityEvent OnEnemyHit;
    [HideInInspector] public UnityEvent OnPlayerDeath;
    [HideInInspector] public UnityEvent OnSlowMotionActivated;
    [HideInInspector] public UnityEvent<float> OnSlowMotionPitchSource;
}
