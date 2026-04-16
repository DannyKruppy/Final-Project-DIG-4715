using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Vector3 pointA;
    public Vector3 pointB;

    public float speed = 2f;
    public float pauseTime = 1f;

    private Vector3 target;
    private bool isWaiting = false;

    private Vector3 lastPosition;
    public Vector3 DeltaMovement { get; private set; }

    void Start()
    {
        target = pointB;
        lastPosition = transform.position;
    }

    void Update()
    {
        if (!isWaiting)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, target) < 0.01f)
            {
                StartCoroutine(PauseAndSwitch());
            }
        }

        // Calculate movement delta
        DeltaMovement = transform.position - lastPosition;
        lastPosition = transform.position;
    }

    System.Collections.IEnumerator PauseAndSwitch()
    {
        isWaiting = true;
        yield return new WaitForSeconds(pauseTime);
        target = (target == pointA) ? pointB : pointA;
        isWaiting = false;
    }
}