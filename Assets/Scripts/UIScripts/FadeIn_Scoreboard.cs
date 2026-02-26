using Cysharp.Threading.Tasks;
using System.Collections;
using UnityEngine;

public class FadeIn_Scoreboard : MonoBehaviour
{
    [SerializeField] private CentralizedValues values;

    private CanvasGroup grp;
    private void Awake()
    {
        grp = GetComponent<CanvasGroup>();
        grp.alpha = 0;
    }
    private void OnEnable()
    {
        StartFadeIn();
    }

    private async void StartFadeIn()
    {
       await FadeInGrp();
    }

    private async UniTask FadeInGrp()
    {
        float temp = 0.5f;
        if (values.TimeBetweenBeats > 0)
            temp = values.TimeBetweenBeats;
        
        while (grp.alpha < 1)
        {
            await UniTask.Delay((int)(Time.deltaTime * temp * 1000));
            grp.alpha += Time.deltaTime;
        }
        await UniTask.WaitForEndOfFrame();
    }
}
