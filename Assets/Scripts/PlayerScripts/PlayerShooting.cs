using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    public UnityAction OnWeaponShoot;
    private Vector3 cameraCenterPoint = new Vector3(0.5f, 0.5f, 1.0f);

    [SerializeField] private GameObject bullet;
    [SerializeField] private Ray crosshairCenterRay;
    [SerializeField] private LayerMask targetLayerMask;
    //[SerializeField] private BeatTracking beattracking;




    public void Player_ShootWeapon(InputAction.CallbackContext context)
    {
        //animator.SetTrigger("OnShoot");

        crosshairCenterRay = Camera.main.ViewportPointToRay(cameraCenterPoint);
        var tempBullet = Instantiate(bullet, crosshairCenterRay.origin, Quaternion.Euler(transform.eulerAngles));
        var tempBulletRB = tempBullet.GetComponent<Rigidbody>();
        tempBulletRB.AddForce(crosshairCenterRay.direction * 20, ForceMode.Impulse);

        //if (beatTracker.isOnBeat == true)
        //{
        //    Debug.Log("ON BEAT!");
        //    ScoreBoard.Instance.AddCombo();
        //    ScoreBoard.Instance.lastActionOnBeat = true;
        //}

        RaycastHit hit;
        Ray ray = Camera.main.ViewportPointToRay(cameraCenterPoint);
        var temp = Physics.Raycast(ray, out hit, 100f, targetLayerMask);
        if (temp == true)
        {
            hit.transform.parent.TryGetComponent(out EnemyController target);
            target.TakeDamage(50);
            Debug.Log("HIT!");
        }
        //animator.ResetTrigger("OnShoot");
    }
}

