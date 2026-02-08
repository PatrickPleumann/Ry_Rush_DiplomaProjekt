using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerController controller;
    [SerializeField] private Scoreboard_UI scoreboard;
    [SerializeField] private BeatTracking beatTracking;
    [SerializeField] private AudioHandler audioHandler;
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

        onBeat = false;
        if (controller.IsOnBeat == true)
        {
            scoreboard.IncreaseComboCounter();
            controller.LastActionOnBeat = true;
            onBeat = true;
        }

        crosshairCenterRay = Camera.main.ViewportPointToRay(cameraCenterPoint);
        var temp = Physics.Raycast(crosshairCenterRay, out hit, 200f, targetLayerMask);

        if (temp == true && hit.transform.TryGetComponent(out HurtboxBehaviour current))
        {
            if (onBeat)
            {
                current.ApplyDamageToEnemy(bulletDamage * scoreboard.currentCombo * shootOnBeatMultiplier); //more dmg pls
                audioHandler.PlaySound_sourceActionAmbience(audioHandler.hitmarker_2);
            }

            else
            {
                current.ApplyDamageToEnemy(bulletDamage * scoreboard.currentCombo);
                audioHandler.PlaySound_sourceActionAmbience(audioHandler.hitmarker_1);
            }
        }
    }
}

