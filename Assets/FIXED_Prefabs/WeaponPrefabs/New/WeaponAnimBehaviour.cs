using System.Collections;
using UnityEngine;

public class WeaponAnimBehaviour : MonoBehaviour
{
    [SerializeField] private Animation shootAnim;

    private void Start()
    {
        StartCoroutine(Anim());
    }
    private IEnumerator Anim()
    {
        yield return new WaitForSeconds(1);
        shootAnim.Play();
        yield break;
    }
}
