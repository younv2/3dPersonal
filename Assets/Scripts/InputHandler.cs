using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private PlayerInput inputActions;

    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public bool IsSprint { get; private set; }
    public Action onJumpAction;
    public Action onInteractionAction;
    public Action onInventoryAction;
    private void Awake()
    {
        inputActions = new PlayerInput();
        inputActions.Enable();
    }
    void OnEnable()
    {
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
        inputActions.Player.Look.performed += OnLook;
        inputActions.Player.Look.canceled += OnLook;
        inputActions.Player.Sprint.performed += OnSprint;
        inputActions.Player.Sprint.canceled += OnSprint;
        inputActions.Player.Jump.performed += OnJump;
        inputActions.Player.Interaction.performed += OnInteraction;
        inputActions.Player.Inventory.performed += OnInventory;
    }
    void OnDisable()
    {
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Look.performed -= OnLook;
        inputActions.Player.Look.canceled -= OnLook;
        inputActions.Player.Sprint.performed -= OnSprint;
        inputActions.Player.Sprint.canceled -= OnSprint;
        inputActions.Player.Jump.performed -= OnJump;
        inputActions.Player.Interaction.performed -= OnInteraction;
        inputActions.Player.Inventory.performed -= OnInventory;
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
    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
            IsSprint = true;
        if (context.canceled)
            IsSprint = false;
    }
    public void OnInteraction(InputAction.CallbackContext context)
    {
        if (context.performed)
            onInteractionAction?.Invoke();
    }
    public void OnInventory(InputAction.CallbackContext context)
    {
        if (context.performed)
            onInventoryAction?.Invoke();
    }
}
