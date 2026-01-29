using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChooseSongManager : MonoBehaviour
{
    [SerializeField] private Button startGame_Button;
    [SerializeField] private Button return_Button;
    [SerializeField] private Button confirmSong_Button;
    [SerializeField] private Button resetSong_Button;

    private string songDictionaryFileName = "SongDataDictionary.json"; //hard coded, never change
    private string songDictionaryPath; //hard coded, never change

    [SerializeField] private TMP_Dropdown chooseYourSong_Dropdown;  //still needs some prettyness
    [SerializeField] private float timeTillGameStarts = 0.5f;
    [SerializeField] private SongData songData;
    

    private Dictionary<string, SongData> songDictionary;

    private void Awake()
    {
        songDictionaryPath = Application.persistentDataPath + "/" + songDictionaryFileName;
        songDictionary = new();


    }
    private void OnEnable()
    {
        confirmSong_Button.onClick.AddListener(ConfirmSongChoice);
        chooseYourSong_Dropdown.onValueChanged.AddListener(TryFindSongByFileNameInSongDataDictionary);
        resetSong_Button.onClick.AddListener(ResetSong);

        GetDataFromPersistentFolder();
        TryGetValuesFromSongDataDictionary();
    }

    private void ResetSong()
    {
        //confirmSong_button.interactable = false; 
    }

    private void TryFindSongByFileNameInSongDataDictionary(int _ = 0)
    {
        if (songDictionary.ContainsKey(chooseYourSong_Dropdown.options[chooseYourSong_Dropdown.value].text) == true)
            if (songDictionary.TryGetValue(chooseYourSong_Dropdown.options[chooseYourSong_Dropdown.value].text, out var temp))
            {
                //cannot overwrite songData with temp (sadly), have to overwrite every single value on it´s own
                songData.songName = temp.songName;
                songData.BPM = temp.BPM;
                songData.AsyncSamplesValue = temp.AsyncSamplesValue;
                songData.BeatMultiplier = temp.BeatMultiplier;
                songData.SamplesPerBeat = temp.SamplesPerBeat;

                //confirmSong_button.interactable = true 
            }

            else
                Debug.Log("Song specific values not found. Please add your desired song in the >> Import << Menu");
        
    }

    private void ConfirmSongChoice()
    {
        Debug.Log("Song was confirmed, nice!");
    }

    private void Start()
    {
        if (chooseYourSong_Dropdown.options.Count > 0)
        {
            chooseYourSong_Dropdown.captionText.text = chooseYourSong_Dropdown.options[0].text;
        }
    }

    private void TryGetValuesFromSongDataDictionary(int _ = 0)
    {
        var temp = File.ReadAllText(songDictionaryPath);
        songDictionary = JsonConvert.DeserializeObject<Dictionary<string, SongData>>(temp);
    }


    private void OnDisable()
    {
        confirmSong_Button.onClick.RemoveListener(ConfirmSongChoice);
        chooseYourSong_Dropdown.onValueChanged.RemoveListener(TryGetValuesFromSongDataDictionary);
        resetSong_Button.onClick.RemoveListener(ResetSong);
    }

    private void OnStartGameButton_Clicked()
    {
        StartCoroutine(OnStartGame(timeTillGameStarts));
    }

    private IEnumerator OnStartGame(float _timeTillGameStarts)
    {

        var temp = SceneManager.GetActiveScene();
        SceneManager.UnloadSceneAsync(temp);
        yield return new WaitForSeconds(_timeTillGameStarts);
        SceneManager.LoadSceneAsync(1);   // Menu Scene is 0, Game Scene is 1
    }


    private void GetDataFromPersistentFolder() //gets all files from persistent data path and adds a dropdown option for each.
    {
        var info = new DirectoryInfo(Application.persistentDataPath);
        var fileInfo = info.GetFiles("*.wav");
        foreach (var file in fileInfo)
        {
            var data = new TMP_Dropdown.OptionData();
            data.text = file.Name;
            chooseYourSong_Dropdown.options.Add(data);
        }
    }
}
