using System.Collections;
using UnityEngine;

public class FadeIn_Scoreboard : MonoBehaviour
{
    [SerializeField][Range(0.1f,5)] private float timeTillFadeIn;

    private CanvasGroup grp;
    private void Awake()
    {
        grp = GetComponent<CanvasGroup>();
        grp.alpha = 0;
    }
    private void Start()
    {
        StartCoroutine(FadeInGrp());
    }

    private IEnumerator FadeInGrp()
    {
        while (grp.alpha < 1)
        {
            yield return new WaitForSeconds(Time.deltaTime * timeTillFadeIn);
            grp.alpha += Time.deltaTime;
        }
        yield return new WaitForEndOfFrame();
    }
}
