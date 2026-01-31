using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;


public class BeatTracking : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public PlayerController controller;
    [SerializeField] public Scoreboard_UI scoreboard;
    [SerializeField] public AudioSource source;
    [SerializeField] private SongData songData;

    [Header("Gameplay Values")]
    [SerializeField] private float timeTillSongStarts = 3f;

    [Header("Events")]
    public UnityAction onBeatInvoke;

    [Header("Beat Tracking Values")]

    [SerializeField] public int asyncValue;
    [SerializeField] public float beatOffsetMultiplier;
    [SerializeField] public int samplesPerBeat = 0;
    [SerializeField] public float bpm = 0f;
    [SerializeField] public float lastActionOnBeatTimer;

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
    private string destPath;


    private void Awake()
    {
        destPath = Application.persistentDataPath + "/";
        source = GetComponent<AudioSource>();

        AssignAudioFile(songData.songName);
        bpm = songData.BPM;
        beatMultiplier = songData.BeatMultiplier;
        samplesPerBeat = songData.SamplesPerBeat;
        asyncValue = songData.AsyncSamplesValue;

        onBeatOffset = (int)(samplesPerBeat * beatOffsetMultiplier) * beatMultiplier;
        samplesPerBeat_UI = samplesPerBeat + onBeatOffset;
    }


    private void Start()
    {
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

    private void ValidateSongValues()
    {

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

            if (controller.LastActionOnBeat == true)
                controller.LastActionOnBeat = false;

            else if (controller.LastActionOnBeat == false)
                scoreboard.DecreaseComboCounter();
        }

        if ((currentSamples_UI / samplesPerBeat_UI) >= 1) // for testing
            currentSamples_UI = currentSamples;


        if ((currentSamples - samplesPerBeat) <= 0 && (currentSamples - samplesPerBeat) > (-onBeatOffset))
            return true;


        if (currentSamples > 0 && (currentSamples <= onBeatOffset))
            return true;


        return false;
    }

    public bool Return_IsOnBeat()
    {
        return isOnBeat;
    }

    private void AssignAudioFile(string _fileName)
    {
        string path = destPath + _fileName;
        string uri = "file://" + path;

        StartCoroutine(LoadCustomSong(uri));
    }
    private IEnumerator LoadCustomSong(string uri)
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(uri, AudioType.WAV))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
                Debug.Log("Could not load audio file");

            else
            {
                source.clip = DownloadHandlerAudioClip.GetContent(www);
            }
        }
    }
}
