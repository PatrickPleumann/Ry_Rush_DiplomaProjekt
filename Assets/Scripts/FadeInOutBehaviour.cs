using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeInOutBehaviour : MonoBehaviour
{
    [SerializeField][Range(0.1f, 5f)] private float fadeDuration;
    [SerializeField] private int nextSceneID;
  
    [SerializeField] private GameObject fadeOut;
    [SerializeField] private CanvasGroup group;

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
            yield return new WaitForSeconds(Time.deltaTime * fadeDuration);
            group.alpha += Time.deltaTime;
        }
        SwitchScene();
    }

    public void SwitchScene() 
    {
        var scene = SceneManager.GetActiveScene();
        SceneManager.UnloadSceneAsync(scene);
        SceneManager.LoadSceneAsync(nextSceneID);
    }
}
