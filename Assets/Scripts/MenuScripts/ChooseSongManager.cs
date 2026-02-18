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
    [SerializeField] private CentralizedValues values;
    [SerializeField] private Button startGame_Button;
    [SerializeField] private Button return_Button;
    [SerializeField] private Button confirmSong_Button;
    [SerializeField] private Button resetSong_Button;

    private string songDictionaryFileName = "SongDataDictionary.json"; //hard coded, never change
    private string songDictionaryPath; //hard coded, never change

    [SerializeField] private TMP_Dropdown chooseYourSong_Dropdown;  //still needs some prettyness
    [SerializeField] private float timeTillGameStarts = 0.5f;
    [SerializeField] private SongData songData;
    private SongData tempSongData;


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
        return_Button.onClick.AddListener(ClearDropdownMenu);
        startGame_Button.onClick.AddListener(OnStartGameButton_Clicked);

        GetDataFromPersistentFolder();
        TryGetValuesFromSongDataDictionary();
        TryFindSongByFileNameInSongDataDictionary();

        if (songDictionary.Count > 0)
            chooseYourSong_Dropdown.interactable = true;
    }

    private void ClearDropdownMenu()
    {
        chooseYourSong_Dropdown.ClearOptions();
    }

    private void ResetSong()
    {
        chooseYourSong_Dropdown.interactable = true;
        confirmSong_Button.interactable = true;
        startGame_Button.interactable = false;
        ResetSongData();
    }

    private void ResetSongData()
    {
        songData.EraseData();
    }

    private void TryFindSongByFileNameInSongDataDictionary(int _ = 0)
    {
        if (songDictionary.Count > 0 && songDictionary.ContainsKey(chooseYourSong_Dropdown.options[chooseYourSong_Dropdown.value].text) == true)
            if (songDictionary.TryGetValue(chooseYourSong_Dropdown.options[chooseYourSong_Dropdown.value].text, out var temp) == true)
            {
                tempSongData = temp;
                chooseYourSong_Dropdown.captionText.text = chooseYourSong_Dropdown.options[chooseYourSong_Dropdown.value].text;
                confirmSong_Button.interactable = true;
            }

            else
                Debug.Log("Song specific values not found. Please add your desired song in the >> Import << Menu");

    }

    private void ConfirmSongChoice()
    {
        //cannot overwrite songData with tempSongData (sadly), have to overwrite every single value on it´s own
        songData.songName = tempSongData.songName;
        songData.BPM = tempSongData.BPM;
        songData.AsyncSamplesValue = tempSongData.AsyncSamplesValue;
        songData.BeatMultiplier = tempSongData.BeatMultiplier;
        songData.SamplesPerBeat = tempSongData.SamplesPerBeat;
        songData.SongSpecificVolume = tempSongData.SongSpecificVolume;

        confirmSong_Button.interactable = false;
        chooseYourSong_Dropdown.interactable = false;
        startGame_Button.interactable = true;
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
        if (File.Exists(songDictionaryPath))
        {
            var temp = File.ReadAllText(songDictionaryPath);
            songDictionary = JsonConvert.DeserializeObject<Dictionary<string, SongData>>(temp);
        }
    }
    private void OnDisable()
    {
        confirmSong_Button.onClick.RemoveListener(ConfirmSongChoice);
        chooseYourSong_Dropdown.onValueChanged.RemoveListener(TryGetValuesFromSongDataDictionary);
        resetSong_Button.onClick.RemoveListener(ResetSong);
        startGame_Button.onClick.RemoveListener(OnStartGameButton_Clicked);
    }

    private void OnStartGameButton_Clicked()
    {
        startGame_Button.interactable = false;
        return_Button.interactable = false ;
        confirmSong_Button.interactable = false;
        resetSong_Button.interactable = false;

        values.AllowInput_Bool = false;
        values.SetDefaultValues();
        StartCoroutine(OnStartGame(timeTillGameStarts));

    }

    private IEnumerator OnStartGame(float _timeTillGameStarts)
    {
        var temp = SceneManager.GetActiveScene();
        SceneManager.UnloadSceneAsync(temp);
        SceneManager.LoadSceneAsync(1);   // Menu Scene is 0, Game Scene is 1
        yield break;
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
