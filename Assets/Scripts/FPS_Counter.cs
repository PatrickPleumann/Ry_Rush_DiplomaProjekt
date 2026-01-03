using TMPro;
using UnityEngine;

public class FPS_Counter : MonoBehaviour
{
    float current;
    public TMP_Text fpsCounter;

    void Update()
    {
        current = (int)(1f / Time.unscaledDeltaTime);
        fpsCounter.text = "FPS: " + current;
    }
}
