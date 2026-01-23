using SFB;
using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ImportManager : MonoBehaviour
{
    //Load Data from file dialog into persitent data folder
    private bool canChooseMultipleFiles;
    private AudioSource source;
    private AudioClip clip;

    [SerializeField] private TMP_Text showAsyncSamplesText;
    [SerializeField] private Slider sampleOffsetSlider;
    [SerializeField] private Button openFileDialog;
    [SerializeField] private string message;

    private string destPath = null;

    private void Awake()
    {
        canChooseMultipleFiles = false;
        destPath = Application.persistentDataPath + "/"; // + "/" is necessary to "hit" the correct folder
    }

    private void OnEnable()
    {
        sampleOffsetSlider.onValueChanged.AddListener(ShowAsyncSamples);
        openFileDialog.onClick.AddListener(BrowseFiles);
    }



    private void BrowseFiles()
    {
        //pauses mainthread, which is good
        var temp = StandaloneFileBrowser.OpenFilePanel(message, "", "", canChooseMultipleFiles);  //stay with false to not 
        if (temp.Length > 0)
            LoadFileIntoPersistentDataFolder(temp[0]);
    }

    private void LoadFileIntoPersistentDataFolder(string _path) // whole logic for loading files into the persistent data folder
    {
        if (File.Exists(destPath + Path.GetFileName(_path)))
            Debug.Log("File already exists in destination folder");

        else if (File.Exists(_path) == true)
        {
            File.Copy(_path, destPath + Path.GetFileName(_path));
            Debug.Log("File successfully loaded into persistent data folder");
        }

        else
            Debug.Log("No file choosen");
    }

    private void ShowAsyncSamples(float _value)
    {
        showAsyncSamplesText.text = "" + sampleOffsetSlider.value;
    }

    //Get Test logic for sample offset stuff

    private void PreviewCurrentSong()
    {
        //start a song with the current offset samples value.
    }
}
