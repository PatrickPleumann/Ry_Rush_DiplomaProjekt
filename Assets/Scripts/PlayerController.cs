using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerMovement_New movement;

    [SerializeField] public InputActionReference move;
    [SerializeField] public InputActionReference look;
    [SerializeField] public InputActionReference jump;
    [SerializeField] public InputActionReference dash;
    [SerializeField] public InputActionReference shoot;
    [SerializeField] public InputActionReference sprint;


    private void OnEnable()
    {
        jump.action.started += movement.Jump;
    }

    private void OnDisable()
    {
        jump.action.started -= movement.Jump;
    }
}
