using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    private const string settingsFileName = "Settings.json";
    private string SettingsJsonPath;

    [SerializeField] private SettingsData settingsData;

    [SerializeField] private Slider playerSFXVolume;
    [SerializeField] private Slider weaponSFXVolume;
    [SerializeField] private Slider ambienceSFXVolume;

    [SerializeField] private Toggle showFPS;
    [SerializeField] private Toggle slowMotionOnAim;

    private void Awake()
    {
        SettingsJsonPath = Application.persistentDataPath + "/" + settingsFileName;
    }

    private void OnEnable()
    {
        LoadDataFromJson();

        playerSFXVolume.onValueChanged.AddListener(PlayerSFXVolumeApplyValue);
        weaponSFXVolume.onValueChanged.AddListener(WeaponSFXVolumeApplyValue);
        ambienceSFXVolume.onValueChanged.AddListener(AmbienceSFXVolumeApplyValue);
        showFPS.onValueChanged.AddListener(ShowFPSApplyValue);
        slowMotionOnAim.onValueChanged.AddListener(SlowMotionOnAimApplyValue);

        SetAllSettings();
    }

    public void SetAllSettings()
    {
        if (settingsData != null)
        {
            SetPlayerSFXVolume(settingsData.PlayerSFXVolume);
            SetWeaponSFXVolume(settingsData.WeaponSFXVolume);
            SetAmbienceSFXVolume(settingsData.AmbienceVolume);
            SetShowFPSToggle(settingsData.ShowFPS);
            SetSlowMotionOnAimToggle(settingsData.SlowMotionOnAim);
        }
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

    private void SetPlayerSFXVolume(float _value)
    {
        playerSFXVolume.value = _value;
    }
    private void SetWeaponSFXVolume(float _value)
    {
        weaponSFXVolume.value = _value;
    }
    private void SetAmbienceSFXVolume(float _value)
    {
        ambienceSFXVolume.value = _value;
    }
    private void SetShowFPSToggle(bool _value)
    {
        showFPS.isOn = _value;
    }
    private void SetSlowMotionOnAimToggle(bool _value)
    {
        slowMotionOnAim.isOn = _value;
    }

    #endregion

    #region On Value Changed Methods

    private void PlayerSFXVolumeApplyValue(float _value)
    {
        settingsData.PlayerSFXVolume = _value;
    }
    private void WeaponSFXVolumeApplyValue(float _value)
    {
        settingsData.WeaponSFXVolume = _value;
    }
    private void AmbienceSFXVolumeApplyValue(float _value)
    {
        settingsData.AmbienceVolume = _value;
    }

    private void ShowFPSApplyValue(bool _value)
    {
        settingsData.ShowFPS = _value;
    }

    private void SlowMotionOnAimApplyValue(bool _value)
    {
        settingsData.SlowMotionOnAim = _value;
    }

    #endregion
}
