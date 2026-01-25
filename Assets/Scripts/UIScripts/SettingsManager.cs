using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    private const string settingsFileName = "Settings.json";
    private string SettingsJsonPath;

    [SerializeField] private SettingsData settingsData;

    [SerializeField] private Slider musicVolume;
    [SerializeField] private Slider playerSFX;
    [SerializeField] private Slider weaponSFX;
    [SerializeField] private Toggle showFPS;

    private void Awake()
    {
        SettingsJsonPath = Application.persistentDataPath + "/" + settingsFileName;
    }
    private void OnEnable()
    {
        LoadDataFromJson();

        musicVolume.onValueChanged.AddListener(MusicVolume_ValueChanged);
        playerSFX.onValueChanged.AddListener(PlayerSFXVolume_ValueChanged);
        weaponSFX.onValueChanged.AddListener(WeaponSFXVolume_ValueChanged);
        showFPS.onValueChanged.AddListener(ShowFPS_ValueChanged);

        SetAllSettings();
    }
    public void SetAllSettings()
    {
        SetMusicVolume(settingsData.MusicVolume);
        SetPlayerSFXVolume(settingsData.PlayerSFXVolume);
        SetWeaponSFXVolume(settingsData.WeaponSFXVolume);
        SetShowFPSToggle(settingsData.ShowFPS);
    }
    public void LoadDataFromJson()
    {
        if (File.Exists(SettingsJsonPath) == true)
        {
            var json = File.ReadAllText(SettingsJsonPath);
            JsonUtility.FromJsonOverwrite(json, settingsData);
            SetAllSettings();
        }
        else
            Debug.Log("No Settings.json found at: " + Application.persistentDataPath);
    }

    public void SafeDataIntoJson()
    {
        var json = JsonUtility.ToJson(settingsData);
        var path = Path.Combine(SettingsJsonPath);
        File.WriteAllText(path, json);
    }



    #region Set Values in visible menu
    private void SetMusicVolume(float _value)
    {
        musicVolume.value = _value;
    }
    private void SetPlayerSFXVolume(float _value)
    {
        playerSFX.value = _value;
    }

    private void SetWeaponSFXVolume(float _value)
    {
        weaponSFX.value = _value;
    }

    private void SetShowFPSToggle(bool _value)
    {
        showFPS.isOn = _value;
    }
    #endregion

    #region On Value Changed Methods
    private void MusicVolume_ValueChanged(float _value)
    {
        settingsData.MusicVolume = _value;
    }

    private void PlayerSFXVolume_ValueChanged(float _value)
    {
        settingsData.PlayerSFXVolume = _value;
    }
    private void WeaponSFXVolume_ValueChanged(float _value)
    {
        settingsData.WeaponSFXVolume = _value;
    }

    private void ShowFPS_ValueChanged(bool _value)
    {
        settingsData.ShowFPS = _value;
    }
    #endregion
}
