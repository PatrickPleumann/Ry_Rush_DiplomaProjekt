using System.Collections;
using UnityEngine;
using UnityEngine.Events;


public class BeatTracking : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public Scoreboard_UI scoreboard;
    [SerializeField] public AudioSource source;
    [SerializeField] public AudioClip clip;

    [Header("Gameplay Values")]
    [SerializeField] private float timeTillSongStarts = 3f;

    [Header("Events")]
    public UnityAction onBeatInvoke;

    [Header("Beat Tracking Values")]

    [SerializeField] public int asyncValue;
    [SerializeField] private float beatOffsetMultiplier;
    [SerializeField] public int samplesPerBeat = 0;
    [SerializeField] public float bpm = 0f;



    public int currentSamples = 0;
    public int currentTimeSamplesMin = 0;
    public int currentTimeSamplesMax = 0;
    public bool isOnBeat = false;
    public int onBeatOffset;
    public int beatCounter = 1;

    [Header("Beattracking Values for UI Visualization")]
    public int currentSamples_UI = 0;
    public int samplesPerBeat_UI = 0;

    public int lastActionOnBeatCounter = 0;
    public bool lastActionOnBeat = false;

    [SerializeField] private int ComboOvershootValue = 3;

    private int lastFrameSamples = 0;
    private int beatMultiplier = 1;



    private void Awake()
    {
        source = GetComponent<AudioSource>();
        source.clip = clip;

        samplesPerBeat = (int)(source.clip.frequency * (60f / bpm) * beatMultiplier);
        onBeatOffset = (int)(samplesPerBeat * beatOffsetMultiplier) * beatMultiplier;

        samplesPerBeat_UI = samplesPerBeat + onBeatOffset;
        
    }

    private void Start()
    {
        //calculate with beat multiplier here 
        currentSamples += asyncValue;
        currentSamples_UI = currentSamples;

        currentSamples_UI += onBeatOffset;

        StartCoroutine(StartSongDelayed(timeTillSongStarts));
    }
    private void Update()
    {
        isOnBeat = CheckForNewBeat();
    }


    IEnumerator StartSongDelayed(float _timeTillSongStarts)
    {
        yield return new WaitForSecondsRealtime(_timeTillSongStarts);
        source.Play();
    }
    public bool CheckForNewBeat()
    {
        currentSamples += source.timeSamples - lastFrameSamples;
        currentSamples_UI += source.timeSamples - lastFrameSamples;
        lastFrameSamples = source.timeSamples;

        if ((currentSamples / samplesPerBeat) >= 1)
        {
            currentSamples -= samplesPerBeat;
            onBeatInvoke.Invoke();
            beatCounter++;

            if (lastActionOnBeat == true)
                lastActionOnBeat = false;

            else if (lastActionOnBeat == false)
                scoreboard.DecreaseComboCounter();
        }

        if ((currentSamples_UI / samplesPerBeat_UI) >= 1)
        {
            currentSamples_UI = currentSamples;
        }

        if ((currentSamples - samplesPerBeat) <= 0 && (currentSamples - samplesPerBeat) > (-onBeatOffset))
        {
            if (lastActionOnBeatCounter <= ComboOvershootValue)
            {
                lastActionOnBeatCounter++;
            }
            return true;
        }

        if (currentSamples >= 0 && (currentSamples <= onBeatOffset))
        {
            if (lastActionOnBeatCounter <= ComboOvershootValue)
            {
                lastActionOnBeatCounter++;
            }
            return true;
        }

        return false;
    }
}
