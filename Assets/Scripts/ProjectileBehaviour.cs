using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    [SerializeField] public float damage = 100f;
    [SerializeField] private float despawnTime = 3;
    private void Update()
    {
        despawnTime -= Time.deltaTime;
        if (despawnTime <= 0)
        {
            Destroy(gameObject);
        }
    }
}
