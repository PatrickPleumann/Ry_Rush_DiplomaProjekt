using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;

public class DeathScreenBehaviour : MonoBehaviour
{
    [SerializeField] private CentralizedValues values;
    [SerializeField] private CanvasGroup deathScreen_canvasGrp;
    [SerializeField] private Button backToMainMenu_Button;
    [SerializeField] private GameObject PlayerIngameUI;
    [SerializeField] private GameObject deathScreen;

    private void Awake()
    {
        values.onPlayerDeath.AddListener(InitDeathScreen);
        backToMainMenu_Button.onClick.AddListener(ReturnToMainMenu);
    }

    private void ReturnToMainMenu()
    {

        values.onPlayerDeath.RemoveListener(InitDeathScreen);
        backToMainMenu_Button.onClick.RemoveListener(ReturnToMainMenu);

        var temp = SceneManager.GetActiveScene();
        SceneManager.UnloadSceneAsync(temp);
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync(0);
    }

    private async void InitDeathScreen()
    {
        values.AllowInput = false;
        PlayerIngameUI.SetActive(false);
        deathScreen.SetActive(true);

        await FadeInDeathScreen();
    }
    private async UniTask FadeInDeathScreen()
    {
        while(deathScreen_canvasGrp.alpha < 1)
        {
            await UniTask.Delay((int)(Time.deltaTime * 1000), true);
            deathScreen_canvasGrp.alpha += Time.deltaTime;
        }
        Cursor.lockState = CursorLockMode.None;
    }
}
 