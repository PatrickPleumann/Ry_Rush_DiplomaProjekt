using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    [SerializeField] public float damage = 5f;
    [SerializeField] private float despawnTime = 3;

    [Space]
    public bool shotOnBeat = false;
    public int dmgMultiplier = 1;
    public float onBeatMultiplier = 1.5f;


    private void Update()
    {
        despawnTime -= Time.deltaTime;
        if (despawnTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void SetValues(bool _onBeat, int _dmgMultiplier)
    {
        shotOnBeat = _onBeat;
        dmgMultiplier = _dmgMultiplier;
    }

    public float CalculateCurrentDmg()
    {
        float temp;
        if (shotOnBeat)
            temp = (float)damage * dmgMultiplier * onBeatMultiplier;
        else
            temp = (float)damage * dmgMultiplier;

        return temp;
    }
}
