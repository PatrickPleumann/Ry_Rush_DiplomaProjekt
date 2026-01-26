using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChooseSongManager : MonoBehaviour
{
    [SerializeField] private Button startGameButton;
    [SerializeField] private Button returnButton;
    [SerializeField] private float timeTillGameStarts = 0.5f;


    private void OnEnable()
    {
        startGameButton.onClick.AddListener(OnStartGameButton_Clicked);
    }

    private void OnDisable()
    {
        startGameButton.onClick.RemoveListener(OnStartGameButton_Clicked);
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
}
