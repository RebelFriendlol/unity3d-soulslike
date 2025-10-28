using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private Camera _playerCamera;

    public float moveSpeed = 5f;
    public float rotationSpeed = 700f;
    public bool isSprinting;

    private PlayerControls _playerControls;
    private Vector2 _movementInput;
    private bool _isMovementAllowed = true;

    private Vector3 _velocity;
    private float _gravity = -9.81f;
    private bool _isGrounded;

    // Dash
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;

    private bool isDashing = false;
    private float dashTimer = 0f;
    private Vector3 dashDirection;
    private bool dashBlocked = false; // dodane

    private Transform currentPlatform = null;
    private Vector3 lastPlatformPosition;

    public DashManager dashManager;

    // Animacja
    private Animator animator;

    public GameObject dashVFXPrefab;
    public AudioClip dashSound;
    public AudioSource audioSource;

    [Header("Komponenty walki")]
    public ComboScript comboScript; // dodane

    public static PlayerController Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        _characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _playerControls = new PlayerControls();
        _playerControls.Enable();

        _playerControls.Player.Move.performed += OnMove;
        _playerControls.Player.Move.canceled += OnMove;

        _playerControls.Player.SprintStart.performed += sprintPressed;
        _playerControls.Player.SprintStop.performed += sprintReleased;

        _playerControls.Player.Dash.performed += OnDashPerformed;
    }

    private void OnDisable()
    {
        _playerControls.Player.Move.performed -= OnMove;
        _playerControls.Player.Move.canceled -= OnMove;

        _playerControls.Player.SprintStart.performed -= sprintPressed;
        _playerControls.Player.SprintStop.performed -= sprintReleased;

        _playerControls.Player.Dash.performed -= OnDashPerformed;

        _playerControls.Disable();
    }

    private void Update()
    {
        if (isDashing)
        {
            Dash();
            return;
        }

        if (_isMovementAllowed && !ComboScript.Instance.IsAttacking)
        {
            MovePlayer();
        }
    }

    private void MovePlayer()
    {
        if (isDashing) return;

        _isGrounded = _characterController.isGrounded;

        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = 0f;
        }

        Vector3 moveDirection = new Vector3(_movementInput.x, 0, _movementInput.y);
        moveDirection = _playerCamera.transform.TransformDirection(moveDirection);
        moveDirection.y = 0f;

        _velocity.y += _gravity * Time.deltaTime;

        _characterController.Move((moveDirection * moveSpeed + _velocity) * Time.deltaTime);

        if (moveDirection != Vector3.zero && !ComboScript.Instance.IsAttacking && !isDashing)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        if (!ComboScript.Instance.IsAttacking)
        {
            bool isMoving = _movementInput.magnitude > 0.1f;
            bool isWalking = isMoving && !isSprinting;
            bool isRunning = isMoving && isSprinting;

            animator.SetBool("Walk", isWalking);
            animator.SetBool("Run", isRunning);
        }
    }

    private void Dash()
    {
        dashTimer -= Time.deltaTime;

        float adjustedDashSpeed = isSprinting ? dashSpeed * 1.25f : dashSpeed;

        _characterController.Move(dashDirection * adjustedDashSpeed * Time.deltaTime);

        if (dashTimer <= 0)
        {
            isDashing = false;
            animator.SetBool("Roll", false);
        }
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        _movementInput = context.ReadValue<Vector2>();
    }

    private void sprintPressed(InputAction.CallbackContext context)
    {
        isSprinting = true;
        moveSpeed *= 2;
    }

    private void sprintReleased(InputAction.CallbackContext context)
    {
        isSprinting = false;
        moveSpeed /= 2;
    }

    private void OnDashPerformed(InputAction.CallbackContext context)
    {
        StartDash();
    }

    public void StartDash()
    {
        if (dashBlocked || ComboScript.Instance.IsAttacking)
            return;

        if (dashManager.TryUseDash())
        {
            isDashing = true;
            dashTimer = dashDuration;

            dashDirection = new Vector3(_movementInput.x, 0, _movementInput.y);

            // Przekształcenie względem kamery, bo inaczej inne ustawienia kamery mogą powodować dziwne kierunki dashowania
            dashDirection = _playerCamera.transform.TransformDirection(dashDirection);
            dashDirection.y = 0f;

            if (dashDirection == Vector3.zero)
            {
                dashDirection = transform.forward;
            }
            dashDirection.Normalize();

            animator.SetBool("Roll", true);

            if (dashVFXPrefab != null)
            {
                Instantiate(dashVFXPrefab, transform.position, transform.rotation);
            }

            if (audioSource != null && dashSound != null)
            {
                audioSource.PlayOneShot(dashSound);
            }
        }
        else
        {
            Debug.Log("Brak dostępnych dashy!");
        }
    }

    public void SetMovementAllowed(bool isAllowed)
    {
        _isMovementAllowed = isAllowed;

        // Blokuj dash
        dashBlocked = !isAllowed;

        // Blokuj ataki
        if (comboScript != null)
        {
            comboScript.enabled = isAllowed;
        }

        // Jeśli dash był aktywny w momencie blokady
        if (!isAllowed && isDashing)
        {
            isDashing = false;
            animator.SetBool("Roll", false);
        }
    }

    private void LateUpdate()
    {
        if (currentPlatform != null)
        {
            Vector3 platformMovement = currentPlatform.position - lastPlatformPosition;
            if (platformMovement != Vector3.zero)
            {
                _characterController.Move(platformMovement);
            }
            lastPlatformPosition = currentPlatform.position;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("MovingPlatform"))
        {
            if (currentPlatform != hit.collider.transform)
            {
                currentPlatform = hit.collider.transform;
                lastPlatformPosition = currentPlatform.position;
            }
        }
        else
        {
            currentPlatform = null;
        }
    }
}
