using UnityEngine;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public class SongDataDictionary : MonoBehaviour
{
    private string songDictionaryFileName = "SongDataDictionary.json"; //hard coded, never change
    private string songDictionaryPath; //hard coded, never change

    public Dictionary<string, SongData> songDictionary;

    private void Awake()
    {
        songDictionaryPath = Application.persistentDataPath + "/" + songDictionaryFileName;
        songDictionary = new();
    }

    private void Start()
    {
        if (File.Exists(songDictionaryPath) == true)
        {
            GetSongDataDictionaryFromJson();
            Debug.Log("Song dictionary successfully loaded");
        }
    }

    public void SaveEntry(string _key, SongData _value) // it seems like one game breaking bug was that SongData _values was handled as a ref type and overwritten for every value
    {
        if (songDictionary.ContainsKey(_key) == true)
        {
            songDictionary[_key].songName = _value.songName;
            songDictionary[_key].AsyncSamplesValue = _value.AsyncSamplesValue;
            songDictionary[_key].BeatMultiplier = _value.BeatMultiplier;
            songDictionary[_key].BPM = _value.BPM;
            songDictionary[_key].SamplesPerBeat = _value.SamplesPerBeat;
            songDictionary[_key].SongSpecificVolume = _value.SongSpecificVolume;
            Debug.Log("Song values from song: " + _key + " overwritten.");
        }
        else
        {
            songDictionary.Add(_key, new SongData());
            songDictionary[_key].songName = _value.songName;
            songDictionary[_key].AsyncSamplesValue = _value.AsyncSamplesValue;
            songDictionary[_key].BeatMultiplier = _value.BeatMultiplier;
            songDictionary[_key].BPM = _value.BPM;
            songDictionary[_key].SamplesPerBeat = _value.SamplesPerBeat;
            songDictionary[_key].SongSpecificVolume = _value.SongSpecificVolume;
            Debug.Log("New song entry successful");
        }
    }

    private void GetSongDataDictionaryFromJson()
    {
        var temp = File.ReadAllText(songDictionaryPath);
        songDictionary = JsonConvert.DeserializeObject<Dictionary<string, SongData>>(temp);
    }

    public void SafeDictionaryAsJson()
    {
        var json = JsonConvert.SerializeObject(songDictionary);
        File.WriteAllText(songDictionaryPath, json);
    }

}
