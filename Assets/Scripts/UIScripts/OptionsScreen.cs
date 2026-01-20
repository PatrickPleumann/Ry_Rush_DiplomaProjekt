using UnityEngine;
using UnityEngine.UI;

public class OptionsScreen : MonoBehaviour
{
    public Toggle ShowFPSToggle;

    public Slider MusicVolumeSlider;
    public Slider PlayerSFXVolumeSlider;
    public Slider WeaponSFXVolumeSlider;


    public void SetShowFPSToggle(bool _isOn)
    {
        ShowFPSToggle.isOn = _isOn;
    }

    public void SetMusicVolumeSlider(float _value)
    {
        MusicVolumeSlider.value = _value;
    }

    public void SetPlayerSFXSlider(float _value)
    {
        PlayerSFXVolumeSlider.value = _value;
    }

    public void SetWeaponSFXSlider(float _value)
    {
        WeaponSFXVolumeSlider.value = _value;
    }

    public void ApplySettings()
    {

    }
}
