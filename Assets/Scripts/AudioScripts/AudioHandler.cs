using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class AudioHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AudioSource sourceShooting;
    [SerializeField] private AudioSource sourcePlayerMovement;

    [Header("Audio Sources Settings")]
    [SerializeField] private float sourceShooting_Volume;
    [SerializeField] private float sourcePlayerMovement_Volume;

    [Header("Shooting Audio")]
    [SerializeField] public AudioClip playerAim;
    [SerializeField] public AudioClip[] playerShoot;
    [SerializeField] public AudioClip playerShootCharged;

    [Header("Movement Audio")]
    [SerializeField] public AudioClip playerJump;
    [SerializeField] public AudioClip playerLanded;
    [SerializeField] public AudioClip playerWallrun;
    [SerializeField] public AudioClip playerDash;

    private void Start()
    {
        var sources = GetComponents<AudioSource>();

        sourceShooting = sources[0];
        sourcePlayerMovement = sources[1];

        sourceShooting.volume = sourceShooting_Volume;
        sourcePlayerMovement.volume = sourcePlayerMovement_Volume;
    }

    //only for shooting gun related sounds because sounds interrupt each other
    public void PlaySound_sourceShooting(AudioClip _clip)
    {
        sourceShooting.clip = _clip;
        sourceShooting.Play();
    }

    //only for movement related sounds, because sounds interrupt each other
    public void PlaySound_sourcePlayerMovement(AudioClip _clip)
    {
        sourcePlayerMovement.clip = _clip;
        sourcePlayerMovement.Play();
    }
}
