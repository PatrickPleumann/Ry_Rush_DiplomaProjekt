using SFB;
using System;
using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class ImportManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SongPreviewBeatTracking songPreview;
    [SerializeField] private ImportManager_UX view;
    [SerializeField] private GameObject dropdown;
    [SerializeField] private SongDataDictionary songDataDictionary;

    [Header("Calculation relevant")]
    private float bpm;
    private int samplesPerBeat;

    [Header("File browsing values")]
    ExtensionFilter[] extensions = { new ExtensionFilter("Songfiles", "wav") }; // only .wav files so far.
    private bool canChooseMultipleFiles;
    [SerializeField] private SongData data;

    private string destPath = null;

    private void Awake()
    {
        
        canChooseMultipleFiles = false;
        destPath = Application.persistentDataPath + "/"; // + "/" is necessary to "hit" the correct folder
    }

    private void AddAllListener()
    {
        view.SampleOffset_Slider.onValueChanged.AddListener(ShowAsyncSamples);
        view.SongSpecificVolume_Slider.onValueChanged.AddListener(ChangeVolume);
        view.SampleOffset_Slider.onValueChanged.AddListener(songPreview.OverrideCurrentAsyncSamples);

        view.LoadYourSong_Button.onClick.AddListener(BrowseFilesForSong);
        view.ConfirmSong_Button.onClick.AddListener(ConfirmCurrentSong);

        view.ConfirmBPM_Button.onClick.AddListener(ConfirmBPM);
        view.PlayPreview_Button.onClick.AddListener(StartPreview);

        view.SaveSongValues_Button.onClick.AddListener(SafeSongValuesIntoDictionary);
        view.songs_DropdownMenu.onValueChanged.AddListener(DisplaySongChoice);

        view.ResetValues_Button.onClick.AddListener(ResetAllValues);
        view.Back_Button.onClick.AddListener(ResetAllValues);
    }

    private void RemoveAllListeners()
    {
        view.SampleOffset_Slider.onValueChanged.RemoveListener(ShowAsyncSamples);
        view.SongSpecificVolume_Slider.onValueChanged.RemoveListener(ChangeVolume);
        view.SampleOffset_Slider.onValueChanged.RemoveListener(songPreview.OverrideCurrentAsyncSamples);

        view.LoadYourSong_Button.onClick.RemoveListener(BrowseFilesForSong);
        view.ConfirmSong_Button.onClick.RemoveListener(ConfirmCurrentSong);

        view.ConfirmBPM_Button.onClick.RemoveListener(ConfirmBPM);
        view.PlayPreview_Button.onClick.RemoveListener(StartPreview);

        view.SaveSongValues_Button.onClick.RemoveListener(SafeSongValuesIntoDictionary);
        view.songs_DropdownMenu.onValueChanged.RemoveListener(DisplaySongChoice);

        view.ResetValues_Button.onClick.RemoveListener(ResetAllValues);
        view.Back_Button.onClick.RemoveListener(ResetAllValues);

    }

    private void OnEnable()
    {
        AddAllListener();

        ClearDropDown();
        GetDataFromPersistentFolder();

        if (view.songs_DropdownMenu.options.Count > 0 && view.songs_DropdownMenu.value >= 0)
        {
            view.YourSong_Name.text = view.songs_DropdownMenu.options[0].text;
            view.songs_DropdownMenu.captionText.text = view.songs_DropdownMenu.options[0].text;
        }
    }

    private void OnDisable()
    {
        RemoveAllListeners();
    }


    private void SafeSongValuesIntoDictionary() 
    {
        FillSongData();
        songDataDictionary.SaveEntry(view.songs_DropdownMenu.options[view.songs_DropdownMenu.value].text, data);
        songDataDictionary.SafeDictionaryAsJson();
    }


    private void StartPreview()
    {
        view.PlayPreview_Button.interactable = false;
        FillSongData();
        songPreview.AssignSongDataValuesToPreview(data);
    }

    private void ChangeVolume(float _volume)
    {
        songPreview.source_song.volume = _volume;
        songPreview.source_metronome.volume = _volume;
    }

    private void FillSongData() //method has to be called earlier
    {
        data.songName = view.songs_DropdownMenu.options[view.songs_DropdownMenu.value].text;
        data.BPM = bpm;
        data.SamplesPerBeat = GetSamplesPerBeat();
        data.AsyncSamplesValue = (int)view.SampleOffset_Slider.value;
        data.BeatMultiplier = 1;
        data.SongSpecificVolume = view.SongSpecificVolume_Slider.value;
    }
    private void ConfirmCurrentSong()
    {
        var selectedSong = view.songs_DropdownMenu.options[view.songs_DropdownMenu.value].text;
        AssignAudioFile(selectedSong);
        dropdown.SetActive(false); // into reset values
        view.ConfirmSong_Button.interactable = false;
        view.LoadYourSong_Button.interactable = false; // into reset values
        ArrangeBPMInputField();
    }
    private void DisplaySongChoice(int _ = 0)
    {
        view.YourSong_Name.text = view.songs_DropdownMenu.options[view.songs_DropdownMenu.value].text;
    }
    private void ClearDropDown()
    {
        view.songs_DropdownMenu.ClearOptions();
    }

    private void ResetAllValues()
    {
        data.EraseData();
        songPreview.EraseData();
        songPreview.source_song.Stop();
        dropdown.SetActive(true);
        view.LoadYourSong_Button.interactable = true;
        view.ConfirmSong_Button.interactable = true;

        view.BPM_GO.SetActive(false);
        view.BPMInput_InputField.interactable = true;
        view.BPMInput_InputField.text = "Enter BMP here... 30 - 200";
        view.ConfirmBPM_Button.interactable = true;

        view.AsyncValue_GO.gameObject.SetActive(false);
        view.PlayPreview_Button.interactable = true;
        view.SampleOffset_Slider.minValue = 0;
        view.SampleOffset_Slider.maxValue = 1;
        view.SampleOffset_Slider.value = 0;
        view.SampleOffset_Slider.wholeNumbers = true;
    }

    public int GetSamplesPerBeat()
    {
        if (bpm != 0 && songPreview.clip != null)
        {
            return (int)(songPreview.clip.frequency * ((60 / bpm) * 1));
        }
        else return 0;
    }
    private void ConfirmBPM()
    {
        if (float.TryParse(view.BPMInput_InputField.text, out float output) && output < 201 && output > 30)
        {
            FillSongData();
            bpm = output;
            samplesPerBeat = GetSamplesPerBeat();
            ArrangeAsyncSlider(samplesPerBeat);
            view.BPMInput_InputField.interactable = false; // into reset
            view.ConfirmBPM_Button.interactable = false;   // into reset
        }

        else
        {
            view.BPMInput_InputField.text = "Enter BMP here... 30 - 200";
            Debug.Log("Invalid input for BPM input field");
        }
    }

    private void BrowseFilesForSong()
    {
        dropdown.SetActive(true);
        //pauses mainthread, which is good
        var path = StandaloneFileBrowser.OpenFilePanel(view.Message, "", extensions, canChooseMultipleFiles);
        if (path.Length > 0)
            LoadFileIntoPersistentDataFolder(path[0]);
    }

    private void LoadFileIntoPersistentDataFolder(string _path) // whole logic for loading files into the persistent data folder
    {
        if (File.Exists(destPath + Path.GetFileName(_path)))
            Debug.Log("File already exists in destination folder");

        else if (File.Exists(_path) == true)
        {
            ClearDropDown();
            File.Copy(_path, destPath + Path.GetFileName(_path));
            Debug.Log("File successfully loaded into persistent data folder");
            view.YourSong_Name.text = Path.GetFileName(_path);
            view.songs_DropdownMenu.captionText.text = view.YourSong_Name.text;
            GetDataFromPersistentFolder();
        }

        else
        
            
            Debug.Log("No file chosen");
        
    }


    private void GetDataFromPersistentFolder() //gets all files from persistent data path and adds a dropdown option for each.
    {
        var info = new DirectoryInfo(Application.persistentDataPath);
        var fileInfo = info.GetFiles("*.wav");
        foreach (var file in fileInfo)
        {
            var data = new TMP_Dropdown.OptionData();
            data.text = file.Name;
            view.songs_DropdownMenu.options.Add(data);
        }
    }

    private void ArrangeBPMInputField()
    {
        view.BPM_GO.gameObject.SetActive(true);
    }

    private void ArrangeAsyncSlider(float _samplesPerBeat)
    {
        view.AsyncValue_GO.gameObject.SetActive(true);
        view.SampleOffset_Slider.minValue = -(_samplesPerBeat * 0.5f);
        view.SampleOffset_Slider.maxValue = (_samplesPerBeat * 0.5f);
        view.SampleOffset_Slider.wholeNumbers = true;
    }

    private void ShowAsyncSamples(float _value)
    {
        //maybe cool would be to implement a stepsize multiplier, which allows the user to use different stepsizes while changing async values
        view.ShowAsyncSamples_Text.text = "" + view.SampleOffset_Slider.value;
    }

    private void AssignAudioFile(string _fileName)
    {
        string path = destPath + _fileName;
        string uri = "file://" + path;

        StartCoroutine(LoadCustomSong(uri));
    }

    private IEnumerator LoadCustomSong(string uri)
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(uri, AudioType.WAV))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
                Debug.Log("Could not load audio file");

            else
            {
                songPreview.clip = DownloadHandlerAudioClip.GetContent(www);
            }
        }
    }
}
