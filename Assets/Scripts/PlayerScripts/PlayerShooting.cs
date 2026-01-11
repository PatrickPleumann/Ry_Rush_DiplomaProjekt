using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private Scoreboard_UI scoreboard;
    [SerializeField] private BeatTracking beatTracking;
    [Space]
    [Space]
    [SerializeField] private LayerMask targetLayerMask;
    [SerializeField] private float bulletDamage;
    [SerializeField] private float hitOnBeatMultiplier;

    public UnityAction OnWeaponShoot;
    private Vector3 cameraCenterPoint = new Vector3(0.5f, 0.5f, 1.0f);
    private Ray crosshairCenterRay;
    private Ray ray;
    private RaycastHit hit;

    public void Player_ShootWeapon(InputAction.CallbackContext context)
    {
        //animator.SetTrigger("OnShoot");
        crosshairCenterRay = Camera.main.ViewportPointToRay(cameraCenterPoint);

        var tempBullet = Instantiate(bullet, crosshairCenterRay.origin, Quaternion.Euler(transform.eulerAngles));
        var tempBulletRB = tempBullet.GetComponent<Rigidbody>();
        tempBulletRB.AddForce(crosshairCenterRay.direction * 20, ForceMode.Impulse);

        var temp = Physics.Raycast(crosshairCenterRay, out hit, 100f, targetLayerMask);
        if (temp == true)
        {
            hit.transform.parent.TryGetComponent(out EnemyController target);
            if (beatTracking.isOnBeat == true)
            {
                target.TakeDamage( (bulletDamage * scoreboard.currentCombo) * hitOnBeatMultiplier);
            }
            else
            {
                target.TakeDamage(bulletDamage * scoreboard.currentCombo);
            }
        }
        //animator.ResetTrigger("OnShoot");
    }
}

