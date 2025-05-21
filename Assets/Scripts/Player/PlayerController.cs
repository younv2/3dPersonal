using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    private Vector2 moveInput;
    [Header("Look")]
    public Transform cameraContainer;
    public Transform tpCameraContainer;
    private bool isTp = false;
    public float minXLook;
    public float maxXLook;
    private float camCurXRot;
    public float lookSensitivity;
    private Vector2 mouseDelta;

    private PlayerStat stat;
    private Rigidbody rb;
    private Interaction interaction;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private InputHandler inputHandler;

    public bool canLook = true;
    public InputHandler InputHandler { get { return inputHandler; } }
    private int jumpCount = 0;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        interaction = GetComponent<Interaction>();
        
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        
    }
    private void OnEnable()
    {
        inputHandler.onJumpAction += Jump;
        inputHandler.onInteractionAction += interaction.OnInteractInput;
        inputHandler.onInventoryAction += ToggleCursor;
        inputHandler.onChangeCameraAction += ChangeCamera;
    }
    private void OnDisable()
    {
        inputHandler.onJumpAction -= Jump;
        inputHandler.onInteractionAction -= interaction.OnInteractInput;
        inputHandler.onInventoryAction -= ToggleCursor;
        inputHandler.onChangeCameraAction -= ChangeCamera;
    }
    private void FixedUpdate()
    {
        moveInput = inputHandler.MoveInput;
        if (moveInput == Vector2.zero)
            return;
        Move();
        Climb();
        stat.RecoveryStamina(0.02f);
    }
    private void LateUpdate()
    {
        mouseDelta = inputHandler.LookInput;
        if(canLook)
            CameraLook();
    }

    public void Move()
    {
        Vector3 dir = transform.forward * moveInput.y + transform.right * moveInput.x;
        
        dir *= (inputHandler.IsSprint && stat.SpendStamina(1f)) ? stat.RunSpeed : stat.MoveSpeed;
        dir.y = rb.velocity.y;
        rb.velocity = dir;
        Debug.DrawRay(transform.position, transform.forward*1f, Color.red);
        
    }

    public void Climb()
    {
        if (Physics.Raycast(transform.position, transform.forward, out var hit, 1f, LayerMask.GetMask(LayerString.Wall)))
        {
            rb.useGravity = false;
        }
        else
        {
            rb.useGravity = true;
            return;
        }

        if (!stat.SpendStamina(0.1f))
        {
            rb.useGravity = true;
            return;
        }

        rb.velocity = Vector3.zero;
        rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);
    }
    public void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        if(!isTp)
            cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);
        else
            tpCameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);
        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }
    public void ChangeCamera()
    {
        if(isTp)
        {
            tpCameraContainer.gameObject.SetActive(false);
            cameraContainer.gameObject.SetActive(true);
            isTp = false;
        }
        else
        {
            cameraContainer.gameObject.SetActive(false);
            tpCameraContainer.gameObject.SetActive(true);
            isTp = true;
        }
        
    }

    public void Jump()
    {
        bool isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, LayerMask.GetMask(LayerString.Ground));

        if (isGrounded)
        {
            jumpCount = 0; // 착지했으면 점프 카운트 초기화
        }

        // 점프 가능 횟수 초과시 차단
        if (jumpCount >= stat.JumpCount)
            return;

        // 스태미나 부족
        if (!stat.SpendStamina(5f))
            return;

        // 점프 실행
        rb.AddForce(Vector3.up * stat.JumpPower, ForceMode.Impulse);
        jumpCount++;
    }

    internal void Init(PlayerStat playerStat)
    {
        stat = playerStat;
    }
    private void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }
}
