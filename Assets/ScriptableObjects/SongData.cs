using UnityEngine;

[CreateAssetMenu(fileName = "SongData", menuName = "Scriptable Objects/SongData")]
public class SongData : ScriptableObject
{
    public AudioClip Song; //trying to get rid of this... 
    public string songName = "";
    public int AsyncSamplesValue = 0;
    public int BeatMultiplier = 1;
    public float BPM = 0;
    public int SamplesPerBeat = 0;
}
