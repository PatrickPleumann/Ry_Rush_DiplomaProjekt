using UnityEngine;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using NUnit.Framework.Constraints;

public class SongDataDictionary : MonoBehaviour
{
    [SerializeField] private string songDictionaryFileName = "SongDataDictionary.json";

    public Dictionary<string, SongData> songDictionary;

    private string songDictionaryPath;
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

    public void CreateNewEntry(string _key, SongData _value)
    {
        if (songDictionary.ContainsKey(_key) == true)
            Debug.Log("Song entry with this name already exists");

        else
        {
            songDictionary.Add(_key, _value);
            Debug.Log("New song entry successful");
        }
    }
    private void GetSongDataDictionaryFromJson()
    {
        var temp = File.ReadAllText(songDictionaryPath);

        JsonConvert.DeserializeObject<Dictionary<string, SongData>>(temp);
    }


    public void SafeDictionaryAsJson()
    {
        var json = JsonConvert.SerializeObject(songDictionary);
        File.WriteAllText(songDictionaryPath, json);
    }
}
