using UnityEngine;

[CreateAssetMenu(fileName = "SettingsData", menuName = "Scriptable Objects/SettingsData")]
public class SettingsData : ScriptableObject
{
    [Header("Sound Volumes")]
    public float MusicVolume;
    public float PlayerSFXVolume;
    public float WeaponSFXVolume;

    [Header("Gameplay")]
    public bool ShowFPS;

    //check settingsmanager for how to handle the values
}
