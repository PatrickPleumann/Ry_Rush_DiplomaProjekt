using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System.Threading;

public class DeathScreenBehaviour : MonoBehaviour
{
    [SerializeField] private CentralizedValues values;
    [SerializeField] private CanvasGroup deathScreen_canvasGrp;
    [SerializeField] private Button backToMainMenu_Button;
    [SerializeField] private GameObject PlayerIngameUI;
    [SerializeField] private GameObject deathScreen;

    private CancellationTokenSource cts = new();
    private void Awake()
    {
        values.OnPlayerDeath.AddListener(InitDeathScreen);
        backToMainMenu_Button.onClick.AddListener(ReturnToMainMenu);
    }

    private void ReturnToMainMenu()
    {
        values.OnPlayerDeath.RemoveListener(InitDeathScreen);
        backToMainMenu_Button.onClick.RemoveListener(ReturnToMainMenu);

        Time.timeScale = 1f;
        var temp = SceneManager.GetActiveScene();
        SceneManager.UnloadSceneAsync(temp);
        SceneManager.LoadSceneAsync(0);
    }

    private async void InitDeathScreen()
    {
        values.AllowInput = false;
        PlayerIngameUI.SetActive(false);
        deathScreen.SetActive(true);

        await FadeInDeathScreen(cts.Token);
    }
    private async UniTask FadeInDeathScreen(CancellationToken _token)
    {
        _token.ThrowIfCancellationRequested();
        Cursor.lockState = CursorLockMode.None;
        while (deathScreen_canvasGrp.alpha < 1 && deathScreen_canvasGrp == true)
        {
            await UniTask.Delay((int)(Time.deltaTime * 1000), true);
            deathScreen_canvasGrp.alpha += Time.deltaTime;
        }
    }

    private void OnDestroy()
    {
        cts.Cancel();
        cts.Dispose();
    }
}
