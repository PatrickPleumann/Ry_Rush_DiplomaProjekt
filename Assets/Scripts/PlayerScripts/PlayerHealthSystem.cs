using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerHealthSystem : MonoBehaviour
{
    [SerializeField] private BeatTracking beatTracking;
    [SerializeField] private CentralizedValues values;
    [SerializeField] private float playerMaxHealth;
    [SerializeField] private float playerDamagePerHit;
    [SerializeField] private float percentageToTakeDamage;

    [SerializeField][Range(1f, 5f)] private float decreaseTimeScaleOnPlayerDeathMultiplier = 1f;

    private float percentageIntoRandomNumber;

    private void Start()
    {
        percentageIntoRandomNumber = Mathf.FloorToInt(100 / percentageToTakeDamage);
        values.PlayerCurrentHealth = playerMaxHealth;
    }

    /// <summary>
    /// Methods gets called if an enemy hits the player with a Raycast. If the damage gets through is random, which simulates bloom effect on weapons.
    /// At default the rate of enemies actually hitting the player is 20 %.
    /// </summary>
    public void TakeDamage_Player()
    {
        var randomNum = Mathf.FloorToInt(Random.Range(0, percentageIntoRandomNumber - 1)); //beware, maxInclusive
        if (randomNum == 1 &&  values.PlayerIsDead == false)
        {
            values.PlayerCurrentHealth -= playerDamagePerHit;

            AudioHandler.Instance.PlayOneShot(AudioHandler.Instance.playerDamaged_Sound[Random.Range(0, AudioHandler.Instance.playerDamaged_Sound.Length)]);

            if (values.PlayerCurrentHealth <= 0)
                PlayerDies();
        }
    }

    /// <summary>
    /// On Health item pickup, this method increases the players health. Gets called only if player health is below max health.
    /// </summary>
    /// <param name="_healthAmount"></param>
    public void IncreasePlayerHealth(float _healthAmount)
    {
        values.PlayerCurrentHealth = values.PlayerCurrentHealth + _healthAmount;
        AudioHandler.Instance.PlayOneShot(AudioHandler.Instance.healthPickUp_Sound[Random.Range(0, AudioHandler.Instance.healthPickUp_Sound.Length)]);
    }

    private async void PlayerDies()
    {
        values.SessionIsOver = true;
        values.PlayerIsDead = true;
        values.OnPlayerDeath.Invoke();
        Debug.Log("PLAYER DEAD");
        AudioHandler.Instance.PlayOneShot(AudioHandler.Instance.playerDead_Sounds[Random.Range(0, AudioHandler.Instance.playerDead_Sounds.Length)]);
        await DecreaseTimeOnPlayerDead(decreaseTimeScaleOnPlayerDeathMultiplier);
    }

    /// <summary>
    /// Validates if current player health is below max player health. This return false if player health is equal to max health
    /// </summary>
    /// <returns></returns>
    public bool OnValidate_PickUpHealthItem()
    {
        return values.PlayerCurrentHealth < values.PlayerMaxHealth;
    }

    private async UniTask DecreaseTimeOnPlayerDead(float _decreaseTimeScaleOnPlayerDeathMultiplier)
    {
        while (Time.timeScale > 0.01f)
        {
            await UniTask.Delay((int)(_decreaseTimeScaleOnPlayerDeathMultiplier * 1000 * Time.deltaTime),true);
            Time.timeScale -= Time.deltaTime;
        }
        Time.timeScale = 0f;
    }
}
