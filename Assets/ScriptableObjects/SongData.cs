using UnityEngine;

[CreateAssetMenu(fileName = "SongData", menuName = "Scriptable Objects/SongData")]
public class SongData : ScriptableObject
{
    public AudioClip Song;
    public int AsyncSamplesValue;
    public int BeatMultiplier = 1;
    public float BPM;
    public int SamplesPerBeat;
}
