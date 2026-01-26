using UnityEngine;

public class ParticleSpawnerTEst : MonoBehaviour
{
    [SerializeField] public GameObject prefab;
    [SerializeField] public Transform origin;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(prefab, origin);
        }
    }
}
