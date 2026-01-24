using SFB;
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

    //Load Data from file dialog into persitent data folder
    private bool canChooseMultipleFiles;
    private AudioSource source;
    [SerializeField] private AudioClip clip;
    private string currentAudioFileName;

    private string[] tempPath;

    [SerializeField] private SongData data;

    private string destPath = null;

    private void Awake()
    {
        canChooseMultipleFiles = false;
        destPath = Application.persistentDataPath + "/"; // + "/" is necessary to "hit" the correct folder
    }

    private void OnEnable()
    {
        view.SampleOffset_Slider.onValueChanged.AddListener(ShowAsyncSamples);
        view.SampleOffset_Slider.onValueChanged.AddListener(songPreview.OverrideCurrentAsyncSamples);
        view.BrowseFilesButton.onClick.AddListener(BrowseFiles);
        view.ConfirmBPM.onClick.AddListener(ConfirmBPM);
        view.songs_DropdownMenu.onValueChanged.AddListener(OnSongSelection);
    }
    private void OnSongSelection(int _ = 0)
    {
        
        //songDictionary = GetSongDataFromDictionary();

        var selectedSong = view.songs_DropdownMenu.options[view.songs_DropdownMenu.value].text;
        //if (songDictionary.TryGetValue(selectedSong, out SongData data) == true)
        //{
        //    view.tmp_InputFieldBPM.text = data.songBPM.ToString();
        //    view.tmp_InputFieldAsyncValue.text = data.songAsyncValue.ToString();
        //}
        AssignAudioFile(selectedSong);
        ArrangeBPMInputField();
    }
    private void ConfirmBPM()
    {
        if (float.TryParse(view.BPMInput.text, out float output))
        {
            data.BPM = output;
            ArrangeAsyncSlider();
        }

        else
            Debug.Log("Invalid input for BPM input field");
    }

    private void ConfirmAsyncValue()
    {
        ArrangePreviewButton();
    }

    private void ArrangePreviewButton()
    {

    }

    private void BrowseFiles()
    {
        //pauses mainthread, which is good
        tempPath = StandaloneFileBrowser.OpenFilePanel(view.Message, "", "", canChooseMultipleFiles);  //stay with false to not 
        if (tempPath.Length > 0)
            LoadFileIntoPersistentDataFolder(tempPath[0]);
        GetDataFromPersistentFolder();
    }

    private void LoadFileIntoPersistentDataFolder(string _path) // whole logic for loading files into the persistent data folder
    {
        if (File.Exists(destPath + Path.GetFileName(_path)))
            Debug.Log("File already exists in destination folder");

        else if (File.Exists(_path) == true)
        {
            File.Copy(_path, destPath + Path.GetFileName(_path));
            Debug.Log("File successfully loaded into persistent data folder");
            view.YourSong_Name.text = Path.GetFileName(_path);
            currentAudioFileName = view.YourSong_Name.text;
            view.songs_DropdownMenu.ClearOptions();
        }

        else
            Debug.Log("No file choosen");
    }

    private void AssignAudioFile(string _fileName)
    {
        //load song into beattracking_preview
        string path = destPath + _fileName;
        string uri = "file://" + path;

        StartCoroutine(LoadCustomSong(uri));
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

    private void ArrangeAsyncSlider()
    {
        view.AsyncValue_GO.gameObject.SetActive(true);
        view.SampleOffset_Slider.minValue = -(int)(songPreview.samplesPerBeat_Preview * 0.5f);
        view.SampleOffset_Slider.maxValue = (int)(songPreview.samplesPerBeat_Preview * 0.5f);
        view.SampleOffset_Slider.wholeNumbers = true;
    }

    private void ShowAsyncSamples(float _value)
    {
        view.ShowAsyncSamples_Text.text = "" + view.SampleOffset_Slider.value;
    }

    private IEnumerator LoadCustomSong(string uri)
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(uri, AudioType.WAV))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log("Could not load audio file");
            }
            else
            {
                data.Song = DownloadHandlerAudioClip.GetContent(www);
                //clip = DownloadHandlerAudioClip.GetContent(www);
            }
        }
    }
}
