using UnityEngine;

public class PlayerHealthSystem : MonoBehaviour
{
    [SerializeField] private float playerMaxHealth;
    [SerializeField] private float playerCurrentHealth;
    [SerializeField] private float playerDamagePerHit;
    [SerializeField] private float percentageToTakeDamage;
    private float percentageIntoRandomNumber;

    private void Start()
    {
        percentageIntoRandomNumber = Mathf.FloorToInt(100 / percentageToTakeDamage);
        playerCurrentHealth = playerMaxHealth;
    }
    public void TakeDamage_Player()
    {
        var randomNum = Mathf.FloorToInt(Random.Range(0, percentageIntoRandomNumber - 1)); //beware, inclusive
        Debug.Log("Try to hit player, random number is: " + randomNum);
        if (randomNum == 1)
        {
            //maybe apply a post process effect & sound effect
            playerCurrentHealth -= playerDamagePerHit;
            Debug.Log("I just took damage, current health: " + playerCurrentHealth);
        }
    }
}
