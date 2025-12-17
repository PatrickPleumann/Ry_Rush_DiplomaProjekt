using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button StartGameButton;
    public Button ImportSongButton;
    public Button OptionsButton;
    public Button QuitGameButton;



    private void OnEnable()
    {
        StartGameButton.onClick.AddListener(OnStartGameButtonClicked);
        ImportSongButton.onClick.AddListener(OnImportSongButtonClicked);
        OptionsButton.onClick.AddListener(OnOptionsButtonClicked);
        QuitGameButton.onClick.AddListener(OnQuitGameButtonClicked);
    }
    private void OnDisable()
    {
        StartGameButton.onClick.RemoveListener(OnStartGameButtonClicked);
        ImportSongButton.onClick.RemoveListener(OnImportSongButtonClicked);
        OptionsButton.onClick.RemoveListener(OnOptionsButtonClicked);
        QuitGameButton.onClick.RemoveListener(OnQuitGameButtonClicked);
    }

    private void OnStartGameButtonClicked()
    {
        var temp = SceneManager.GetActiveScene();
        SceneManager.UnloadSceneAsync(temp);
        SceneManager.LoadSceneAsync(1);   // Menu Scene is 0, Game Scene is 1
    }

    private void OnOptionsButtonClicked()
    {
        throw new NotImplementedException();
    }
    private void OnImportSongButtonClicked()
    {
        throw new NotImplementedException();
    }

    private void OnQuitGameButtonClicked()
    {
        Application.Quit();
    }
}
