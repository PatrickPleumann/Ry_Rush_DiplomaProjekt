using UnityEngine;

public class PlayReloadSounds : MonoBehaviour
{
    public void PlayMagOutSound()
    {
        AudioHandler.Instance.PlaySound_sourceShooting(AudioHandler.Instance.gunMagOut);
    }

    public void PlayMagInSound()
    {
        AudioHandler.Instance.PlaySound_sourceShooting(AudioHandler.Instance.gunMagIn);
    }

    public void PlayRecieverClickSound()
    {
        AudioHandler.Instance.PlaySound_sourceShooting(AudioHandler.Instance.gunRecieverClick);
    }
}
