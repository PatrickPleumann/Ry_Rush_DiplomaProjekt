using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] private Transform cameraPos;

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = cameraPos.position;
    }
}
