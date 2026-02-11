using UnityEngine;

public class HoldPlayerPosition : MonoBehaviour
{
    [SerializeField] private PlayerInfo info;
    [SerializeField] private GameObject allEnemies;
    [SerializeField] private GameObject allSpawnpoints;
    [SerializeField] private GameObject navMesh;
    private void Awake()
    {
        info.FindPlayerPosition();
        navMesh.SetActive(true);
        allEnemies.SetActive(true);
        allSpawnpoints.SetActive(true);
    }
}
