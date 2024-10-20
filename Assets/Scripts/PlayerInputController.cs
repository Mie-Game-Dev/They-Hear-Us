using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public Camera mainCamera;
    public LayerMask groundLayer;
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public Animator animator;  // Reference to the Animator component

    private PlayerInputActions playerInputActions;
    private CharacterController characterController;
    private Vector2 moveInput;
    private bool jumpInput;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        characterController = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        playerInputActions.Player.Enable();
        playerInputActions.Player.Move.performed += OnMove;
        playerInputActions.Player.Move.canceled += OnMove;
        playerInputActions.Player.Jump.performed += OnJump;
        playerInputActions.Player.Jump.canceled += OnJump;
    }

    private void OnDisable()
    {
        playerInputActions.Player.Move.performed -= OnMove;
        playerInputActions.Player.Move.canceled -= OnMove;
        playerInputActions.Player.Jump.performed -= OnJump;
        playerInputActions.Player.Jump.canceled -= OnJump;
        playerInputActions.Player.Disable();
    }

    private void Update()
    {
        if (!IsShooting())  // Only handle movement if not shooting
        {
            HandleMovement();
        }

        HandleMouseLook();
        UpdateAnimator();
    }

    private void HandleMovement()
    {
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        move = transform.TransformDirection(move) * moveSpeed;
        characterController.Move(move * Time.deltaTime);

        if (jumpInput && characterController.isGrounded)
        {
            characterController.Move(Vector3.up * jumpForce * Time.deltaTime);
        }
    }

    private void HandleMouseLook()
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
        {
            Vector3 lookAtPoint = hit.point;
            lookAtPoint.y = transform.position.y;
            transform.LookAt(lookAtPoint);

            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                ShootBullet(lookAtPoint);
            }
        }
    }

    private void ShootBullet(Vector3 target)
    {
        animator.SetTrigger("Shoot");

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
           // bulletScript.SetTarget(target);
        }
    }

    private void UpdateAnimator()
    {
        if (moveInput.sqrMagnitude > 0.01f && !IsShooting())
        {
            animator.SetFloat("Move", 1f);
        }
        else
        {
            animator.SetFloat("Move", 0f);
        }
    }

    private bool IsShooting()
    {
        // Check if the Animator's current state has the "Shoot" tag
        return animator.GetCurrentAnimatorStateInfo(0).IsTag("Shoot");
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        jumpInput = context.ReadValueAsButton();
    }
}
