using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerController controller;
    [SerializeField] private Scoreboard_UI scoreboard;
    [SerializeField] private BeatTracking beatTracking;
    [SerializeField] private float projectileForce;
    [SerializeField] private AlignWeaponToCrosshair alignWeaponToCrosshair;

    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform weaponMuzzleOrigin;
    [Space]

    [SerializeField] private LayerMask targetLayerMask;
    [SerializeField] private float bulletDamage;
    [SerializeField] private float hitOnBeatMultiplier;
    

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

    public void Player_ShootWeapon(InputAction.CallbackContext context)
    {
        alignWeaponToCrosshair.AlignMuzzleToCrosshairMiddlePoint();

        onBeat = false;
        if (controller.IsOnBeat == true)
        {
            scoreboard.IncreaseComboCounter();
            controller.LastActionOnBeat = true;
            onBeat = true;
        }

        crosshairCenterRay = Camera.main.ViewportPointToRay(cameraCenterPoint);

        var bullet_Prefab = Instantiate(bullet);
        bullet_Prefab.transform.position = weaponMuzzleOrigin.position;
        bullet_Prefab.transform.rotation = weaponMuzzleOrigin.rotation;

        var bullet_RB = bullet_Prefab.GetComponent<Rigidbody>();

        var projectileBehaviour = bullet_Prefab.GetComponent<ProjectileBehaviour>();
        projectileBehaviour.SetValues(onBeat, (int)scoreboard.currentCombo);

        bullet_RB.AddForce(weaponMuzzleOrigin.forward * projectileForce, ForceMode.Impulse);
    }
}

