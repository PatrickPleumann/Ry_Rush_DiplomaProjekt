using Unity.VisualScripting;
using UnityEngine;

public class HurtboxBehaviour : MonoBehaviour
{
    [SerializeField] private float dmgMultiplier;
    [SerializeField] private EnemyController controller;

    public void ApplyDamageToEnemy(float _dmgAmount)
    {
        controller.TakeDamage(_dmgAmount * dmgMultiplier);
        Debug.Log(_dmgAmount * dmgMultiplier);
    }
}
