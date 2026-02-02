using TMPro;
using UnityEngine;

public class FPS_Counter : MonoBehaviour
{
    [SerializeField] private float fpsUpdateTimeDelay = 0.25f;
    public TMP_Text fpsCounter;

    private float current;
    private float fpsUpdateTime;

    public bool showFPS;

    private void Awake()
    {
        showFPS = true;
    }
    private void Start()
    {
        if (GameManager.Instance.settingsData != null)
            showFPS = GameManager.Instance.settingsData.ShowFPS;
    }
    void Update()
    {
        if (showFPS == true)
        {
            fpsUpdateTime -= Time.deltaTime;
            if (fpsUpdateTime <= 0f)
            {
                ShowFPS();
            }
        }
    }

    private void ShowFPS()
    {
        current = (int)(1f / Time.unscaledDeltaTime);
        fpsCounter.text = "FPS: " + current;
        fpsUpdateTime = fpsUpdateTimeDelay;
    }
}
