using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    Vector3 lastPos;

    void Start()
    {
        lastPos = transform.position;
    }

    void Update()
    {
        Vector3 dir = transform.position - lastPos;
        dir.y = 0f;

        if (dir.sqrMagnitude > 0.0001f)
            transform.rotation = Quaternion.LookRotation(dir);

        lastPos = transform.position;
    }
}
