using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterControllerScript : MonoBehaviour
{
    private CharacterController controller;

    public AudioSource audioSource;
    public AudioClip jumpSound;

    [SerializeField] private ParticleSystem jumpParticles;

    public InputAction moveAction;
    public InputAction sprintAction;
    public InputAction jumpAction;

    private Vector2 moveInput;

    public float speed = 4.0f;
    public float sprintSpeed = 7.0f;
    public float gravity = -9.8f;
    public float jumpVel = 40;

    Vector3 velocity;
    private Vector3 currentVelocity;

    public float acceleration = 10f;
    public float deceleration = 12f;

    public float maxSprint = 8f;
    public float drainRate = 1f;
    public float rechargeRate = 0.5f;
    public float sprintDelay = 2f;

    private float sprint;
    private float counter;

    public GameObject sprintBar;
    private Image sprintBarImage;

    private MovingPlatform currentPlatform;

    void OnEnable()
    {
        moveAction.Enable();
        jumpAction.Enable();
        sprintAction.Enable();
    }

    void OnDisable()
    {
        moveAction.Disable();
        jumpAction.Disable();
        sprintAction.Disable();
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();

        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;

        sprintBarImage = sprintBar.GetComponent<Image>();
        sprint = maxSprint;
    }

    void Update()
    {
        if (!controller.isGrounded)
        {
            currentPlatform = null;
        }
        else
        {
            ValidatePlatform();
        }

        Vector3 move = Movement();
        Vector3 grav = Gravity();
        Vector3 platformMove = GetPlatformMovement();

        controller.Move((move + grav) * Time.deltaTime + platformMove);

        if (jumpAction.WasPressedThisFrame())
        {
            Jump();
        }

        Recharge();
        sprintBarImage.fillAmount = sprint / maxSprint;
    }

    Vector3 Movement()
    {
        moveInput = moveAction.ReadValue<Vector2>();

        float forwardInput = moveInput.y;
        float sideInput = moveInput.x;

        Vector3 moveDirection = (transform.right * sideInput + transform.forward * forwardInput).normalized;

        bool isMoving = moveInput.magnitude > 0.1f;

        float targetSpeed = speed;

        if (sprintAction.IsPressed() && isMoving && sprint > 0)
        {
            targetSpeed = sprintSpeed;
            sprint -= drainRate * Time.deltaTime;
            counter = 0;
        }

        Vector3 targetVelocity = moveDirection * targetSpeed;
        float rate = isMoving ? acceleration : deceleration;

        currentVelocity = Vector3.MoveTowards(currentVelocity, targetVelocity, rate * Time.deltaTime);

        return currentVelocity;
    }

    Vector3 Gravity()
    {
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;

        return velocity;
    }

    Vector3 GetPlatformMovement()
    {
        if (currentPlatform != null && controller.isGrounded)
        {
            return currentPlatform.DeltaMovement;
        }

        return Vector3.zero;
    }

    void Recharge()
    {
        if (sprint >= maxSprint)
        {
            sprint = maxSprint;
            return;
        }

        counter += Time.deltaTime;

        if (counter >= sprintDelay)
        {
            sprint += rechargeRate * Time.deltaTime;
        }
    }

    void Jump()
    {
        if (controller.isGrounded)
        {
            velocity.y = jumpVel;
            jumpParticles.Play();
            audioSource.PlayOneShot(jumpSound);
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.TryGetComponent<MovingPlatform>(out MovingPlatform platform))
        {
            currentPlatform = platform;
            Debug.Log("Platform detected");
        }
    }

    void ValidatePlatform()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        float distance = controller.height / 2f + 0.3f;

        if (Physics.Raycast(ray, out RaycastHit hit, distance))
        {
            if (hit.collider.TryGetComponent<MovingPlatform>(out MovingPlatform platform))
            {
                currentPlatform = platform;
                return;
            }
        }

        currentPlatform = null;
    }
}