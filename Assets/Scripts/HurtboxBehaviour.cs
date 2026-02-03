using Unity.VisualScripting;
using UnityEngine;

public class HurtboxBehaviour : MonoBehaviour
{
    [SerializeField] private float dmgMultiplier;
    [SerializeField] private EnemyController controller;


    private void OnCollisionEnter(Collision _other)
    {
        
        if (_other.transform.TryGetComponent(out ProjectileBehaviour bullet) == true)
        {
            controller.TakeDamage(bullet.CalculateCurrentDmg() * dmgMultiplier);
            Debug.Log(bullet.CalculateCurrentDmg() * dmgMultiplier);
        }
    }
}
