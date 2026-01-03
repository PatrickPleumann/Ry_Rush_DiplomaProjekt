using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] private Transform cameraPos;
    void LateUpdate()
    {
        gameObject.transform.position = cameraPos.position;
    }
}
