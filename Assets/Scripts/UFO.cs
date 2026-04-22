using UnityEngine;

public class UFO : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 8f;
    public Vector3 direction = Vector3.right;

    [Header("Path (optional)")]
    public bool useTarget = false;
    public Transform target;

    [Header("Rotation")]
    public bool faceCamera = true;

    // Base tilt
    public Vector3 baseRotationOffset = new Vector3(15f, 0f, 0f);

    // Y spin (continuous)
    public float ySpinSpeed = 90f;

    // X/Z wobble (small range)
    public float wobbleAmountX = 10f;
    public float wobbleAmountZ = 10f;
    public float wobbleSpeed = 2f;

    [Header("Cleanup")]
    public bool destroyOnReach = false;
    public float destroyDistance = 0.1f;

    private float currentYRotation = 0f;
    private float wobbleTimer = 0f;

    void Update()
    {
        // Movement
        if (useTarget && target != null)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                target.position,
                speed * Time.deltaTime
            );

            if (destroyOnReach && Vector3.Distance(transform.position, target.position) < destroyDistance)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);
        }

        // Update spin + wobble timers
        currentYRotation += ySpinSpeed * Time.deltaTime;
        wobbleTimer += Time.deltaTime * wobbleSpeed;
    }

    void LateUpdate()
    {
        if (Camera.main == null) return;

        // Calculate wobble using sine wave
        float wobbleX = Mathf.Sin(wobbleTimer) * wobbleAmountX;
        float wobbleZ = Mathf.Cos(wobbleTimer) * wobbleAmountZ;

        if (faceCamera)
        {
            Quaternion baseRotation = Quaternion.LookRotation(Camera.main.transform.forward);

            // Combine base tilt + wobble + spin
            Quaternion finalRotation =
                baseRotation *
                Quaternion.Euler(baseRotationOffset + new Vector3(wobbleX, 0f, wobbleZ)) *
                Quaternion.Euler(0f, currentYRotation, 0f);

            transform.rotation = finalRotation;
        }
        else
        {
            // Free rotation version
            transform.Rotate(
                wobbleX * Time.deltaTime,
                ySpinSpeed * Time.deltaTime,
                wobbleZ * Time.deltaTime
            );
        }
    }
}