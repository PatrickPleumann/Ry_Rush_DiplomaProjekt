using System.Runtime.CompilerServices;
using UnityEngine;

public class HealthPickUp : MonoBehaviour
{
    [SerializeField] private float healthAmountPerItem;

    private void OnTriggerEnter(Collider _trigger)
    {
        if (_trigger.transform.TryGetComponent(out PlayerHealthSystem _health) == true && _health.OnValidate_PickUpHealthItem())
        {
            _health.IncreasePlayerHealth(healthAmountPerItem);
            DeactivateHealthItem();
        }
    }

    private void DeactivateHealthItem()
    {
        gameObject.SetActive(false);
    }
}
