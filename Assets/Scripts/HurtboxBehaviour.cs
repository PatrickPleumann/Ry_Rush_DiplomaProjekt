using UnityEngine;

public class HurtboxBehaviour : MonoBehaviour
{
    [SerializeField] private float dmgMultiplier;
    [SerializeField] private EnemyController controller;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ProjectileBehaviour bullet) == true)
        {
            controller.TakeDamage(bullet.damage * dmgMultiplier);
        }
    }
}
