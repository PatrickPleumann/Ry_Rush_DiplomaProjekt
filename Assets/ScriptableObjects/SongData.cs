using UnityEngine;

[CreateAssetMenu(fileName = "SongData", menuName = "Scriptable Objects/SongData")]
public class SongData : ScriptableObject
{
    public string songName = "";
    public int AsyncSamplesValue = 0;
    public int BeatMultiplier = 1;
    public float BPM = 0;
    public int SamplesPerBeat = 0;

    public void EraseData()
    {
        songName = "";
        AsyncSamplesValue = 0;
        BeatMultiplier = 1;
        BPM = 0;
        SamplesPerBeat = 0;
    }
}
