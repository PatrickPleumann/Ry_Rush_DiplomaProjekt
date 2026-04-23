using Cysharp.Threading.Tasks;
using System.Collections;
using System.Threading;
using Unity.VisualScripting;
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
    [SerializeField] private int remainingBeatsBeforeDisAllowSlowMotion = 24;
    [SerializeField] private int currentBeatsInTrack = 0;

    CancellationTokenSource cts = new();


    [Header("Properties")]
    public int CurrentBeatsInTrack
    {
        get => currentBeatsInTrack;

        set
        {
            if (currentBeatsInTrack != value)
            {
                currentBeatsInTrack = value;

                if (currentBeatsInTrack > (estimatedBeatsPerTrack - remainingBeatsBeforeDisAllowSlowMotion))
                    values.DisAllowSlowMotion.Invoke();

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
    public bool IsOnBeat = false;
    public int OnBeatOffset;
    public int BeatCounter = 1;

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
        values.SetDefaultValues();
        destPath = Application.persistentDataPath + "/";
        source = GetComponent<AudioSource>();

        AssignAudioFile(songData.songName);
        bpm = songData.BPM;
        beatMultiplier = songData.BeatMultiplier;
        samplesPerBeat = songData.SamplesPerBeat;
        asyncValue = songData.AsyncSamplesValue;
        source.volume = songData.SongSpecificVolume;

        values.AllowInput = true;

        OnBeatOffset = (int)(samplesPerBeat * beatOffsetMultiplier) * beatMultiplier;
        samplesPerBeat_UI = samplesPerBeat + OnBeatOffset;
    }

    private void OnEnable()
    {
        OnSongLoaded += GetEstimatesBeatsPerTrack;
        OnSongEnded += SongEnds;
        values.OnPlayerDeath.AddListener(DecreaseSourceVolume);
    }
    private void OnDisable()
    {
        OnSongLoaded -= GetEstimatesBeatsPerTrack;
        OnSongEnded -= SongEnds;
        values.OnPlayerDeath.RemoveListener(DecreaseSourceVolume);
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
        var temp = (float)(60 / songData.BPM);
        values.TimeBetweenBeats = Utility.FloorFloatToTwoDigits(temp);
        Debug.Log(values.TimeBetweenBeats);
        currentSamples += asyncValue;
        currentSamples_UI = currentSamples;
        currentSamples_UI += OnBeatOffset;

        StartSong();
    }
    private void Update()
    {
        if (songStarted == true)
        {
            IsOnBeat = CheckForNewBeat();
            values.IsOnBeat = IsOnBeat;
        }
    }

    private async void StartSong()
    {
        await  StartSongDelayedAsync(timeTillSongStarts, cts.Token);
    }

    private async UniTask StartSongDelayedAsync(float _timeTillSongStarts, CancellationToken _token)
    {
        _token.ThrowIfCancellationRequested();
        await UniTask.Delay((int)(_timeTillSongStarts * 1000));
        songStarted = true;
        source.Play();
    }

    private async void DecreaseSourceVolume()
    {
        await DecreaseSourceVolumeAsync(cts.Token);
    }
    private async UniTask DecreaseSourceVolumeAsync(CancellationToken _token)
    {
        while (source != null && source.volume > 0.001f) // feels very good, so song fades out very long until it completely disappears
        {
            _token.ThrowIfCancellationRequested();
            source.volume -= Time.deltaTime;
            await UniTask.Delay((int)(Time.deltaTime * 1000 * (1 / source.volume)), true);
        }
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

        values.CurrentSamples = currentSamples;

        if ((currentSamples_UI / samplesPerBeat_UI) >= 1) // for testing // maybe a second logic for a second visualizer 
            currentSamples_UI = currentSamples;

        if ((currentSamples - samplesPerBeat) <= 0 && (currentSamples - samplesPerBeat) > (-OnBeatOffset))
            return true;

        if (currentSamples > 0 && (currentSamples <= OnBeatOffset))
            return true;

        return false;
    }

    /// <summary>
    /// Returns constantly if an action at this frame is on beat or not.
    /// </summary>
    /// <returns></returns>
    public bool Return_IsOnBeat()
    {
        return IsOnBeat; // into CV_SO 
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

            else if (www.result == UnityWebRequest.Result.Success)
            {
                source.clip = DownloadHandlerAudioClip.GetContent(www);
                OnSongLoaded.Invoke();
            }
        }
    }

    private void SongEnds()
    {
        values.SessionIsOver = true;
        songStarted = false;
        values.AllowInput = false;
        values.OnSessionEnds.Invoke();
        Debug.Log("GAME OVER");
    }

    private void OnDestroy()
    {
        cts.Cancel();
        cts.Dispose();
    }
}
