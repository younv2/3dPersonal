using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    private Vector2 moveInput;
    [Header("Look")]
    public Transform CameraContainer;
    public float minXLook;
    public float maxXLook;
    private float camCurXRot;
    public float lookSensitivity;
    private Vector2 mouseDelta;

    private PlayerStat stat;
    private Rigidbody rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private InputHandler inputHandler;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        inputHandler.onJumpAction += Jump;
    }
    private void FixedUpdate()
    {
        moveInput = inputHandler.MoveInput;
        if (moveInput == Vector2.zero)
            return;
        Move();
        stat.RecoveryStamina(0.02f);
    }
    private void LateUpdate()
    {
        mouseDelta = inputHandler.LookInput;
        CameraLook();
    }

    public void Move()
    {
        Vector3 dir = transform.forward * moveInput.y + transform.right * moveInput.x;
        
        dir *= (inputHandler.IsSprint && stat.SpendStamina(1f)) ? stat.RunSpeed : stat.MoveSpeed;
        dir.y = rb.velocity.y;
        rb.velocity = dir;
    }
    public void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        CameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }
    

    public void Jump()
    {
        if (!Physics.CheckSphere(groundCheck.position, 0.2f, LayerMask.GetMask("Ground")))
            return;

        if (!stat.SpendStamina(5f))
            return;

        rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);
    }

    internal void Init(PlayerStat playerStat)
    {
        stat = playerStat;
    }
}
