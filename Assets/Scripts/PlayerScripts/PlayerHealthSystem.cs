using UnityEngine;

public class PlayerHealthSystem : MonoBehaviour
{
    [SerializeField] private CentralizedValues values;
    [SerializeField] private float playerMaxHealth;
    [SerializeField] private float playerDamagePerHit;
    [SerializeField] private float percentageToTakeDamage;

    private float percentageIntoRandomNumber;

    private void Start()
    {
        percentageIntoRandomNumber = Mathf.FloorToInt(100 / percentageToTakeDamage);
        values.PlayerCurrentHealth = playerMaxHealth;
    }
    public void TakeDamage_Player()
    {
        var randomNum = Mathf.FloorToInt(Random.Range(0, percentageIntoRandomNumber - 1)); //beware, maxInclusive
        //Debug.Log("Try to hit player, random number is: " + randomNum);
        if (randomNum == 1)
        {
            //maybe an vfx effect on player got hit
            values.PlayerCurrentHealth -= playerDamagePerHit;
            //Debug.Log("I just took damage, current health: " + values.PlayerCurrentHealth);

            AudioHandler.Instance.PlayOneShot(AudioHandler.Instance.playerDamaged_Sound[Random.Range(0, AudioHandler.Instance.playerDamaged_Sound.Length)]);

            if (values.PlayerCurrentHealth <= 0)
                PlayerDies();
        }
    }

    public void IncreasePlayerHealth(float _healthAmount)
    {
        //Debug.Log("Health increased by value: " + _healthAmount);
        values.PlayerCurrentHealth = values.PlayerCurrentHealth + _healthAmount;
        AudioHandler.Instance.PlayOneShot(AudioHandler.Instance.healthPickUp_Sound[Random.Range(0, AudioHandler.Instance.healthPickUp_Sound.Length)]);
        //Debug.Log("New Health is now: " + values.PlayerCurrentHealth);
    }

    private void PlayerDies()
    {
        Debug.Log("PLAYER DEAD");
        //play dramatic audio sound or so in ambience_1 source

        //some logic for the player to die
        //death screen
        //maybe highscore screen 
        //invoke some death screen event here..
    }

    public bool OnValidate_PickUpHealthItem()
    {
        return values.PlayerCurrentHealth < values.playerMaxHealth;
    }
}
