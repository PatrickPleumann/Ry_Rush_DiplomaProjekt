using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private CentralizedValues values;
    [SerializeField] private GameObject SCOREBOARD;
    [SerializeField] private UI_HighscoreBoard scoreboard;

    [SerializeField] public SettingsData settingsData;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;

        values.SetDefaultValues();
    }

    private void OnEnable()
    {
        values.onSessionEnds += OnSessionEnds;
    }

    private void OnDisable()
    {
        values.onSessionEnds -= OnSessionEnds;
    }
    private void OnDestroy()
    {
        Instance = null;
    }

    private void OnSessionEnds()
    {
        SCOREBOARD.SetActive(true);
    }
}
