using SFB;
using System.Collections;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class ImportManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SongPreviewBeatTracking songPreview;
    [SerializeField] private ImportManager_UX view;
    [SerializeField] private GameObject dropdown;

    private AudioClip clip;
    private float bpm;
    private int asyncValue;
    private int samplesPerBeat;

    //Load Data from file dialog into persitent data folder
    private bool canChooseMultipleFiles;

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

        view.BrowseFiles_Button.onClick.AddListener(BrowseFiles);
        view.ConfirmSong_Button.onClick.AddListener(Assign);

        view.ConfirmBPM_Button.onClick.AddListener(ConfirmBPM);
        view.PlayPreview_Button.onClick.AddListener(StartPreview);

        ClearDropDown();
        GetDataFromPersistentFolder();
    }

    private void StartPreview()
    {
        songPreview.AssignSongDataValuesToPreview(data);
    }
    private void Assign()
    {
        var selectedSong = view.songs_DropdownMenu.options[view.songs_DropdownMenu.value].text;
        AssignAudioFile(selectedSong);
        dropdown.SetActive(false);
        ArrangeBPMInputField();
    }

    private void ClearDropDown()
    {
        view.songs_DropdownMenu.ClearOptions();
    }
    private void OnSongSelection(int _ = 0)
    {

        //songDictionary = GetSongDataFromDictionary();


        //if (songDictionary.TryGetValue(selectedSong, out SongData data) == true)
        //{
        //    view.tmp_InputFieldBPM.text = data.songBPM.ToString();
        //    view.tmp_InputFieldAsyncValue.text = data.songAsyncValue.ToString();
        //}


        //ArrangeBPMInputField();
    }

    public float GetSamplesPerBeat()
    {
        if (bpm != 0 && clip != null)
        {
            return (clip.frequency * ((60 / bpm) * 1));
        }
        else return 0;
    }
    private void ConfirmBPM()
    {
        if (float.TryParse(view.BPMInput_InputField.text, out float output))
        {
            bpm = output;
            data.BPM = bpm;
            data.Song = clip;
            data.BeatMultiplier = 1;
            ArrangeAsyncSlider(GetSamplesPerBeat());
        }

        else
            Debug.Log("Invalid input for BPM input field");
    }

    private void ConfirmAsyncValue()
    {
        data.AsyncSamplesValue = (int)view.SampleOffset_Slider.value;


        ArrangePreviewButton();
    }

    private void ArrangePreviewButton()
    {

    }

    private void BrowseFiles()
    {
        dropdown.SetActive(true);
        //pauses mainthread, which is good
        var path = StandaloneFileBrowser.OpenFilePanel(view.Message, "", "", canChooseMultipleFiles);  //stay with false to not 
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
            GetDataFromPersistentFolder();
        }

        else
            Debug.Log("No file choosen");
    }

    private void AssignAudioFile(string _fileName)
    {
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

    private void ArrangeAsyncSlider(float _samplesPerBeat)
    {
        view.AsyncValue_GO.gameObject.SetActive(true);
        view.PlayPreview_Button.gameObject.SetActive(true);
        view.SampleOffset_Slider.minValue = -(_samplesPerBeat * 0.5f);
        view.SampleOffset_Slider.maxValue = (_samplesPerBeat * 0.5f);
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
                Debug.Log("Could not load audio file");


            else
            {
                //data.Song = DownloadHandlerAudioClip.GetContent(www);
                data.Song = DownloadHandlerAudioClip.GetContent(www);
            }
        }
    }
}
