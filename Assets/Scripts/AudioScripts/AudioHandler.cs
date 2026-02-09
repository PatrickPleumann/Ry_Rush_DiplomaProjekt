using UnityEngine;
using UnityEngine.InputSystem;

public class AudioHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerController controller;

    [SerializeField] public AudioSource sourceShooting;
    [SerializeField] private AudioSource sourcePlayerMovement;
    [SerializeField] private AudioSource sourceActionAmbience;

    [Header("Audio Sources Volumes")]
    [SerializeField] private float sourceShooting_Volume;
    [SerializeField] private float sourcePlayerMovement_Volume;
    [SerializeField] private float sourceActionAmbience_Volume;

    [Header("Shooting Audio")]
    [SerializeField] public AudioClip playerAim;
    [SerializeField] public AudioClip playerShoot;
    [SerializeField] public AudioClip playerShootCharged;
    [SerializeField] public AudioClip gunMagOut;
    [SerializeField] public AudioClip gunMagIn;
    [SerializeField] public AudioClip gunRecieverClick;
    [SerializeField] public AudioClip noAmmoClick;


    [Header("Movement Audio")]
    [SerializeField] public AudioClip playerJump;
    [SerializeField] public AudioClip playerLanded;
    [SerializeField] public AudioClip playerWallrun;
    [SerializeField] public AudioClip playerDash;

    [Header("Hitmarker Audio")]
    [SerializeField] public AudioClip hitmarker_1;
    [SerializeField] public AudioClip hitmarker_2;
    [SerializeField] public AudioClip hitmarker_3;
    [SerializeField] public AudioClip hitmarker_4;

    [Header("Scoreboard Audio")]
    [SerializeField] public AudioClip scoreboard_Hit;
    [SerializeField] public AudioClip scoreboard_Hit2;


    public static AudioHandler Instance;

    private void Awake()
    {
        if (Instance != null)
            Destroy(Instance);

        Instance = this;
    }

    private void Start()
    {
        //var sources = GetComponents<AudioSource>(); // audiosources assigned in editor

        //sourceShooting = sources[0];
        //sourcePlayerMovement = sources[1];
        //sourceActionAmbience = sources[2];

        sourceShooting.volume = sourceShooting_Volume;
        sourcePlayerMovement.volume = sourcePlayerMovement_Volume;
        sourceActionAmbience.volume = sourceActionAmbience_Volume;

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

    public void PlaySound_sourceActionAmbience(AudioClip _clip)
    {
        sourceActionAmbience.clip = _clip;
        sourceActionAmbience.Play();
    }
    private void OnDestroy()
    {
        Instance = null;
    }

    public void PlayScoreboardSounds(AudioClip _clip)
    {
        sourceActionAmbience.clip = _clip;
        sourceActionAmbience.Play();
    }
}
