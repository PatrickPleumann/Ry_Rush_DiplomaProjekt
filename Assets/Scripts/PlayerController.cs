using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerMovement movement;

    [SerializeField] public InputActionReference move;
    [SerializeField] public InputActionReference look;
    [SerializeField] public InputActionReference jump;
    [SerializeField] public InputActionReference dash;
    [SerializeField] public InputActionReference shoot;
    [SerializeField] public InputActionReference wallJump;

    private void OnEnable()
    {
        jump.action.started += movement.PlayerJump;
        //move.action.started += movement.SetDesiredDirection;
    }

    private void OnDisable()
    {
        //move.action.started -= movement.SetDesiredDirection;
    }
}
