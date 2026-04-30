using UnityEngine;

public class UFO : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 8f;
    public Vector3 direction = Vector3.right;

    [Header("Path (optional)")]
    public bool useTarget = false;
    public Transform target;

    [Header("Multi Target Path")]
    public bool useMultipleTargets = false;
    public Transform[] targets;
    public bool loopTargets = true;      // A -> B -> C -> A
    public bool pingPong = false;        // A -> B -> C

    private int currentTargetIndex = 0;
    private int directionSign = 1;       // for ping-pong

    [Header("Curved Movement")]
    public bool useCurve = false;        // Enable arc motion between points
    public float curveHeight = 2f;       // How high the arc goes
    public float curveStrength = 2f;     // Multiplier for curve intensity

    private Vector3 startPoint;          // Start position of current segment
    private float journeyLength;         // Distance between points
    private float journeyProgress = 0f;  // Normalized progress (0 -> 1)

    [Header("Rotation")]
    public bool faceCamera = true;       // Always face camera
    public Vector3 baseRotationOffset = new Vector3(15f, 0f, 0f); // Tilt Adjustment
    
    public float ySpinSpeed = 90f;       // Continuous spin around Y axis

    public float wobbleAmountX = 10f;    // Side-to-side wobble
    public float wobbleAmountZ = 10f;    // Forward/back wobble
    public float wobbleSpeed = 2f;       // Speed of wobble motion

    [Header("Cleanup")]
    public bool destroyOnReach = false;  // Destroy when reaching single target
    public float destroyDistance = 0.1f; // How close counts as "arrived"

    private float currentYRotation = 0f; // Tracks spin over time
    private float wobbleTimer = 0f;      // Timer for sine wave wobble

    void Start()
    {
        // Used only for curved multi-target movement
        if (useMultipleTargets && targets.Length > 0)
        {
            startPoint = transform.position;
            journeyLength = Vector3.Distance(startPoint, targets[0].position);
        }
    }

    void Update()
    {
        // -------------------------
        // MULTI-TARGET PATH MOVEMENT
        // -------------------------
        if (useMultipleTargets && targets.Length > 0)
        {
            HandleMultiTargetMovement();
        }
        // -------------------------
        // SINGLE TARGET MOVEMENT
        // -------------------------
        else if (useTarget && target != null)
        {
            MoveToSingleTarget();
        }
        // -------------------------
        // SIMPLE DIRECTIONAL MOVEMENT
        // -------------------------
        else
        {
            transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);
        }

        // Update rotation and wobble timers
        currentYRotation += ySpinSpeed * Time.deltaTime;
        wobbleTimer += Time.deltaTime * wobbleSpeed;
    }

    // =========================================================
    // MULTI-TARGET MOVEMENT LOGIC
    // =========================================================
    void HandleMultiTargetMovement()
    {
        Transform currentTarget = targets[currentTargetIndex];

        // Straight Movement
        if (!useCurve)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                currentTarget.position,
                speed * Time.deltaTime
            );
        }
        // Curved Movement
        else
        {
            // Progress from 0 > 1 based on travel distance
            journeyProgress += (speed / Mathf.Max(journeyLength, 0.01f)) * Time.deltaTime;

            // Linear interpolation between points
            Vector3 flatPos = Vector3.Lerp(startPoint, currentTarget.position, journeyProgress);

            // Adds arc shape using sine wave
            float heightOffset = Mathf.Sin(journeyProgress * Mathf.PI) * curveHeight;
            Vector3 curvedPos = flatPos + Vector3.up * heightOffset * curveStrength;

            transform.position = curvedPos;
        }

        // Check if UFO has reached target
        float distance = Vector3.Distance(transform.position, currentTarget.position);

        if (distance < destroyDistance)
        {
            // Move to next target
            AdvanceTarget();

            // Reset curve tracking
            startPoint = transform.position;
            journeyProgress = 0f;
            journeyLength = Vector3.Distance(startPoint, targets[currentTargetIndex].position);
        }
    }

    // =========================================================
    // HANDLES TARGET INDEX SWITCHING
    // =========================================================
    void AdvanceTarget()
    {
        // -------------------------
        // PING PONG MODE (A > B > C)
        // -------------------------
        if (pingPong)
        {
            currentTargetIndex += directionSign;

            if (currentTargetIndex >= targets.Length || currentTargetIndex < 0)
            {
                directionSign *= -1;
                currentTargetIndex += directionSign * 2;
            }
        }
        // -------------------------
        // LOOP MODE (A > B > C > A)
        // -------------------------
        else if (loopTargets)
        {
            currentTargetIndex = (currentTargetIndex + 1) % targets.Length;
        }
        // -------------------------
        // ONE-WAY PATH (no loop)
        // -------------------------
        else
        {
            currentTargetIndex++;

            // Stop at end or destroy
            if (currentTargetIndex >= targets.Length)
            {
                if (destroyOnReach)
                    Destroy(gameObject);

                currentTargetIndex = targets.Length - 1;
            }
        }
    }

    // =========================================================
    // SINGLE TARGET MOVEMENT
    // =========================================================
    void MoveToSingleTarget()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime
        );

        // Destroy when reaching target (optional)
        if (destroyOnReach && Vector3.Distance(transform.position, target.position) < destroyDistance)
        {
            Destroy(gameObject);
        }
    }

    // =========================================================
    // ROTATION + VISUAL EFFECTS
    // =========================================================
    void LateUpdate()
    {
        if (Camera.main == null) return;

        // Wobble animation using sine waves
        float wobbleX = Mathf.Sin(wobbleTimer) * wobbleAmountX;
        float wobbleZ = Mathf.Cos(wobbleTimer) * wobbleAmountZ;

        if (faceCamera)
        {
            // Face camera direction with extra tilt and spin
            Quaternion baseRotation = Quaternion.LookRotation(Camera.main.transform.forward);

            Quaternion finalRotation =
                baseRotation *
                Quaternion.Euler(baseRotationOffset + new Vector3(wobbleX, 0f, wobbleZ)) *
                Quaternion.Euler(0f, currentYRotation, 0f);

            transform.rotation = finalRotation;
        }
        else
        {
            // Free rotation mode (no camera alignment)
            transform.Rotate(
                wobbleX * Time.deltaTime,
                ySpinSpeed * Time.deltaTime,
                wobbleZ * Time.deltaTime
            );
        }
    }
}