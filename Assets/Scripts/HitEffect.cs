using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

public class HitEffect : MonoBehaviour
{
    [SerializeField] public VisualEffect hitEffect;
    [SerializeField] private VisualEffectAsset hitEffect_asset;
    [HideInInspector] public UnityEvent<Vector3> onEnemyHit;

    private void OnEnable()
    {
        onEnemyHit.AddListener(PlayHitEffect);
    }
    private void OnDisable()
    {
        onEnemyHit.AddListener(PlayHitEffect);
    }
    private void Start()
    {
        hitEffect = GetComponent<VisualEffect>();
    }

    private void PlayHitEffect(Vector3 _hitPoint)
    {
        gameObject.transform.position = _hitPoint;
        hitEffect.Play();
    }
}
