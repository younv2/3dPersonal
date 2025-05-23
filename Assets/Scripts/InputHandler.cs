using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 인풋을 총괄관리하는 클래스
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
    /// 움직이는 이벤트
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
    /// 바라보는 이벤트
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
    /// 점프 이벤트
    /// </summary>
    /// <param name="context"></param>
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
            onJumpAction?.Invoke();
    }
    /// <summary>
    /// 달리기 이벤트
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
    /// 상호작용 이벤트
    /// </summary>
    /// <param name="context"></param>
    public void OnInteraction(InputAction.CallbackContext context)
    {
        if (context.performed)
            onInteractionAction?.Invoke();
    }
    /// <summary>
    /// 인벤토리 이벤트
    /// </summary>
    /// <param name="context"></param>
    public void OnInventory(InputAction.CallbackContext context)
    {
        if (context.performed)
            onInventoryAction?.Invoke();
    }
    /// <summary>
    /// 카메라 시점 변경 이벤트
    /// </summary>
    /// <param name="context"></param>
    public void OnChangeCamera(InputAction.CallbackContext context)
    {
        if (context.performed)
            onChangeCameraAction?.Invoke();
    }
}
