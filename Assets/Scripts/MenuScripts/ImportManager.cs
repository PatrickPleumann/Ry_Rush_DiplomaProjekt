using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ImportManager : MonoBehaviour
{
    //Load Data from file dialog into persitent data folder

    [SerializeField] private Button openFileDialog;
    private const string message = "Browse Files";

    private void Awake()
    {
        
    }

    private void OnEnable()
    {
        openFileDialog.onClick.AddListener(BrowseFiles);
    }

    private void BrowseFiles()
    {
        var temp = EditorUtility.OpenFilePanel(message, null, ".wav");
    }
}
