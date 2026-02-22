using Cysharp.Threading.Tasks;
using System.Collections;
using System.Threading.Tasks;
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
    [SerializeField] private GameObject SCOREBOARD;
    [SerializeField] private CentralizedValues values;

    [Header("Gameplay Values")]
    [SerializeField] private float timeTillSongStarts = 3f;

    [Header("Events & Actions")]
    [HideInInspector] public UnityAction onBeatInvoke;
    [HideInInspector] public UnityAction OnSongLoaded;
    private UnityAction OnSongEnded;

    [SerializeField] private int estimatedBeatsPerTrack = 11; // dirty magic number just for additional saftey, gets overwritten anyway
    [SerializeField] private int remainingBeatsBeforeGameEnds = 10; //hard coded but depends on the fadeout of the song when it ends
    [SerializeField] private int currentBeatsInTrack = 0;

    [Header("Properties")]
    public int CurrentBeatsInTrack
    {
        get => currentBeatsInTrack;

        set
        {
            if (currentBeatsInTrack != value)
            {
                currentBeatsInTrack = value;
                if (currentBeatsInTrack > (estimatedBeatsPerTrack - remainingBeatsBeforeGameEnds))
                    OnSongEnded.Invoke();
            }
        }
    }

    [Header("Beat Tracking Values")]

    [SerializeField] public int asyncValue;
    [SerializeField] public float beatOffsetMultiplier;
    [SerializeField] public int samplesPerBeat = 0;
    [SerializeField] public float bpm = 0f;


    public int currentSamples = 0;
    public int currentTimeSamplesMin = 0;
    public int currentTimeSamplesMax = 0;
    public bool isOnBeat = false;
    public int onBeatOffset;
    public int beatCounter = 1;

    [Header("Beattracking Values for UI Visualization")]
    public int currentSamples_UI = 0; // into CV_SO
    public int samplesPerBeat_UI = 0; // into CV_SO


    [SerializeField] private int ComboOvershootValue = 3;
    private bool songStarted = false;

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
        source.volume = songData.SongSpecificVolume;

        values.AllowInput = true;

        onBeatOffset = (int)(samplesPerBeat * beatOffsetMultiplier) * beatMultiplier;
        samplesPerBeat_UI = samplesPerBeat + onBeatOffset;
    }

    private void OnEnable()
    {
        OnSongLoaded += GetEstimatesBeatsPerTrack;
        OnSongEnded += SongEnds;
    }
    private void OnDisable()
    {
        OnSongLoaded -= GetEstimatesBeatsPerTrack;
        OnSongEnded -= SongEnds;
    }
    private void GetEstimatesBeatsPerTrack() // works
    {

        if (source.clip != null)
        {
            var temp = source.clip.length;
            estimatedBeatsPerTrack = (int)(temp * (bpm / 60));
        }
    }
    private void Start()
    {
        values.TimeBetweenBeats = Utility.FloorFloatToTwoDigits(60 / songData.BPM);
        currentSamples += asyncValue;
        currentSamples_UI = currentSamples;
        currentSamples_UI += onBeatOffset;

        StartSong();
    }
    private void Update()
    {
        if (songStarted == true)
            isOnBeat = CheckForNewBeat();
    }

    private async void StartSong()
    {
       await  StartSongDelayed(timeTillSongStarts);
    }

    private async UniTask StartSongDelayed(float _timeTillSongStarts)
    {
        await UniTask.Delay((int)(_timeTillSongStarts * 1000));
        songStarted = true;
        source.Play();
    }


    /// <summary>
    /// Checks every frame if a new beat just happened.
    /// </summary>
    /// <returns></returns>
    public bool CheckForNewBeat()
    {
        currentSamples += source.timeSamples - lastFrameSamples;
        currentSamples_UI += source.timeSamples - lastFrameSamples;
        lastFrameSamples = source.timeSamples;

        if ((currentSamples / samplesPerBeat) >= 1)
        {
            currentSamples -= samplesPerBeat;
            //onBeatInvoke.Invoke();
            CurrentBeatsInTrack = CurrentBeatsInTrack + 1; //it doesn´t seem that CurrentBeatsInTrack++ works as expected with the property

            if (values.LastActionOnBeat_Bool == false)
            {
                if (values.CurrentComboOvershoot_Value > 0)
                    values.CurrentComboOvershoot_Value = values.CurrentComboOvershoot_Value - 1;

                else
                    values.CurrentCombo_Value = values.CurrentCombo_Value - 1;
            }

            else
                values.LastActionOnBeat_Bool = false;
        }

        if ((currentSamples_UI / samplesPerBeat_UI) >= 1) // for testing // maybe a second logic for a second visualizer 
            currentSamples_UI = currentSamples;

        if ((currentSamples - samplesPerBeat) <= 0 && (currentSamples - samplesPerBeat) > (-onBeatOffset))
            return true;

        if (currentSamples > 0 && (currentSamples <= onBeatOffset))
            return true;

        return false;
    }

    /// <summary>
    /// Returns constantly if an action at this frame is on beat or not.
    /// </summary>
    /// <returns></returns>
    public bool Return_IsOnBeat()
    {
        return isOnBeat; // into CV_SO 
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
                OnSongLoaded.Invoke();
            }
        }
    }

    private void SongEnds()
    {
        songStarted = false;
        values.AllowInput = false;
        values.onSessionEnds.Invoke();
        Debug.Log("GAME OVER");
    }
}
