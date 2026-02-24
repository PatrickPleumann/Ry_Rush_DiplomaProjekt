using UnityEngine;

[CreateAssetMenu(fileName = "SettingsData", menuName = "Scriptable Objects/SettingsData")]
public class SettingsData : ScriptableObject, ISetDefaultValues
{
    [Header("Sound Volumes")]
    public float PlayerSFXVolume;
    public float WeaponSFXVolume;
    public float AmbienceVolume;

    [Header("Gameplay")]
    public bool ShowFPS;
    public bool SlowMotionOnAim;

    public void SetDefaultValues()
    {
        PlayerSFXVolume = 0.2f;
        WeaponSFXVolume = 0.2f;
        AmbienceVolume = 0.2f;

        ShowFPS = false;
        SlowMotionOnAim = false;
    }
}
