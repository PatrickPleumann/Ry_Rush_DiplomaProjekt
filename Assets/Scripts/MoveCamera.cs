using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] private Transform cameraPos;
    void Update()
    {
        gameObject.transform.position = cameraPos.position;
    }
}
