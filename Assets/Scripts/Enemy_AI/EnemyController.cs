using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public Enemy_FSM<EnemyController> controller;

    [SerializeField] public Animator animator;
    [SerializeField] public Transform player;
    [SerializeField] public NavMeshAgent agent;



    private void Start()
    {
        controller = new Enemy_FSM<EnemyController>(this);
        controller.currentState.EnterState(); //point of entry
    }

    private void Update()
    {
        controller.Update();        
    }
}
