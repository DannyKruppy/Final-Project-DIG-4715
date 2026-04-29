using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            respawnPoint.position = new Vector3(transform.position.x, respawnPoint.position.y, respawnPoint.position.z);
        }
    }
}
