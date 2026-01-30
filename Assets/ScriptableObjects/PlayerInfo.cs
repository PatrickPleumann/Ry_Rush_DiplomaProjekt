using UnityEngine;

[CreateAssetMenu(fileName = "PlayerInfo", menuName = "Scriptable Objects/PlayerInfo")]
public class PlayerInfo : ScriptableObject
{
    public Transform PlayerPosition;
    /// <summary>
    /// This methods only exists because of the unsure execution order behaviour of scriptable objects compared to regular gameobjects. It is import
    /// to call the OnEnable function on regular gameobjects after the scriptable object recieved the player position. But for sure very case sensitive.
    /// </summary>
    public void FindPlayerPosition()
    {
        Debug.Log("Player Transform wird gesucht und zu gewiesen");
        PlayerPosition = FindFirstObjectByType<PlayerController>().transform;
    }
}
