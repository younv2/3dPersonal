using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// ��ǲ�� �Ѱ������ϴ� Ŭ����
/// </summary>
public class InputHandler : MonoBehaviour
{
    private PlayerInput inputActions;

    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public bool IsSprint { get; private set; }
    public Action onJumpAction;
    public Action onInteractionAction;
    public Action onInventoryAction;
    public Action onChangeCameraAction;
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
        inputActions.Player.ChangeCamera.performed += OnChangeCamera;
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
        inputActions.Player.ChangeCamera.performed -= OnChangeCamera;
    }
    /// <summary>
    /// �����̴� �̺�Ʈ
    /// </summary>
    /// <param name="context"></param>
    public void OnMove(InputAction.CallbackContext context)
    {
        if(context.performed)
            MoveInput = context.ReadValue<Vector2>();
        else if(context.canceled)
            MoveInput = Vector2.zero;
    }
    /// <summary>
    /// �ٶ󺸴� �̺�Ʈ
    /// </summary>
    /// <param name="context"></param>
    public void OnLook(InputAction.CallbackContext context)
    {
        if (context.performed)
            LookInput = context.ReadValue<Vector2>();
        else if (context.canceled)
            LookInput = Vector2.zero;
    }
    /// <summary>
    /// ���� �̺�Ʈ
    /// </summary>
    /// <param name="context"></param>
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
            onJumpAction?.Invoke();
    }
    /// <summary>
    /// �޸��� �̺�Ʈ
    /// </summary>
    /// <param name="context"></param>
    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
            IsSprint = true;
        if (context.canceled)
            IsSprint = false;
    }
    /// <summary>
    /// ��ȣ�ۿ� �̺�Ʈ
    /// </summary>
    /// <param name="context"></param>
    public void OnInteraction(InputAction.CallbackContext context)
    {
        if (context.performed)
            onInteractionAction?.Invoke();
    }
    /// <summary>
    /// �κ��丮 �̺�Ʈ
    /// </summary>
    /// <param name="context"></param>
    public void OnInventory(InputAction.CallbackContext context)
    {
        if (context.performed)
            onInventoryAction?.Invoke();
    }
    /// <summary>
    /// ī�޶� ���� ���� �̺�Ʈ
    /// </summary>
    /// <param name="context"></param>
    public void OnChangeCamera(InputAction.CallbackContext context)
    {
        if (context.performed)
            onChangeCameraAction?.Invoke();
    }
}
