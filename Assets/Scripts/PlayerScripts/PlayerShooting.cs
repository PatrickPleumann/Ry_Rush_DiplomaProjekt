using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerController controller;
    [SerializeField] private Scoreboard_UI scoreboard;
    [SerializeField] private BeatTracking beatTracking;

    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform weaponMuzzleOrigin;
    [Space]

    [SerializeField] private LayerMask targetLayerMask;
    [SerializeField] private float bulletDamage;
    [SerializeField] private float hitOnBeatMultiplier;
    

    public UnityAction OnWeaponShoot;
    private Vector3 cameraCenterPoint = new Vector3(0.5f, 0.5f, 1.0f);
    private Ray crosshairCenterRay;
    private Ray ray;
    private RaycastHit hit;
    private bool onBeat;

    public void Player_ShootWeapon(InputAction.CallbackContext context)
    {
        beatTracking.ShowCurrentSamples();
        onBeat = false;
        if (controller.IsOnBeat == true)
        {
            scoreboard.IncreaseComboCounter();
            controller.LastActionOnBeat = true;
            onBeat = true;
        }

        //controller.weaponAnimationController.SetTrigger("OnShoot");
        crosshairCenterRay = Camera.main.ViewportPointToRay(cameraCenterPoint);

        var tempBullet = Instantiate(bullet, weaponMuzzleOrigin);
        var tempBulletRB = tempBullet.GetComponent<Rigidbody>();
        tempBulletRB.AddForce(crosshairCenterRay.direction * 20, ForceMode.Impulse);

        var temp = Physics.Raycast(crosshairCenterRay, out hit, 100f, targetLayerMask);
        if (temp == true)
        {
            hit.transform.parent.TryGetComponent(out EnemyController target);

            if (onBeat == true)
            {
                Debug.Log((bulletDamage * scoreboard.currentCombo) * hitOnBeatMultiplier);
                target.TakeDamage((bulletDamage * scoreboard.currentCombo) * hitOnBeatMultiplier);
            }

            else
            {
                Debug.Log(bulletDamage * scoreboard.currentCombo);
                target.TakeDamage(bulletDamage * scoreboard.currentCombo);
            }
        }
        //animator.ResetTrigger("OnShoot");
    }
}

