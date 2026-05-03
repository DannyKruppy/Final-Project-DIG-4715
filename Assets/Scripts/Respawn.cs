using UnityEngine;

public class Respawn : MonoBehaviour
{
    public Transform respawnPoint;
    public float threshold = -5f;
    public AudioSource respawnSound;

    CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (transform.position.y < threshold)
        {
            RespawnPlayer();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hazard"))
        {
            RespawnPlayer();
        }
    }

    void RespawnPlayer()
    {
        if (respawnSound != null)
        {
            respawnSound.Play();
        }

        controller.enabled = false;
        transform.position = respawnPoint.position;
        controller.enabled = true;
    }
}