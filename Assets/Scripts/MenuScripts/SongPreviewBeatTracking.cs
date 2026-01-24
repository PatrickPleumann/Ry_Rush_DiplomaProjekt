using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class SongPreviewBeatTracking : MonoBehaviour
{
    [SerializeField] private float bpm_Preview;

    [SerializeField] public AudioSource source_song;
    [SerializeField] private AudioSource source_metronome;

    public AudioClip clip;
    [SerializeField] private AudioClip metronome;

    [SerializeField] private int currentSamples_Preview;
    [SerializeField] private int asyncValues_Preview;
    [SerializeField] public int samplesPerBeat_Preview;
    [SerializeField] private int beatMultiplier_Preview = 1;

    private bool songPreviewPlaying = false;

    private int lastFrameSamples_Preview;
    public UnityAction onBeat;

    
    public void OverrideCurrentAsyncSamples(float _value)
    {
        asyncValues_Preview = (int)_value;
    }

    private void Update()
    {
        if (songPreviewPlaying == true)
        {
            BeatTracking_Preview();
        }
    }

    //somewhere here  a button which assigns to beatMultiplier the value 2 (for half beat stuff)
    public void AssignSongDataValuesToPreview()
    {
        var temp = GetComponents<AudioSource>(); // still unsafe but it works
        source_song = temp[0];
        source_metronome = temp[1];
        
        samplesPerBeat_Preview = (int)(source_song.clip.frequency * (60f / bpm_Preview) * beatMultiplier_Preview);
    }

    public void PlayTestSong() // only allow after confirmed values
    {
        source_song.Play();
        songPreviewPlaying = true;
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
        if (((currentSamples_Preview + asyncValues_Preview)  / (samplesPerBeat_Preview)) >= 1)
        {
            currentSamples_Preview -= (samplesPerBeat_Preview - asyncValues_Preview); 
            onBeat.Invoke();
        }
    }

    private void PlayMetronome()
    {
        source_metronome.Play();
    }
}
