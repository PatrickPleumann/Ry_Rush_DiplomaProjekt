using System.Collections;
using UnityEngine;

public class WeaponAnimBehaviour : MonoBehaviour
{
    private Animation anim;


    private void Awake()
    {
        anim = GetComponent<Animation>();
    }
    private void Start()
    {
        StartCoroutine(Anim());
        

    }
    private IEnumerator Anim()
    {
        yield return new WaitForSeconds(1);
        anim.Play();
        yield break;
    }
}
