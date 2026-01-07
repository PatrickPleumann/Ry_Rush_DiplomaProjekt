using TMPro;
using UnityEngine;

public class FPS_Counter : MonoBehaviour
{
    [SerializeField] private float fpsUpdateTimeDelay = 0.25f;
    public TMP_Text fpsCounter;

    private float current;
    private float fpsUpdateTime;

    void Update()
    {
        fpsUpdateTime -= Time.deltaTime;
        if (fpsUpdateTime <= 0f)
        {
            ShowFPS();
            fpsUpdateTime = fpsUpdateTimeDelay;
        }
    }

    private void ShowFPS()
    {
        current = (int)(1f / Time.unscaledDeltaTime);
        fpsCounter.text = "FPS: " + current;
    }
}
