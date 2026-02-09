using System.Collections;
using TMPro;
using UnityEngine;

public class UI_HighscoreBoard : MonoBehaviour
{
    [SerializeField] private CentralizedValues values;

    [SerializeField] public TMP_Text ShotsFired;
    [SerializeField] public TMP_Text ShotsHit;
    [SerializeField] public TMP_Text Accuracy;
    [SerializeField] public TMP_Text PointsPerKill;
    [SerializeField] public TMP_Text PointsPerHit;
    [SerializeField] public TMP_Text YourScore;

    [SerializeField] private float timeToTillShowScore;

    [SerializeField] private float timeBetweenScoreShowingUp;
    [SerializeField] private float timeBeforeEndScorePopsUp;
    private void Start()
    {
        StartCoroutine(ShowHighscoreScreen());
    }
    private IEnumerator ShowHighscoreScreen()
    {
        yield return new WaitForSeconds(timeToTillShowScore);

        yield return new WaitForSeconds(timeBetweenScoreShowingUp);
        ShotsFired.text = "Nice";
        AudioHandler.Instance.PlaySound_sourceActionAmbience(AudioHandler.Instance.scoreboard_Hit);

        yield return new WaitForSeconds(timeBetweenScoreShowingUp);
        ShotsHit.text = "Nice";
        AudioHandler.Instance.PlaySound_sourceActionAmbience(AudioHandler.Instance.scoreboard_Hit);

        yield return new WaitForSeconds(timeBetweenScoreShowingUp);
        Accuracy.text = "Nice";
        AudioHandler.Instance.PlaySound_sourceActionAmbience(AudioHandler.Instance.scoreboard_Hit);

        yield return new WaitForSeconds(timeBetweenScoreShowingUp);
        PointsPerKill.text = "Nice";
        AudioHandler.Instance.PlaySound_sourceActionAmbience(AudioHandler.Instance.scoreboard_Hit);

        yield return new WaitForSeconds(timeBetweenScoreShowingUp);
        PointsPerHit.text = "Nice";
        AudioHandler.Instance.PlaySound_sourceActionAmbience(AudioHandler.Instance.scoreboard_Hit);

        yield return new WaitForSeconds(timeBeforeEndScorePopsUp);
        AudioHandler.Instance.PlaySound_sourceActionAmbience(AudioHandler.Instance.scoreboard_Hit2);
        YourScore.text = values.CurrentScore_Value.ToString();

        yield return new WaitForEndOfFrame();
    }
}
