using UnityEngine;

public class EnemyMaxChasingCounter : MonoBehaviour
{
    [SerializeField] private int maxEnemiesChasing = 5;
    private int currentEnemiesChasing = 0;

    public void RegisterChaseState()
    {
        if (currentEnemiesChasing < maxEnemiesChasing)
        {
            currentEnemiesChasing++;
        }
    }

    public void UnregisterFromChasing()
    {
        if (currentEnemiesChasing < 0)
            currentEnemiesChasing--;
    }

    public bool CheckCurrentEnemiesChasing()
    {
        if (currentEnemiesChasing < maxEnemiesChasing)
        {
            return true;
        }
        return false;
    }
}
