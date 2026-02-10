using UnityEngine;
using UnityEngine.Events;


public class PlayerShooting : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerController controller;
    [SerializeField] private Scoreboard_UI scoreboard;
    [SerializeField] private BeatTracking beatTracking;
    [SerializeField] private CentralizedValues values;
    [Space]

    [SerializeField] private LayerMask targetLayerMask;
    [SerializeField] private float bulletDamage;
    [SerializeField] private float shootOnBeatMultiplier = 1;


    public UnityAction OnWeaponShoot;
    private Vector3 cameraCenterPoint = new Vector3(0.5f, 0.5f, 1.0f);
    private Ray crosshairCenterRay;
    private RaycastHit hit;
    private bool onBeat;

    private void OnEnable()
    {
        controller.onShootInvoked_started.AddListener(Player_ShootWeapon);
        
    }

    private void OnDisable()
    {
        controller.onShootInvoked_started.RemoveListener(Player_ShootWeapon);
        
    }

    public void Player_ShootWeapon()
    {
        values.ShotsFired++;
        onBeat = false;
        if (controller.IsOnBeat == true)
        {
            values.CurrentCombo_Value = values.CurrentCombo_Value + 1;
            values.LastActionOnBeat_Bool = true;
            onBeat = true;
        }

        crosshairCenterRay = Camera.main.ViewportPointToRay(cameraCenterPoint);
        var temp = Physics.Raycast(crosshairCenterRay, out hit, 200f, targetLayerMask);

        if (temp == true && hit.transform.TryGetComponent(out HurtboxBehaviour current))
        {
            if (onBeat)
            {
                current.ApplyDamageToEnemy(bulletDamage * values.CurrentCombo_Value * shootOnBeatMultiplier); //more dmg pls
                AudioHandler.Instance.PlaySound_sourceActionAmbience(AudioHandler.Instance.hitmarker_2);
            }

            else
            {
                current.ApplyDamageToEnemy(bulletDamage * values.CurrentCombo_Value);
                AudioHandler.Instance.PlaySound_sourceActionAmbience(AudioHandler.Instance.hitmarker_1);
            }
        }
    }
}

