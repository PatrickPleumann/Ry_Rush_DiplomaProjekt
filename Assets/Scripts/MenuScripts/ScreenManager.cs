using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    public GameObject currentScreen;
    public GameObject initialScreen;

    private void Awake()
    {
        currentScreen = initialScreen;
        initialScreen.SetActive(true);
    }

    public void ShowScreen(GameObject _screen)
    {
        currentScreen.SetActive(false);
        _screen.SetActive(true);

        currentScreen = _screen;
    }
}
