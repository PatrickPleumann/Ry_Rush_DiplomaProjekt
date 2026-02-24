using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Threading;
using TMPro;
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
    [SerializeField] public TMP_Text OnBeatActions;
    [SerializeField] public TMP_Text YourScore;

    [SerializeField] public Button backButton;

    private CancellationTokenSource cts = new();

    private void OnEnable()
    {
        backButton.onClick.AddListener(OnBackButton);
    }

    private void OnDisable()
    {
        backButton.onClick.RemoveListener(OnBackButton);
    }

    private async void Start()
    {
        var _timeBetween = values.TimeBetweenBeats;
        Debug.Log("Time between beats: " + _timeBetween);
        await ShowHighscoreScreen(_timeBetween, cts.Token);
    }

    private async UniTask ShowHighscoreScreen(float _timeBetween, CancellationToken _token)
    {
        _token.ThrowIfCancellationRequested();

        await UniTask.Delay((int)(_timeBetween * 2 * 1000), false);
        AudioHandler.Instance.PlaySound_sourceActionAmbience(AudioHandler.Instance.scoreboard_Hit);
        ShotsFired.text = Get_ShotsFired();

        await UniTask.Delay((int)(_timeBetween * 1000), false);
        AudioHandler.Instance.PlaySound_sourceActionAmbience(AudioHandler.Instance.scoreboard_Hit);
        ShotsHit.text = Get_ShotsHit();

        await UniTask.Delay((int)(_timeBetween * 1000), false);
        AudioHandler.Instance.PlaySound_sourceActionAmbience(AudioHandler.Instance.scoreboard_Hit);
        Accuracy.text = Get_Accuracy();

        await UniTask.Delay((int)(_timeBetween * 1000), false);
        AudioHandler.Instance.PlaySound_sourceActionAmbience(AudioHandler.Instance.scoreboard_Hit);
        PointsPerKill.text = Get_PointsPerKill();

        await UniTask.Delay((int)(_timeBetween * 1000), false);
        AudioHandler.Instance.PlaySound_sourceActionAmbience(AudioHandler.Instance.scoreboard_Hit);
        PointsPerHit.text = Get_PointsPerHit();

        await UniTask.Delay((int)(_timeBetween * 1000), false);
        AudioHandler.Instance.PlaySound_sourceActionAmbience(AudioHandler.Instance.scoreboard_Hit);
        Kills.text = Get_Kills();

        await UniTask.Delay((int)(_timeBetween * 1000), false);
        AudioHandler.Instance.PlaySound_sourceActionAmbience(AudioHandler.Instance.scoreboard_Hit);
        OnBeatActions.text = Get_OnBeatActions();

        Cursor.lockState = CursorLockMode.None;

        await UniTask.Delay((int)(_timeBetween * 1000), false);
        AudioHandler.Instance.PlaySound_sourceActionAmbience(AudioHandler.Instance.scoreboard_Hit2);
        YourScore.text = Get_YourScore();

        await UniTask.WaitForEndOfFrame();
    }

    //private IEnumerator ShowHighscoreScreen(float _timeBetween)
    //{
    //    //increase volume here to roughly math song volume
    //    yield return new WaitForSeconds(_timeBetween * 2);
    //    AudioHandler.Instance.PlaySound_sourceActionAmbience(AudioHandler.Instance.scoreboard_Hit);
    //    ShotsFired.text = Get_ShotsFired();

    //    yield return new WaitForSeconds(_timeBetween);
    //    AudioHandler.Instance.PlaySound_sourceActionAmbience(AudioHandler.Instance.scoreboard_Hit);
    //    ShotsHit.text = Get_ShotsHit();

    //    yield return new WaitForSeconds(_timeBetween);
    //    AudioHandler.Instance.PlaySound_sourceActionAmbience(AudioHandler.Instance.scoreboard_Hit);
    //    Accuracy.text = Get_Accuracy();

    //    yield return new WaitForSeconds(_timeBetween);
    //    AudioHandler.Instance.PlaySound_sourceActionAmbience(AudioHandler.Instance.scoreboard_Hit);
    //    PointsPerKill.text = Get_PointsPerKill();

    //    yield return new WaitForSeconds(_timeBetween);
    //    AudioHandler.Instance.PlaySound_sourceActionAmbience(AudioHandler.Instance.scoreboard_Hit);
    //    PointsPerHit.text = Get_PointsPerHit();

    //    yield return new WaitForSeconds(_timeBetween);
    //    AudioHandler.Instance.PlaySound_sourceActionAmbience(AudioHandler.Instance.scoreboard_Hit);
    //    Kills.text = Get_Kills();

    //    yield return new WaitForSeconds(_timeBetween);
    //    AudioHandler.Instance.PlaySound_sourceActionAmbience(AudioHandler.Instance.scoreboard_Hit);
    //    OnBeatActions.text = Get_OnBeatActions();

    //    Cursor.lockState = CursorLockMode.None;

    //    yield return new WaitForSeconds(_timeBetween);
    //    AudioHandler.Instance.PlaySound_sourceActionAmbience(AudioHandler.Instance.scoreboard_Hit2);
    //    YourScore.text = Get_YourScore();

    //    yield return new WaitForEndOfFrame();
    //}

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
        return values.ShotsFired.ToString();
    }
    private string Get_ShotsHit()
    {
        return values.ShotsHit.ToString();
    }
    private string Get_Accuracy()
    {
        if (values.ShotsFired > 0 && values.ShotsHit > 0)
            return (Utility.FloorFloatToTwoDigits(values.ShotsHit / values.ShotsFired) * 100f).ToString() + " %";
        else
            return "Invalid values";
    }
    private string Get_PointsPerKill()
    {
        if (values.CurrentScore_Value > 0 && values.Kills > 0)
            return Utility.FloorFloatToTwoDigits(values.CurrentScore_Value / values.Kills).ToString();
        else
            return "Invalid values";
    }
    private string Get_PointsPerHit()
    {
        if (values.CurrentScore_Value > 0 && values.ShotsHit > 0)
            return Utility.FloorFloatToTwoDigits(values.CurrentScore_Value / values.ShotsHit).ToString();
        else
            return "Invalid Values";
    }
    private string Get_Kills()
    {
        return values.Kills.ToString();
    }
    private string Get_OnBeatActions()
    {
        return values.OnBeatActions.ToString();
    }
    private string Get_YourScore()
    {
        return values.CurrentScore_Value.ToString();
    }
    #endregion

    private void OnDestroy()
    {
        cts.Cancel();
        cts.Dispose();
    }
}
