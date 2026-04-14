using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterControllerScript : MonoBehaviour
{
    private CharacterController controller;

    // JADON
    public AudioSource audioSource;
    public AudioClip jumpSound;

    // Input Actions
    public InputAction moveAction;
    //public InputAction lookAction;
    public InputAction sprintAction;

    //private Vector2 lookInput;
    private Vector2 moveInput;

    //public float mouseSens = 1;
    public float speed = 4.0f;
    public float sprintSpeed = 7.0f;
    public float gravity = -9.8f;
    public float jumpVel = 40;
    Vector3 velocity;

    private Vector3 currentVelocity;
    public float acceleration = 10f;
    public float deceleration = 12f;

    //public Transform mainCamera;

    //private float xRotation;

    public float maxSprint = 8f;
    public float drainRate = 1f;
    public float rechargeRate = 0.5f;
    public float sprintDelay = 2f;

    private float sprint;
    private float counter;

    public GameObject sprintBar;
    private Image sprintBarImage;

    void OnEnable()
    {
        moveAction.Enable();
        //lookAction.Enable();
        sprintAction.Enable();
    }

    void OnDisable()
    {
        moveAction.Disable();
        //lookAction.Disable();
        sprintAction.Disable();
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        sprintBarImage = sprintBar.GetComponent<Image>();

        sprint = maxSprint;
    }

    void Update()
    {
        Movement();
        //Look();
        Gravity();

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Jump();
        }

        Recharge();

        sprintBarImage.fillAmount = sprint / maxSprint;

        /*if (Keyboard.current.escapeKey.wasPressedThisFrame)
            SceneManager.LoadSceneAsync("Title");*/
    }

    void Movement()
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

        controller.Move(currentVelocity * Time.deltaTime);
    }

    /*void Look()
    {
        lookInput = lookAction.ReadValue<Vector2>();

        float lookX = lookInput.x * mouseSens;
        float lookY = lookInput.y * mouseSens;

        transform.Rotate(Vector3.up * lookX);

        xRotation -= lookY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        mainCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }*/

    void Gravity()
    {
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
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
            audioSource.PlayOneShot(jumpSound); // JADON
        }
    }
}