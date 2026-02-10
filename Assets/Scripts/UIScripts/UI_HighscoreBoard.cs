using System;
using System.Collections;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_HighscoreBoard : MonoBehaviour
{
    [SerializeField] private CentralizedValues values;

    [SerializeField] public TMP_Text ShotsFired;
    [SerializeField] public TMP_Text ShotsHit;
    [SerializeField] public TMP_Text Accuracy;
    [SerializeField] public TMP_Text PointsPerKill;
    [SerializeField] public TMP_Text PointsPerHit;
    [SerializeField] public TMP_Text Kills;
    [SerializeField] public TMP_Text YourScore;


    [SerializeField][Range(1f, 2f)] private float timeToTillShowScore;
    [SerializeField][Range(0.1f, 0.5f)] private float timeBetweenScoreShowingUp;
    [SerializeField][Range(0.2f, 1f)] private float timeBeforeEndScorePopsUp;

    [SerializeField] public Button backButton;

    private void OnEnable()
    {
        backButton.onClick.AddListener(OnBackButton);
    }


    private void OnDisable()
    {
        backButton.onClick.RemoveListener(OnBackButton);
    }


    private void Start()
    {
        StartCoroutine(ShowHighscoreScreen());
    }


    private IEnumerator ShowHighscoreScreen()
    {
        yield return new WaitForSeconds(timeToTillShowScore);

        yield return new WaitForSeconds(timeBetweenScoreShowingUp);
        AudioHandler.Instance.PlaySound_sourceActionAmbience(AudioHandler.Instance.scoreboard_Hit);
        ShotsFired.text = Get_ShotsFired();

        yield return new WaitForSeconds(timeBetweenScoreShowingUp);
        AudioHandler.Instance.PlaySound_sourceActionAmbience(AudioHandler.Instance.scoreboard_Hit);
        ShotsHit.text = Get_ShotsHit();

        yield return new WaitForSeconds(timeBetweenScoreShowingUp);
        AudioHandler.Instance.PlaySound_sourceActionAmbience(AudioHandler.Instance.scoreboard_Hit);
        Accuracy.text = Get_Accuracy();

        yield return new WaitForSeconds(timeBetweenScoreShowingUp);
        AudioHandler.Instance.PlaySound_sourceActionAmbience(AudioHandler.Instance.scoreboard_Hit);
        PointsPerKill.text = Get_PointsPerKill();

        yield return new WaitForSeconds(timeBetweenScoreShowingUp);
        AudioHandler.Instance.PlaySound_sourceActionAmbience(AudioHandler.Instance.scoreboard_Hit);
        PointsPerHit.text = Get_PointsPerHit();

        yield return new WaitForSeconds(timeBetweenScoreShowingUp);
        AudioHandler.Instance.PlaySound_sourceActionAmbience(AudioHandler.Instance.scoreboard_Hit);
        Kills.text = Get_Kills();

        Cursor.lockState = CursorLockMode.None;

        yield return new WaitForSeconds(timeBeforeEndScorePopsUp);
        AudioHandler.Instance.PlaySound_sourceActionAmbience(AudioHandler.Instance.scoreboard_Hit2);
        YourScore.text = Get_YourScore();

        yield return new WaitForEndOfFrame();
    }
    private void OnBackButton()
    {
        values.SetDefaultValues();
        var temp = SceneManager.GetActiveScene();
        SceneManager.UnloadSceneAsync(temp);
        SceneManager.LoadSceneAsync(0);
    }


    #region Get Scoreboard values methods
    private string Get_ShotsFired()
    {
        return ShotsFired.text = values.ShotsFired.ToString();
    }
    private string Get_ShotsHit()
    {
        return ShotsHit.text = values.ShotsHit.ToString();
    }
    private string Get_Accuracy()
    {
        if (values.ShotsFired > 0 && values.ShotsHit > 0)
            return Accuracy.text = Utility.FloorFloat_TwoDigits(values.ShotsHit / values.ShotsFired).ToString();
        else
            return Accuracy.text = "Invalid values";
    }
    private string Get_PointsPerKill()
    {
        if (values.CurrentScore_Value > 0 && values.Kills > 0)
            return PointsPerKill.text = Utility.FloorFloat_TwoDigits(values.CurrentScore_Value / values.Kills).ToString();
        else
            return PointsPerKill.text = "Invalid values";
    }
    private string Get_PointsPerHit()
    {
        if (values.CurrentScore_Value > 0 && values.ShotsHit > 0)
            return PointsPerHit.text = Utility.FloorFloat_TwoDigits(values.CurrentScore_Value / values.ShotsHit).ToString();
        else
            return PointsPerHit.text = "Invalid Values";
    }
    private string Get_Kills()
    {
        return Kills.text = values.Kills.ToString();
    }
    private string Get_YourScore()
    {
        return values.CurrentScore_Value.ToString();
    }
    #endregion
}
