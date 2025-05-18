using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private PlayerInput inputActions;

    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }

    public Action onJumpAction;
    private void Awake()
    {
        inputActions = new PlayerInput();
        inputActions.Enable();
    }
    void Start()
    {
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
        inputActions.Player.Look.performed += OnLook;
        inputActions.Player.Look.canceled += OnLook;
        inputActions.Player.Jump.performed += OnJump;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if(context.performed)
            MoveInput = context.ReadValue<Vector2>();
        else if(context.canceled)
            MoveInput = Vector2.zero;
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (context.performed)
            LookInput = context.ReadValue<Vector2>();
        else if (context.canceled)
            LookInput = Vector2.zero;
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
            onJumpAction?.Invoke();
    }
}
