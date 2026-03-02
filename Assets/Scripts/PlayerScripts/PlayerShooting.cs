using UnityEngine;


public class PlayerShooting : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerController controller;
    [SerializeField] private CentralizedValues values;
    [SerializeField] private HitEffect hitEffect;
    
    [Space]

    [SerializeField] private float bulletDamage;
    [SerializeField] private float shootOnBeatMultiplier = 1;


    private Vector3 cameraCenterPoint = new Vector3(0.5f, 0.5f, 1.0f);
    private Ray crosshairCenterRay;
    private RaycastHit hit;
    private bool onBeat;

    private void OnEnable()
    {
        controller.OnShootInvoked_started.AddListener(Player_ShootWeapon);
    }

    private void OnDisable()
    {
        controller.OnShootInvoked_started.RemoveListener(Player_ShootWeapon);
    }

    public void Player_ShootWeapon()
    {
        values.ShotsFired++;
        onBeat = false;
        if (controller.IsOnBeat == true)
        {
            values.OnBeatActions++;
            values.CurrentCombo_Value = values.CurrentCombo_Value + 1;
            values.LastActionOnBeat_Bool = true;
            onBeat = true;
        }

        crosshairCenterRay = Camera.main.ViewportPointToRay(cameraCenterPoint);
        var temp = Physics.Raycast(crosshairCenterRay, out hit, 200f);

        if (temp == true && hit.transform.TryGetComponent(out HurtboxBehaviour current))
        {
            if (onBeat)
            {
                values.OnEnemyHit.Invoke();
                hitEffect.onEnemyHit.Invoke(hit.point);
                current.ApplyDamageToEnemy(bulletDamage * values.CurrentCombo_Value * shootOnBeatMultiplier, true);
                AudioHandler.Instance.PlaySound_sourceActionAmbience(AudioHandler.Instance.hitmarker_2);
            }

            else
            {
                values.OnEnemyHit.Invoke();
                hitEffect.onEnemyHit.Invoke(hit.point);
                current.ApplyDamageToEnemy(bulletDamage * values.CurrentCombo_Value, false);
                AudioHandler.Instance.PlaySound_sourceActionAmbience(AudioHandler.Instance.hitmarker_1);
            }
        }
    }
}

