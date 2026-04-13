using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Movement Points")]
    public Vector3 pointA;
    public Vector3 pointB;

    [Header("Settings")]
    public float speed = 2f;
    public float pauseTime = 1f;

    private Vector3 target;
    private bool isWaiting = false;

    void Start()
    {
        target = pointB;
    }

    void Update()
    {
        if (isWaiting) return;

        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 0.01f)
        {
            StartCoroutine(PauseAndSwitch());
        }
    }

    System.Collections.IEnumerator PauseAndSwitch()
    {
        isWaiting = true;

        yield return new WaitForSeconds(pauseTime);

        target = (target == pointA) ? pointB : pointA;

        isWaiting = false;
    }

    // display a line between endpoint A and B
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(pointA, 0.2f);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(pointB, 0.2f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(pointA, pointB);
    }
}