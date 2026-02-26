using UnityEngine;

public class PlayReloadSounds : MonoBehaviour
{
    [SerializeField] private CentralizedValues values;
    public void PlayMagEmptySound()
    {
        AudioHandler.Instance.PlaySound_sourceShooting(AudioHandler.Instance.MagEmptySound);
    }
    public void PlayMagOutSound()
    {
        AudioHandler.Instance.PlaySound_sourceShooting(AudioHandler.Instance.gunMagOut);

        if (values.IsOnBeat == true)
            values.CurrentCombo_Value = values.CurrentCombo_Value + 1;
    }

    public void PlayMagInSound()
    {
        AudioHandler.Instance.PlaySound_sourceShooting(AudioHandler.Instance.gunMagIn);

        if (values.IsOnBeat == true)
            values.CurrentCombo_Value = values.CurrentCombo_Value + 1;
    }

    public void PlayRecieverClickSound()
    {
        AudioHandler.Instance.PlaySound_sourceShooting(AudioHandler.Instance.gunRecieverClick);
    }
}
