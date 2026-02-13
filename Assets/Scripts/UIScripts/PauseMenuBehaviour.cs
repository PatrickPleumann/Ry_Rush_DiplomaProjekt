using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuBehaviour : MonoBehaviour
{
    [SerializeField] private PlayerController controller;
    [SerializeField] private CentralizedValues values;
    [SerializeField] private BeatTracking beatTracking;
    [SerializeField] private GameObject fadeOut_BG;
    [SerializeField] private GameObject pauseMenu;
 
    [SerializeField] private Button resume_Button;
    [SerializeField] private Button mainMenu_Button;

    private float currentTimeScale;

    private void OnEnable()
    {
        resume_Button.onClick.AddListener(OnResumeButton_Clicked);
        mainMenu_Button.onClick.AddListener(OnMainMenuButton_Clicked);
        controller.onEsc_started.AddListener(EscButton_Clicked);
    }
    private void OnDisable()
    {
        resume_Button.onClick.RemoveListener(OnResumeButton_Clicked);
        mainMenu_Button.onClick.RemoveListener(OnMainMenuButton_Clicked);
        controller.onEsc_started.RemoveListener(EscButton_Clicked);
    }

    private void EscButton_Clicked()
    {
        if (pauseMenu.activeSelf == true)
        {
            Time.timeScale = 1f; // 
            Cursor.lockState = CursorLockMode.Locked;
            values.AllowInput_Bool = true;
            pauseMenu.SetActive(false);
            beatTracking.source.UnPause();
        }
        else
        {
            Time.timeScale = 0f; // very slow while pause menu is active
            Cursor.lockState = CursorLockMode.None;
            values.AllowInput_Bool = false;
            pauseMenu.SetActive(true);
            beatTracking.source.Pause();
        }
    }

    private void OnMainMenuButton_Clicked()
    {
        values.SetDefaultValues();
        fadeOut_BG.SetActive(true);
        var temp = SceneManager.GetActiveScene();
        SceneManager.UnloadSceneAsync(temp);
        SceneManager.LoadSceneAsync(0);
    }

    private void OnResumeButton_Clicked()
    {
        EscButton_Clicked();
    }
}
