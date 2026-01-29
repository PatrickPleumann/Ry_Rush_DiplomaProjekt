using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeInOutBehaviour : MonoBehaviour
{
    [SerializeField] private float fadeDuration;
    [SerializeField] private int nextSceneID;
  
    [SerializeField] private GameObject fadeOut;
    [SerializeField] private CanvasGroup group;

    private void OnEnable()
    {
        fadeOut.SetActive(false);
    }

    public void FadeOut_Start()
    {
        StartCoroutine(FadeScreen());
    }
    private IEnumerator FadeScreen()
    {
        fadeOut.SetActive(true);

        group.alpha = 0;
        while (group.alpha < 1)
        {
            group.alpha += Time.deltaTime / fadeDuration;
            yield return new WaitForEndOfFrame();
        }
        SwitchScene();
    }

    public void SwitchScene() //gets called only if button is active
    {
        var scene = SceneManager.GetActiveScene();
        SceneManager.UnloadSceneAsync(scene);
        SceneManager.LoadSceneAsync(nextSceneID);
    }
}
