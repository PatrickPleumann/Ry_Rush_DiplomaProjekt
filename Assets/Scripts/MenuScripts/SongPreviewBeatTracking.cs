using System.Collections;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class SongPreviewBeatTracking : MonoBehaviour
{
    [SerializeField] private float bpm_Preview;

    [SerializeField] public AudioSource source_song;
    [SerializeField] public AudioSource source_metronome;

    [SerializeField] private AudioClip metronome;
    public AudioClip clip;

    [SerializeField] private int currentSamples_Preview = 0;
    [SerializeField] private int asyncValues_Preview = 0;
    [SerializeField] public int samplesPerBeat_Preview = 0;
    [SerializeField] private int beatMultiplier_Preview = 1;

    private bool songPreviewPlaying = false;

    private int lastFrameSamples_Preview;

    private void Update()
    {
        if (songPreviewPlaying == true)
        {
            BeatTracking_Preview();
        }
    }
    private void Start()
    {
        var temp = GetComponents<AudioSource>(); // unsafe but it works
        source_song = temp[0];
        source_metronome = temp[1];
        source_metronome.clip = metronome;
    }
    public void OverrideCurrentAsyncSamples(float _value)
    {
        asyncValues_Preview = (int)_value;
    }
    //somewhere here  a button which assigns to beatMultiplier the value 2 (for half beat stuff)
    public void AssignSongDataValuesToPreview(SongData _data, float _volume)
    {
        source_song.volume = _volume;
        source_song.clip = clip;

        bpm_Preview = _data.BPM;
        asyncValues_Preview = _data.AsyncSamplesValue;
        beatMultiplier_Preview = _data.BeatMultiplier;
        samplesPerBeat_Preview = _data.SamplesPerBeat;

        StartSong();
    }

    public void EraseData()
    {
        songPreviewPlaying = false;
        source_song.clip = null;

        currentSamples_Preview = 0;
        lastFrameSamples_Preview = 0;
        bpm_Preview = 0;
        asyncValues_Preview = 0;
        beatMultiplier_Preview = 1;
        samplesPerBeat_Preview = 0;
    }

    private async void StartSong()
    {
        songPreviewPlaying = true;
        source_song.volume = 0.2f;
        await Task.Delay(500); // hard coded magic num
        source_song.Play();
    }

    public void StopTestSong()
    {
        songPreviewPlaying = false;
        source_song.Stop();
    }

    private void BeatTracking_Preview()
    {
        currentSamples_Preview += source_song.timeSamples - lastFrameSamples_Preview;
        lastFrameSamples_Preview = source_song.timeSamples;
        if (((currentSamples_Preview + asyncValues_Preview)  / samplesPerBeat_Preview) >= 1)
        {
            currentSamples_Preview -= samplesPerBeat_Preview; 
            PlayMetronome();
        }
    }

    private void PlayMetronome()
    {
        source_metronome.Play();
    }
}
