using System;
using System.Collections;
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
        QuitGameButton.onClick.AddListener(OnQuitGameButtonClicked);
    }
    private void OnDisable()
    {
        StartGameButton.onClick.RemoveListener(OnStartGameButtonClicked);
        QuitGameButton.onClick.RemoveListener(OnQuitGameButtonClicked);
    }

    private void OnStartGameButtonClicked()
    {
        //Load song data dictionary and other stuff to prepare for game start here
    }

    private void OnQuitGameButtonClicked()
    {
        //maybe fade out and quit afterwards
        Application.Quit();
    }
}
