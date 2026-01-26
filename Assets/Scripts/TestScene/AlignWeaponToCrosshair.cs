using UnityEngine;

public class AlignWeaponToCrosshair : MonoBehaviour
{
    private Vector3 cameraCenterPoint = new Vector3(0.5f, 0.5f, 1.0f);
    private Ray centerPoint;
    private RaycastHit hit;
    [SerializeField] private GameObject weapon;

    private void FixedUpdate()
    {
        AlignMuzzleToCrosshairMiddlePoint();
    }
    

    private void AlignMuzzleToCrosshairMiddlePoint()
    {
        centerPoint = Camera.main.ViewportPointToRay(cameraCenterPoint);
        var temp = Physics.Raycast(centerPoint, out hit, 200f);
        if (temp == true)
        {
            weapon.transform.LookAt(hit.point);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(centerPoint);
    }
}
