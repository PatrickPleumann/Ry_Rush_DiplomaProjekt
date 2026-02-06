using UnityEngine;

public class HoldPlayerPosition : MonoBehaviour
{
    [SerializeField] private PlayerInfo info;
    [SerializeField] private GameObject allEnemies;
    [SerializeField] private GameObject allSpawnpoints;
    private void Awake()
    {
        info.FindPlayerPosition();
        allEnemies.SetActive(true);
        allSpawnpoints.SetActive(true);
    }
}
