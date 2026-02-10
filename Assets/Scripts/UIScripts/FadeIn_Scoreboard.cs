using System.Collections;
using UnityEngine;

public class FadeIn_Scoreboard : MonoBehaviour
{
    [SerializeField] private CentralizedValues values;

    private CanvasGroup grp;
    private void Awake()
    {
        grp = GetComponent<CanvasGroup>();
    }
    private void Start()
    {
        grp.alpha = 0;
        StartCoroutine(FadeInGrp());
    }

    private IEnumerator FadeInGrp()
    {
        float temp = 0.5f;
        if (values.TimeBetweenBeats > 0)
            temp = values.TimeBetweenBeats;
        
        while (grp.alpha < 1)
        {
            yield return new WaitForSeconds(Time.deltaTime * temp);
            grp.alpha += Time.deltaTime;
        }
        yield return new WaitForEndOfFrame();
    }
}
