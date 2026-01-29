using UnityEngine;

[CreateAssetMenu(fileName = "PlayerInfo", menuName = "Scriptable Objects/PlayerInfo")]
public class PlayerInfo : ScriptableObject
{
    public Transform PlayerPosition;

    private void Awake()
    {
        PlayerPosition = FindFirstObjectByType<PlayerController>().transform;
    }
}
