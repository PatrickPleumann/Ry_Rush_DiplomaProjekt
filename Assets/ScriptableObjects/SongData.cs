using UnityEngine;

[CreateAssetMenu(fileName = "SongData", menuName = "Scriptable Objects/SongData")]
public class SongData : ScriptableObject
{
    public int AsyncSamplesValue;
    public int BeatMultiplier;
    public AudioClip Song;
    public float BPM;
}
